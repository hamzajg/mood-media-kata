using EasyNetQ;
using EasyNetQ.Topology;
using MoodMediaKata;
using MoodMediaKata.Infra;

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

List<CreateCompanyRequest> companies = new List<CreateCompanyRequest>();
app.MapGet("/api/companies", () =>
    {
        return companies;
    })
    .WithName("GetCompany")
    .WithOpenApi();
app.MapPost("/api/companies", async (CreateCompanyRequest createCompanyRequest) =>
    {
        using var messageBus = RabbitHutch.CreateBus("host=127.0.0.1:5672;username=guest;password=guest");
        await messageBus.PubSub.PublishAsync( new CreateCompanyCommand(){Name = createCompanyRequest.Name}, "Q.MoodMediaKata");
        //await messageBus.PubSub.PublishAsync( new CreateCompanyCommand(){Name = createCompanyRequest.Name});
        companies.Add(createCompanyRequest);
    })
    .WithName("PostCompany")
    .WithOpenApi();

app.Run();

public class CreateCompanyRequest
{
    public string Name { get; set; }
    public string Code { get; set; }
    public DeviceDto[] Devices { get; set; }
}

public record DeviceDto
{
    public string SerialNumber { get; set; }
    public string Type { get; set; }
}