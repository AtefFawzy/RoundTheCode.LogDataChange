using RoundTheCode.LogDataChange.Data.Attributes;
using RoundTheCode.LogDataChange.Data.Entities.Data.BaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RoundTheCode.LogDataChange.Data.Entities.Data
{
    public partial class Engine : Base
    {
        [DisplayName("Engine Name"), ChangeReference]
        public virtual string Name { get; set; }
    }
}
