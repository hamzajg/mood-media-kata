using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using MoodMediaKata.App;
using JsonSerializer = System.Text.Json.JsonSerializer;

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

app.UseHttpsRedirection();

app.MapGet("/api/companies/{id}", async ([FromRoute] long id) =>
    {
        var source = new TaskCompletionSource<CompanyDto>();
        await messageBus.PubSub.PublishAsync(new QueryCompanyByIdMessage{CompanyId = id}, "Q.MoodMediaKata");
        await messageBus.PubSub.SubscribeAsync<QueryCompanyByIdResultMessage>("Q.MoodMediaKata",
            msg =>
            {
                source.SetResult(msg.Company);;
            });
        return await source.Task;
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
