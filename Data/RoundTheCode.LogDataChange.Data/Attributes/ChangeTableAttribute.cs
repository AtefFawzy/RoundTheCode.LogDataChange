using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.LogDataChange.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public partial class ChangeTableAttribute : Attribute
    {
        protected string _schema;

        public ChangeTableAttribute(string name)
        {
            Name = name;
        }

        public virtual string Name { get; }

        public virtual string Schema
        {
            get => _schema;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    // Schema cannot be null
                    throw new Exception("Schema is not set to a referenced object");
                }

                _schema = value;
            }
        }

    }
}
