using AutoMapper;
using FluentValidation.AspNetCore;
using HogwartsAPI.Entities;
using HogwartsAPI.Middlewares;
using HogwartsAPI.Services;
using HogwartsAPI.Tools;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace HogwartsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddFluentValidation(o =>
            {
                o.RegisterValidatorsFromAssemblyContaining<Program>();
            })
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                o.JsonSerializerOptions.WriteIndented = true;
            });
            builder.Services.AddDbContext<HogwartDbContext>(o =>
                o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<IStudentService, StudentService>();

            builder.Services.AddScoped<ErrorHandlingMiddleware>();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hogwarts API"));

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
