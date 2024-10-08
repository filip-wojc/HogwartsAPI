using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using HogwartsAPI.Authorization;
using HogwartsAPI.Entities;
using HogwartsAPI.Exceptions;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Tools;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Text;
using System.Text.Json.Serialization;

namespace HogwartsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var modules = new List<IModule>
            {
                new StudentModule(),
                new WandModule(),
                new CourseModule(),
                new UserModule(),
                new CourseStudentModule(),
                new HouseModule(),
                new PetModule(),
                new TeacherModule(),
                new HomeworkModule(),
                new HomeworkSubmissionModule(),
                new EventsModule()
            };

            foreach (var module in modules)
            {
                module.RegisterServices(builder.Services);
            }

            builder.Services.AddControllers()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                o.JsonSerializerOptions.WriteIndented = true;
            });

            builder.Host.UseNLog();

            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();


            var authParameters = new JwtParameters();
            builder.Configuration.GetSection("Authentication").Bind(authParameters);
            builder.Services.AddSingleton(authParameters);
            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "Bearer";
                o.DefaultScheme = "Bearer";
                o.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authParameters.JwtIssuer,
                    ValidAudience = authParameters.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authParameters.JwtKey)),
                };
            });

            var authPolicies = new AuthorizationPolicyHandler();
            authPolicies.AddAuthorizationPolicy(builder.Services);

            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Services.AddDbContext<HogwartDbContext>(o =>
                o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddExceptionHandler<AppExceptionHandler>();
            builder.Services.AddExceptionHandler<GeneralExceptionHandler>();
            builder.Services.AddHttpContextAccessor();
            var swaggerConfig = new SwaggerConfigTool();
            swaggerConfig.RegisterServices(builder.Services);
            builder.Services.AddCors(o =>
            {
                o.AddPolicy("FrontEndClient", b =>
                {
                    b.AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://127.0.0.1:5500");
                });
            });

            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseResponseCaching();

            app.UseCors("FrontEndClient");
            app.UseExceptionHandler(_ => { });
            // Configure the HTTP request pipeline.

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hogwarts API"));


            app.MapControllers();

            app.Run();
        }
    }
}
