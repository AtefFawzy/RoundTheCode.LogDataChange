using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.LogDataChange.Data.Entities.Change
{
    public class ChangeProperty
    {
        public virtual string PropertyName { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual object Original { get; set; }
        public virtual object Current { get; set; }
        public virtual string Reference { get; set; }
        public virtual string ReferenceDisplayName { get; set; }

        public ChangeProperty(string propertyName, object value, ChangePropertyTypeEnum changePropertyType, string displayName = null, string reference = null, string referenceDisplayName = null)
        {
            PropertyName = propertyName;

            if (changePropertyType == ChangePropertyTypeEnum.Current)
            {
                Current = value;
            }
            else if (changePropertyType == ChangePropertyTypeEnum.Original)
            {
                Original = value;
            }
            DisplayName = displayName;
            Reference = reference;
            ReferenceDisplayName = referenceDisplayName;
        }

        public ChangeProperty(string propertyName, object current, object original, string displayName = null, string reference = null, string referenceDisplayName = null)
        {
            PropertyName = propertyName;
            Current = current;
            Original = original;
            DisplayName = displayName;
            Reference = reference;
            ReferenceDisplayName = referenceDisplayName;
        }

        public void SetValue(object value, ChangePropertyTypeEnum changePropertyType)
        {
            if (changePropertyType == ChangePropertyTypeEnum.Current)
            {
                Current = value;
            }
            else if (changePropertyType == ChangePropertyTypeEnum.Original)
            {
                Original = value;
            }
        }
    }

    public enum ChangePropertyTypeEnum
    {
        Current = 1,
        Original = 2
    }
}
