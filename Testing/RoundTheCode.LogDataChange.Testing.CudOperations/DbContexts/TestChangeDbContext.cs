using Microsoft.EntityFrameworkCore;
using RoundTheCode.LogDataChange.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RoundTheCode.LogDataChange.Testing.CudOperations.DbContexts
{
    public partial class TestChangeDbContext : ChangeDbContext
    {
        protected override IList<Assembly> Assemblies
        {
            get
            {
                return new List<Assembly>()
                {
                    {
                        Assembly.Load("RoundTheCode.LogDataChange.Testing.CudOperations")
                    }
                };
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseInMemoryDatabase("TestChangeDbContext-" + Guid.NewGuid().ToString());
        }
    }
}
