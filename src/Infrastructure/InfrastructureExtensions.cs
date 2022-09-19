using Bridge.Application.Common;
using Bridge.Application.Places.Repos;
using Bridge.Application.Products.Repos;
using Bridge.Application.Users.ReadRepos;
using Bridge.Domain.Places.Repos;
using Bridge.Domain.Products.Repos;
using Bridge.Domain.Users.Repos;
using Bridge.Infrastructure.Data;
using Bridge.Infrastructure.Data.ReadRepos;
using Bridge.Infrastructure.Data.Repos;
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

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPlaceRepository, PlaceRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IPlaceReadRepository, PlaceReadRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();

        }

    }
}
