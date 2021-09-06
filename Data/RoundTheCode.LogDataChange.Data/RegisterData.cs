using Microsoft.Extensions.DependencyInjection;
using RoundTheCode.LogDataChange.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.LogDataChange.Data
{
    public static class RegisterData
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            services.AddDbContext<DataDbContext>();
            services.AddDbContext<ChangeDbContext>();

            return services;
        }
    }
}
