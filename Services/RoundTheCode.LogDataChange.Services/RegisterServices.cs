using Microsoft.Extensions.DependencyInjection;
using RoundTheCode.LogDataChange.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.LogDataChange.Services
{
    public static class RegisterServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IVideoGameService, VideoGameService>();

            return services;
        }
    }
}
