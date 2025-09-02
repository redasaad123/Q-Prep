
using Core.Interfaces;
using Core.Model;
using Core.Services;
using Core.Servises;
using Core.Settings;
using Infrastructure;
using Infrastructure.Authentication;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectAPI.Hubs;
using System;
using System.Text;

namespace ProjectAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var conn = builder.Configuration.GetConnectionString("ProdcutionConnection");
                if (string.IsNullOrEmpty(conn))
                    throw new InvalidOperationException("Connection string 'ProdcutionConnection' is missing!");

                options.UseSqlServer(conn,
                    options => options.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            });

            // Fix for ASP0025: Use AddAuthorizationBuilder to register authorization services and construct policies
            var authorizationBuilder = builder.Services.AddAuthorizationBuilder();
            authorizationBuilder.AddPolicy("AdminRole", p => p.RequireRole("Admin"));
            authorizationBuilder.AddPolicy("UserRole", p => p.RequireRole("User"));

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("jwt"));
            builder.Services.Configure<EmailConfgSettings>(builder.Configuration.GetSection("email-confg"));
            builder.Services.AddScoped<CodeDatabaseServices>();
            builder.Services.AddScoped<SendEmailServices>();
            builder.Services.AddScoped<GenerateCodeVerify>();

            builder.Services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            builder.Services.AddScoped<PasswordHasher<AppUser>>();

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            builder.Configuration.AddUserSecrets<Program>(optional: true);
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddTransient<IAuthentication, Authentication>();
            builder.Services.AddTransient<Service>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            builder.Services.AddSignalR().AddHubOptions<CommunityHub>(options =>
            {
                options.EnableDetailedErrors = true; // عرض أخطاء مفصلة
            });
            builder.Services.AddSingleton<IUserIdProvider, customId>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;

                // Fix for CS8604: Ensure the configuration value is not null
                var jwtKey = builder.Configuration["jwt:Key"];
                if (string.IsNullOrEmpty(jwtKey))
                {
                    throw new InvalidOperationException("JWT Key is not configured.");
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/communityHub"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                });

                options.AddSecurityRequirement(securityRequirement: new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Name = "Bearer",
                                In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                });
            });

            var app = builder.Build();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<CommunityHub>("/communityHub");
            app.MapControllers();
            app.Run();
        }
    }
}
