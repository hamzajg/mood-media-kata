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

app.UseHttpsRedirection();

app.MapGet("/api/companies", Array.Empty<object>)
    .WithName("GetCompany")
    .WithOpenApi();

app.MapPost("/api/companies", async ([FromBody] CreateNewCompanyMessage message) =>
    {
        using var messageBus = RabbitHutch.CreateBus("host=127.0.0.1:5672;username=guest;password=guest");
        await messageBus.PubSub.PublishAsync( message, "Q.MoodMediaKata");
    })
    .WithName("PostCompany")
    .WithOpenApi();
app.MapDelete("/api/company/{id}/devices", async ( [FromRoute] long id,  [FromBody] DeleteDevicesMessage message) =>
    {
        using var messageBus = RabbitHutch.CreateBus("host=127.0.0.1:5672;username=guest;password=guest");
        await messageBus.PubSub.PublishAsync(message, "Q.MoodMediaKata");
    })
    .WithName("DeleteDevices")
    .WithOpenApi();

app.Run();
