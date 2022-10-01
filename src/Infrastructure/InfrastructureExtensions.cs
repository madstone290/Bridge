using Bridge.Application.Common;
using Bridge.Application.Common.Services;
using Bridge.Application.Places.Repos;
using Bridge.Application.Products.Repos;
using Bridge.Domain.Places.Repos;
using Bridge.Domain.Products.Repos;
using Bridge.Infrastructure.Data;
using Bridge.Infrastructure.Data.ReadRepos;
using Bridge.Infrastructure.Data.Repos;
using Bridge.Infrastructure.Extensions;
using Bridge.Infrastructure.Identity;
using Bridge.Infrastructure.Identity.Entities;
using Bridge.Infrastructure.Identity.Services;
using Bridge.Infrastructure.NaverMaps;
using Bridge.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bridge.Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static void AddInfrastructureDependency(this IServiceCollection services, IConfiguration configuration)
        {
            var bridgeConnString = configuration["DbContext:BridgeContext:ConnectionString"];
            services.AddDbContext<BridgeContext>(options =>
            {
                options.UseNpgsql(bridgeConnString);
            });

            var identityConnString = configuration["DbContext:IdentityContext:ConnectionString"];
            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseNpgsql(identityConnString);
            });

            services.AddIdentity<BridgeUser, BridgeRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            // Options 추가
            services.AddOptionsEx<MailService.Config>(configuration.GetSection("MailService"));
            services.AddOptionsEx<TokenService.Config>(configuration.GetSection("TokenService"));
            services.AddOptionsEx<GeoCodeApi.Config>(configuration.GetSection("GeoCodeApi"));

            services.AddHttpClient<GeoCodeApi>();
            services.AddScoped<GeoCodeApi, GeoCodeApi>();

            // identity services
            services.AddScoped<UserService, UserService>();
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailVerificationService, EmailVerificationService>();
            services.AddScoped<IAdminUserService, AdminUserService>();

            // infra services
            services.AddScoped<IAddressLocationService, AddressLocationService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<ICoordinateService, CoordinateService>();

            // repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPlaceRepository, PlaceRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            
            services.AddScoped<IPlaceReadRepository, PlaceReadRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
        }

    }
}
