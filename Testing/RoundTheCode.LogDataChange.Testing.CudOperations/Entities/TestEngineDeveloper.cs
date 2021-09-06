using Microsoft.EntityFrameworkCore;
using RoundTheCode.LogDataChange.Data.Attributes;
using RoundTheCode.LogDataChange.Data.Entities.Data.BaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RoundTheCode.LogDataChange.Testing.CudOperations.Entities
{
    public partial class TestEngineDeveloper : Base
    {
        [ChangeReference, DisplayName("The Name")]
        public virtual string Name { get; set; }

        public virtual int? TestEngineDeveloperCompanySizeId { get; set; }

        public virtual int? TestSecondaryEngineDeveloperCompanySizeId { get; set; }

        public virtual TestEngineDeveloperCompanySize TestEngineDeveloperCompanySize { get; set; }

        [ChangeReference]
        public virtual TestEngineDeveloperCompanySize TestSecondaryEngineDeveloperCompanySize { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TestEngineDeveloper>().HasOne(testEngineDeveloper => testEngineDeveloper.TestEngineDeveloperCompanySize)
                .WithMany()
                .HasPrincipalKey(testEngineCompanySize => testEngineCompanySize.Id)
                .HasForeignKey(testEngineDeveloper => testEngineDeveloper.TestEngineDeveloperCompanySizeId);

            builder.Entity<TestEngineDeveloper>().HasOne(testEngineDeveloper => testEngineDeveloper.TestSecondaryEngineDeveloperCompanySize)
                .WithMany()
                .HasPrincipalKey(testEngineCompanySize => testEngineCompanySize.Id)
                .HasForeignKey(testEngineDeveloper => testEngineDeveloper.TestSecondaryEngineDeveloperCompanySizeId);

        }

    }
}
