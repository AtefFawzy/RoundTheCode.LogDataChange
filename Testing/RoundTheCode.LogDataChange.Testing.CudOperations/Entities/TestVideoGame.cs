using Microsoft.EntityFrameworkCore;
using RoundTheCode.LogDataChange.Data.Attributes;
using RoundTheCode.LogDataChange.Data.Entities.Change;
using RoundTheCode.LogDataChange.Data.Entities.Data.BaseObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.LogDataChange.Testing.CudOperations.Entities
{
    public partial class TestVideoGame : Base, IChangeEntity<TestVideoGame>
    {
        public virtual string Title { get; set; }

        public virtual int? TestEngineId { get; set; }

        [ChangeReference]
        public virtual TestEngine TestEngine { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TestVideoGame>().HasOne(testVideoGame => testVideoGame.TestEngine)
                .WithMany()
                .HasPrincipalKey(testEngine => testEngine.Id)
                .HasForeignKey(testVideoGame => testVideoGame.TestEngineId);
        }
    }
}
