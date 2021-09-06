using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.LogDataChange.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public partial class ChangeIgnoreAttribute : Attribute
    {
        protected string _schema;

        public ChangeIgnoreAttribute()
        {
        }

    }
}
