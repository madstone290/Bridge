using Bridge.Api.ActionFilters;
using Bridge.Api.Extensions;
using Bridge.Api.Middlewares;
using Bridge.Application;
using Bridge.Infrastructure;
using Bridge.Infrastructure.Identity.Services;
using Bridge.Shared.ApiContract;
using Bridge.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.HandleArgs(args);

if (builder.Environment.IsProduction())
    builder.Configuration.AddJsonFile("Secrets/db_production_config.json");
else
    builder.Configuration.AddJsonFile("Secrets/db_development_config.json");

builder.Configuration.AddJsonFile("Secrets/mail_service_config.json");
builder.Configuration.AddJsonFile("Secrets/token_service_config.json");
builder.Configuration.AddJsonFile("Secrets/geocode_api_config.json");

builder.Services.AddControllers(options =>
{
    var noContentFormatter = options.OutputFormatters.OfType<HttpNoContentOutputFormatter>().FirstOrDefault();
    if (noContentFormatter != null)
    {
        noContentFormatter.TreatNullValueAsNoContent = false;
    }
}).ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errorMessages = actionContext.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
        var error = new ApiError(string.Concat(errorMessages));
        return new BadRequestObjectResult(error);
    };
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

    options.AddPolicy(PolicyConstants.Admin, policyBuilder =>
    {
        policyBuilder.RequireClaim(ClaimTypeConstants.UserType, ClaimConstants.Admin);
        policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    });

    options.AddPolicy(PolicyConstants.AdminOrProvider, policyBuilder =>
    {
        policyBuilder.RequireClaim(ClaimTypeConstants.UserType, ClaimConstants.Admin, ClaimConstants.Provider);
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
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "Files")),
//    RequestPath = new PathString("/Files"),
//});


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
