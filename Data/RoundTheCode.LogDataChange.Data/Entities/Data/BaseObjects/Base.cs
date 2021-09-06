using Microsoft.EntityFrameworkCore;
using RoundTheCode.LogDataChange.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.LogDataChange.Data.Entities.Data.BaseObjects
{
    public abstract class Base : IBase
    {
        [ChangeIgnore, ChangeReference]
        public virtual int Id { get; set; }

        [ChangeIgnore]
        public virtual DateTimeOffset Created { get; set; }

        [ChangeIgnore]
        public virtual DateTimeOffset? LastUpdated { get; set; }

        [ChangeIgnore]
        public virtual DateTimeOffset? Deleted { get; set; }

        public static void OnModelCreating<TEntity>(ModelBuilder builder)
            where TEntity : class, IBase
        {
            builder.Entity<TEntity>().HasKey(entity => entity.Id);
        }

    }
}
