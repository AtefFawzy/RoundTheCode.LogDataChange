using Microsoft.EntityFrameworkCore;
using RoundTheCode.LogDataChange.Data.Entities.Data.BaseObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.LogDataChange.Data.Entities.Change
{
    public partial class ChangeView<TEntity>
            where TEntity : class, IBase, IChangeEntity<TEntity>, new()
    {
        public virtual int ChangeId { get; set; }

        public virtual int ReferenceId { get; set; }

        public virtual string CUD { get; set; }

        public virtual string PropertyName { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string Current { get; set; }

        public virtual string Original { get; set; }

        public virtual string Reference { get; set; }

        public virtual string ReferenceDisplayName { get; set; }


        public virtual DateTimeOffset Created { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ChangeView<TEntity>>().HasNoKey();
        }
    }
}
