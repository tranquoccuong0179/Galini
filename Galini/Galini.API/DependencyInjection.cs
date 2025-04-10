﻿using System.Text;
using Galini.API.ConfigHub;
using Galini.Models.Entity;
using Galini.Models.Utils;
using Galini.Repository.Implement;
using Galini.Repository.Interface;
using Galini.Services.Implement;
using Galini.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
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
            services.AddScoped<ITestHistoryService, TestHistoryService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IPremiumService, PremiumService>();
            services.AddScoped<IUserCallService, UserCallService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICallHistoryService, CallHistoryService>();
            services.AddScoped<IFriendShipService, FriendShipService>();
            services.AddScoped<IUserStatusService, UserStatusService>();
            services.AddScoped<IListenerInfoService, ListenerInfoService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IGoogleAuthenticationService, GoogleAuthenticationService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IWorkShiftService, WorkShiftService>();
            services.AddScoped<IUserPresenceService, UserPresenceService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IDirectChatService, DirectChatService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<HtmlSanitizerUtil>();
            services.AddScoped<CallHub>();
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<CallBookingHub>();
            services.AddScoped<IDashboardService, DashboardService>();
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

        private static string CreateClientId(IConfiguration configuration)
        {
            var clientId = Environment.GetEnvironmentVariable("GOOGLE_OAUTH_CLIENT_ID")
                           ?? configuration.GetValue<string>("Oauth:ClientId");
            return clientId;
        }

        private static string CreateClientSecret(IConfiguration configuration)
        {
            var clientSecret = Environment.GetEnvironmentVariable("GOOGLE_OAUTH_CLIENT_SECRET")
                               ?? configuration.GetValue<string>("Oauth:ClientSecret");
            return clientSecret;
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

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        // Chỉ áp dụng khi request vào SignalR Hub
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/callhub"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            }).AddCookie(
                options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.None;
                })
            .AddGoogle(options =>
            {
                options.ClientId = CreateClientId(configuration);
                options.ClientSecret = CreateClientSecret(configuration);
                options.SaveTokens = true;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.Always;
            }); ;

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

        public static IServiceCollection AddQuartzJobs(this IServiceCollection services)
        {
            var jobKey = new JobKey("PremiumCheckJob");

            services.AddQuartz(config =>
            {
                config.AddJob<PremiumCheckJob>(opts => opts.WithIdentity(jobKey));

                config.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("PremiumCheckTrigger")
                    .WithCronSchedule("0 0 1 * * ?"));
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }
    }
}
