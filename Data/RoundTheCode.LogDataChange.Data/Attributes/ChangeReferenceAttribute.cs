using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.LogDataChange.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ChangeReferenceAttribute : Attribute
    {
        public ChangeReferenceAttribute()
        {
        }

    }
}
