using System.Text;
using Galini.Models.Entity;
using Galini.Repository.Implement;
using Galini.Repository.Interface;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace Galini.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork<HarmonContext>, UnitOfWork<HarmonContext>>();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<HarmonContext>(options => options.UseSqlServer(GetConnectionString()));
            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IPremiumService, PremiumService>();
            services.AddScoped<IUserCallService, UserCallService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IListenerInfoService, ListenerInfoService>();
            services.AddScoped<ITopicService, TopicService>();
            return services;
        }
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
        {
            services.AddHttpClient(); // Registers HttpClient
            return services;
        }

        public static IServiceCollection AddLazyResolution(this IServiceCollection services)
        {
            services.AddTransient(typeof(Lazy<>), typeof(LazyResolver<>));
            return services;
        }

        private class LazyResolver<T> : Lazy<T> where T : class
        {
            public LazyResolver(IServiceProvider serviceProvider)
                : base(() => serviceProvider.GetRequiredService<T>())
            {
            }
        }


        public static IServiceCollection AddJwtValidation(this IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = "HarmonSystem",
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Convert.FromHexString("0102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F00"))
                };
            });

            return services;
        }


        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            var redisConnectionString = configuration.GetConnectionString("Redis");
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
            return services;
        }

        private static string GetConnectionString()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();
            var strConn = config["ConnectionStrings:DefautDB"];

            return strConn;
        }
    }
}
