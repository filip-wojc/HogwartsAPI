using HogwartsAPI.Entities;
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

            builder.Services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                o.JsonSerializerOptions.WriteIndented = true;
            });
            builder.Services.AddDbContext<HogwartDbContext>(o =>
                o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
