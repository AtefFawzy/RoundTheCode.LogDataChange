using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoundTheCode.LogDataChange.Testing.CudOperations.DbContexts;
using RoundTheCode.LogDataChange.Testing.CudOperations.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.LogDataChange.Testing.CudOperations
{
    public partial class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TestDataDbContext>();
            services.AddDbContext<TestChangeDbContext>();

            services.AddScoped<ITestVideoGameService, TestVideoGameService>();
        }

        public void Configure(IApplicationBuilder app)
        {
        }
    }
}
