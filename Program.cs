using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Refit;
using SistemaTarefa.Data;
using SistemaTarefa.Integration;
using SistemaTarefa.Integration.Interfaces;
using SistemaTarefa.Integration.Refit;
using SistemaTarefa.Repositories;
using SistemaTarefa.Repositories.Interfaces;
using SistemaTarefa.Services;
using SistemaTarefa.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"] != null ? builder.Configuration["Jwt:Key"]  : null;
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SistemaTarefa API", Version = "v1" });

    var securitySchema = new OpenApiSecurityScheme
    {
        
        Name = "Authorization",
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { JwtBearerDefaults.AuthenticationScheme } }
    });
});

// Database Configuration
builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<SistemaTarefasDBContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DataBase"))
    );

// Dependency Injection for Repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();


// Dependency Injection for  Services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ITarefaService, TarefaService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IViaCepIntegracao, ViaCepIntegracao>();
builder.Services.AddRefitClient<IViaCepIntegracaoRefit>().ConfigureHttpClient(c =>
{
    c.BaseAddress = new Uri("https://viacep.com.br");
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()

{    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = jwtIssuer,
    ValidAudience = jwtAudience,
    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey)),
    ClockSkew = TimeSpan.Zero 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication Middleware
app.UseAuthentication();


// Authorization Middleware
app.UseAuthorization();

app.MapControllers();

app.Run();
