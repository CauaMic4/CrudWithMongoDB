using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectMongo.Application.Business;
using ProjectMongo.Application.Business.Implementations;
using ProjectMongo.Application.Configurations;
using ProjectMongo.Application.Converter.Contract;
using ProjectMongo.Application.Converter.Implementations;
using ProjectMongo.Application.Services;  
using ProjectMongo.Application.VOs;
using ProjectMongo.Domain.Entities;
using ProjectMongo.Domain.Repositories;
using ProjectMongo.Infrastructure.Context;
using ProjectMongo.Infrastructure.Repositories;
using ProjectMongo.Infrastructure.Services;       
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var tokenConfiguration = new TokenConfiguration();
new ConfigureFromConfigurationOptions<TokenConfiguration>(
    builder.Configuration.GetSection("TokenConfiguration"))
        .Configure(tokenConfiguration);


builder.Services.AddSingleton(tokenConfiguration);

// JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = tokenConfiguration.Issuer,
        ValidAudience = tokenConfiguration.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.Secret))
    };
});


builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build()
    );
});

//CORS
builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

//MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings")
);
builder.Services.AddSingleton<MongoDbContext>();

// Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoginBusiness, LoginBusinessImplementation>(); 
builder.Services.AddScoped<IParser<UserVO, User>, UserConverter>();
builder.Services.AddScoped<IParser<User, UserVO>, UserConverter>();
builder.Services.AddScoped<IParser<LoginVO, User>, LoginVoToUserConverter>();
builder.Services.AddScoped<IUserBusiness, UserBusinessImplementation>();

builder.Services.AddTransient<ITokenService, TokenService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ProjectMongo API",
        Version = "v1",
        Description = "API REST com .NET 8 e MongoDB",
        Contact = new OpenApiContact
        {
            Name = "Cauã Micael",
            Url = new Uri("https://github.com/CauaMic4")
        }
    });


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectMongo API v1");
    });
}

app.UseHttpsRedirection();

// Adiciona o CORS ao pipeline
app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();