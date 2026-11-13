using System.Collections.Concurrent;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using MoodMediaKata.App;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAllOrigins");
    app.UseSwagger();
    app.UseSwaggerUI();
}
using var messageBus = RabbitHutch.CreateBus("host=127.0.0.1:5672;username=guest;password=guest");

// One subscription for all company queries, correlated by request id.
// Subscribing per request used to leak a consumer per call and left
// requests hanging forever when no result ever arrived.
var pendingQueries = new ConcurrentDictionary<Guid, TaskCompletionSource<CompanyDto?>>();
await messageBus.PubSub.SubscribeAsync<QueryCompanyByIdResultMessage>(
    QueryCompanyByIdResultMessage.Queue,
    msg =>
    {
        if (pendingQueries.TryRemove(msg.RequestId, out var tcs))
            tcs.TrySetResult(msg.Company);
    });

app.UseHttpsRedirection();

app.MapGet("/api/companies/{id}", async ([FromRoute] long id) =>
    {
        var query = new QueryCompanyByIdMessage { CompanyId = id };
        var tcs = new TaskCompletionSource<CompanyDto?>(TaskCreationOptions.RunContinuationsAsynchronously);
        pendingQueries[query.Id] = tcs;
        try
        {
            await messageBus.PubSub.PublishAsync(query, "Q.MoodMediaKata");
            var completed = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(10)));
            if (completed != tcs.Task)
                return Results.StatusCode(StatusCodes.Status504GatewayTimeout);
            var company = await tcs.Task;
            return company is null ? Results.NotFound() : Results.Ok(company);
        }
        finally
        {
            pendingQueries.TryRemove(query.Id, out _);
        }
    })
    .WithName("GetCompanyById")
    .WithOpenApi();

app.MapPost("/api/companies", async ([FromBody] CreateNewCompanyMessage message) =>
    {
        await messageBus.PubSub.PublishAsync(message, "Q.MoodMediaKata");
    })
    .WithName("PostCompany")
    .WithOpenApi();

app.MapDelete("/api/company/{id}/devices", async ([FromRoute] long id, [FromBody] DeleteDevicesMessage message) =>
    {
        await messageBus.PubSub.PublishAsync(message, "Q.MoodMediaKata");
    })
    .WithName("DeleteDevices")
    .WithOpenApi();

app.Run();
