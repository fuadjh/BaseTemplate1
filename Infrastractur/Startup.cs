using Application.Interfaces;
using Application.Interfaces.Repositories;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class Startup
    {

        public static IServiceCollection AddDataBase(this IServiceCollection services,IConfiguration configuration) 
        {
            return services
                .AddDbContext<ApplicationDbContext>(option =>
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
        public static IServiceCollection AddRepositoris(this IServiceCollection services)
        {
            return services
                
                .AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
        }
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // سایر تنظیمات EF Core و Identity ...

            services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }


    }
}
