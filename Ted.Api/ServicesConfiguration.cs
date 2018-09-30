using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ted.Bll.Interfaces;
using Ted.Bll.Services;
using Ted.Dal;
using Ted.Knn;
using Ted.Model.Auth;

namespace Ted.Api
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<Context>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //DI
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<IAdService, AdService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IConversationService, ConversationService>();
            services.AddTransient<INetworkService, NetworkService>();
            services.AddTransient<IViewService, ViewService>();
            services.AddTransient<IKnnService, KnnService>();
            
            //Authentication
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password = GetPasswordOptions();

                // Lockout settings
                options.Lockout = GetLockoutOptions();

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            //Authentication

            services.AddIdentity<User, Role>().AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();

            var jwtSettings = Configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSettings);
            
            var key = Encoding.ASCII.GetBytes(jwtSettings.Get<JwtSettings>().SecretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddCors();
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
        }

        private static PasswordOptions GetPasswordOptions()
        {
            PasswordOptions passwordOptions = new PasswordOptions();
            passwordOptions.RequireDigit = false;
            passwordOptions.RequiredLength = 1;
            passwordOptions.RequireNonAlphanumeric = false;
            passwordOptions.RequireUppercase = false;
            passwordOptions.RequireLowercase = false;
            passwordOptions.RequiredUniqueChars = 1;
            return passwordOptions;
        }

        private static LockoutOptions GetLockoutOptions()
        {
            LockoutOptions lockoutOptions = new LockoutOptions();
            lockoutOptions.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            lockoutOptions.MaxFailedAccessAttempts = 10;
            lockoutOptions.AllowedForNewUsers = true;
            return lockoutOptions;
        }
    }
}
