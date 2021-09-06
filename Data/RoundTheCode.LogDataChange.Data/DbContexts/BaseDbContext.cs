using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;

namespace RoundTheCode.LogDataChange.Data.DbContexts
{
    public abstract partial class BaseDbContext : DbContext
    {
        protected IConfiguration _configuration;

        protected BaseDbContext()
        {
            // Creates a new configuration from the appsettings.json file.
            _configuration = new ConfigurationBuilder().AddJsonFile(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\appsettings.json").Build();
        }

        protected BaseDbContext([NotNull] IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected abstract string ConnectionStringLocation { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            // Sets the database connection from appsettings.json
            if (_configuration[ConnectionStringLocation] != null)
            {
                builder.UseSqlServer(_configuration[ConnectionStringLocation]);
            }
        }
    }
}
