using MagicCrypto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;
using AwesomePizzaDAL;
using GenericUnitOfWork.UoW;
using Microsoft.OpenApi.Models;
using AwesomePizzaAPI;
using AwesomePizzaAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment env = builder.Environment;

builder.Services.AddHttpContextAccessor();
builder.Services.RegisterDataServices(configuration, env);
builder.Services.AddDataRepository();
builder.Services.RegisterBusinessServices(env);
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddAuthenticationService(configuration, env);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Token");
        });
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = nameof(AwesomePizzaAPI), Version = "v1" });
    // Aggiunge l'opzione per l'autenticazione tramite header
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticazione JWT tramite header. Inserire il token JWT ottenuto nel header della risposta della richiesta di login.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});
var app = builder.Build();

app.UseCors("AllowAll");
if (app.Environment.IsDevelopment())
{
    app.MapControllers().AllowAnonymous();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", nameof(AwesomePizzaAPI));
    c.RoutePrefix = string.Empty;
});

// Configure the HTTP request pipeline.
app.MapDefaultControllerRoute();
app.UseHttpsRedirection();
app.UseMiddleware<JwtTokenMiddleware>();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.InitializeContext();

app.Run();

public partial class Program { }
