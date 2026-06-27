using System.Security.Claims;
using DataAccess.Contracts;
using DataAccess.Repositories.InMemory;
using Domain.Contracts;
using Domain.Services;
using Host.Endpoints;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddValidation();
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.Authority = "http://localhost:8080/realms/gt_dotnet_lab";
    options.Audience = "account"; //(usually should be the client ID of your application or the api name but that needs to be set in the client and keycloack is defaulting it to account)
    options.RequireHttpsMetadata = false; // dev only
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddSingleton<ICommunityRepository, InMemoryCommunityRepository>();
builder.Services.AddSingleton<ICommunityService, CommunityService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

const string OpenApiDocumentName = "v1";
const string OpenApiDocumentTitle = "Minimal Api Tuto";
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(OpenApiDocumentName, new OpenApiInfo
    {
        Title = OpenApiDocumentTitle,
        Version = "V1.0"
    });
    ConfigureSwaggerGenForOAuth2(options);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("users/me", GetCurrentUserClaims).RequireAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    AddOpenApiDocumentationAndClients(app);
}

app.AddCommunityEndpoints();
app.MapControllers();

app.Run();

static void AddOpenApiDocumentationAndClients(WebApplication app)
{
    
    const string OpenApiDocumentPathTemplate = "/open-api/documentations/{0}/open-api.json";
    var openApiRoutePattern = string.Format(OpenApiDocumentPathTemplate, "{documentName}");
    var openApiDocumentPath = string.Format(OpenApiDocumentPathTemplate, OpenApiDocumentName);
    app.UseSwagger(options => {
        options.RouteTemplate = openApiRoutePattern;
    });

    //To serve Scalar UI at: /open-api/clients/scalar
    app.MapScalarApiReference(OpenApiClientRoutePrefix("scalar"), options =>
    {
        options
            .WithOpenApiRoutePattern(openApiRoutePattern)
            .WithTheme(ScalarTheme.BluePlanet)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .AddDocument(OpenApiDocumentName);
    });

    //To serve Swagger UI at: /open-api/clients/swagger
    app.UseSwaggerUI(c =>
    {
        c.SwaggerDocumentUrlsPath = openApiRoutePattern;
        c.RoutePrefix = OpenApiClientRoutePrefix("swagger");
        c.SwaggerEndpoint(openApiDocumentPath, OpenApiDocumentTitle);

        ConfigureSwaguerUIForOAuth2(c);
    });

    //To serve ReDoc UI at: /open-api/clients/redoc
    app.UseReDoc(c =>
    {
        c.RoutePrefix = OpenApiClientRoutePrefix("redoc");
        c.SpecUrl = openApiDocumentPath;
        c.DocumentTitle = OpenApiDocumentTitle;
    });

    static string OpenApiClientRoutePrefix(string clientName) => $"open-api/clients/{clientName}";
}

static void ConfigureSwaggerGenForOAuth2(SwaggerGenOptions options)
{
    Dictionary<string, string> scopes = new()
    {
        { "openid", "openid"}, { "profile", "profile" }, {"email", "email"}
    };
    const string securitySchemeIdAndDisplayName = "Oauth2";
    options.AddSecurityDefinition(securitySchemeIdAndDisplayName, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "OAuth2 authorization using local keyCloack token issuer.",
        Type = SecuritySchemeType.OAuth2,

        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("http://localhost:8080/realms/gt_dotnet_lab/protocol/openid-connect/auth"),
                TokenUrl = new Uri("http://localhost:8080/realms/gt_dotnet_lab/protocol/openid-connect/token"),
                Scopes = scopes
            }
        },
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference(securitySchemeIdAndDisplayName, document)] = [.. scopes.Keys]
    });
}

static void ConfigureSwaguerUIForOAuth2(Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIOptions c)
{
    c.OAuthClientId("GT-DotnetLab-MinimalApi");
    c.OAuth2RedirectUrl("https://localhost:7027/open-api/clients/swagger/oauth2-redirect.html");
    c.OAuthUsePkce();
    c.OAuthClientSecret("");
}

static IDictionary<string, string> GetCurrentUserClaims(ClaimsPrincipal claimsPrincipal) => 
    claimsPrincipal.Claims
    .Where(x => x.Type.StartsWith("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/"))
    .ToDictionary(c => c.Type, c => c.Value);