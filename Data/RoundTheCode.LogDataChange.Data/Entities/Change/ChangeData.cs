using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace RoundTheCode.LogDataChange.Data.Entities.Change
{
    public partial class ChangeData
    {
        protected readonly EntityState EntityState;
        public IList<ChangeProperty> ChangeProperties { get; set; }

        public ChangeData([NotNull] EntityState entityState)
        {
            // Pass in the entity state as a parameter (Added, Updated, Deleted)
            EntityState = entityState;
        }

        public virtual string CUD
        {
            get
            {
                switch (EntityState)
                {
                    case EntityState.Added:
                        return "Create";
                    case EntityState.Deleted:
                        return "Hard Delete";
                    case EntityState.Modified:
                        if (ChangeProperties != null && ChangeProperties.Any(changeData => changeData.PropertyName == "Deleted"))
                        {
                            return "Soft Delete";
                        }
                        else
                        {
                            return "Update";
                        }
                }

                return null;
            }
        }

        public void AddChangeProperty(ChangeProperty changeProperty)
        {
            if (ChangeProperties == null)
            {
                ChangeProperties = new List<ChangeProperty>();
            }
            ChangeProperties.Add(changeProperty);
        }

        public ChangeProperty this[string propertyName]
        {
            get
            {
                return ChangeProperties != null ? ChangeProperties.FirstOrDefault(property => property.PropertyName == propertyName && property.Reference == null) : null;
            }
        }

        public ChangeProperty this[string referenceName, string propertyName]
        {
            get
            {
                return ChangeProperties != null ? ChangeProperties.FirstOrDefault(property => property.PropertyName == propertyName && property.Reference == referenceName) : null;
            }
        }
    }
}
