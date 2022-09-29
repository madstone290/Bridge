using Bridge.Api.ActionFilters;
using Bridge.Api.Constants;
using Bridge.Application;
using Bridge.Infrastructure;
using Bridge.Infrastructure.Identity.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
    builder.Configuration.AddJsonFile("Secrets/db_context_prod_secret.json");
else
    builder.Configuration.AddJsonFile("Secrets/db_context_dev_secret.json");

builder.Configuration.AddJsonFile("Secrets/mail_service_config.json");
builder.Configuration.AddJsonFile("Secrets/token_service_config.json");

builder.Services.AddControllers(options =>
{
    var noContentFormatter = options.OutputFormatters.OfType<HttpNoContentOutputFormatter>().FirstOrDefault();
    if (noContentFormatter != null)
    {
        noContentFormatter.TreatNullValueAsNoContent = false;
    }
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var jwtConfig = builder.Configuration.GetSection("TokenService").Get<TokenService.Config>();
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .Build();

    options.AddPolicy(PolicyConstants.AdminOrProvider, policyBuilder =>
    {
        policyBuilder.RequireClaim(ClaimTypeConstants.UserType, ClaimConstants.Admin, ClaimConstants.Provider);
        var dd = policyBuilder.AuthenticationSchemes;
        policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    });
});

// Swagger API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
            {
                string securityName = "Bearer";
                options.AddSecurityDefinition(securityName, new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Bearer {token} 형식으로 입력하세요",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = securityName
                            }
                        },
                        new string[] {}
                    }
                });
                options.CustomSchemaIds(type => type.ToString()); // Type의 FullName을 이용해서 스키마를 식별한다.

            });

builder.Services.AddMediatR(typeof(ApplicationReference).Assembly);
builder.Services.AddInfrastructureDependency(builder.Configuration);
builder.Services.AddScoped<ExceptionFilter>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
