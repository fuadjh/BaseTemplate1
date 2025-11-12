

using Application;
using Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


           


            builder.Services.AddControllers()
               .AddJsonOptions(option => option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // فقط با یک خط تمام تنظیمات اینفراستراکچر را اضافه می‌کنیم
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructure(builder.Configuration);
           
            // Add services to the container.
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
