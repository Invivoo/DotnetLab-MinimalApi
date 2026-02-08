using DotnetLab_MinimalApi.Dto;
using DotnetLab_MinimalApi.Endpoints;
using DotnetLab_MinimalApi.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ICommunityMapper, CommunityMapper>();
builder.Services.AddSingleton<ICommunityRepository, InMemoryCommunityRepository>();
builder.Services.AddSingleton<ICommunityGetService, CommunityGetService>();
builder.Services.AddSingleton<ICommunityPostService, CommunityPostService>();
builder.Services.AddSingleton<ICommunityDeleteService, CommunityDeleteService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    AddOpenApiDocumentationAndClients(app);
}

app.UseHttpsRedirection();

app.AddCommunityEndpoints();
app.MapControllers();

app.Run();


static void AddOpenApiDocumentationAndClients(WebApplication app)
{
    app.MapOpenApi();

    //To serve Scalar UI at: /open-api-client/scalar
    app.MapScalarApiReference("/open-api-client/scalar", options =>
    {
        options
            .WithTheme(ScalarTheme.BluePlanet)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });

    //To serve Swagger UI at: /open-api-client/swagger
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "Open API Swagger");
        c.RoutePrefix = "open-api-client/swagger";
    });

    //To serve ReDoc UI at: /open-api-client/redoc
    app.UseReDoc(c =>
    {
        c.SpecUrl = "/openapi/v1.json";
        c.RoutePrefix = "open-api-client/redoc";
    });
}
