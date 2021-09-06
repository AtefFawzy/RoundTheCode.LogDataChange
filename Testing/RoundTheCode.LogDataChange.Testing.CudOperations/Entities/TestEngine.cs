using Microsoft.EntityFrameworkCore;
using RoundTheCode.LogDataChange.Data.Attributes;
using RoundTheCode.LogDataChange.Data.Entities.Data.BaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RoundTheCode.LogDataChange.Testing.CudOperations.Entities
{
    public partial class TestEngine : Base
    {
        [ChangeReference]
        public virtual string Name { get; set; }

        public virtual int? TestEngineDeveloperId { get; set; }

        public virtual int? TestSecondaryEngineDeveloperId { get; set; }

        public virtual TestEngineDeveloper TestEngineDeveloper { get; set; }

        [ChangeReference, DisplayName("Test Secondary Engine Developer")]
        public virtual TestEngineDeveloper TestSecondaryEngineDeveloper { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TestEngine>().HasOne(testEngine => testEngine.TestEngineDeveloper)
                .WithMany()
                .HasPrincipalKey(testEngineDeveloper => testEngineDeveloper.Id)
                .HasForeignKey(testEngine => testEngine.TestEngineDeveloperId);

            builder.Entity<TestEngine>().HasOne(testEngine => testEngine.TestSecondaryEngineDeveloper)
                .WithMany()
                .HasPrincipalKey(testEngineDeveloper => testEngineDeveloper.Id)
                .HasForeignKey(testEngine => testEngine.TestSecondaryEngineDeveloperId);

        }
    }
}
