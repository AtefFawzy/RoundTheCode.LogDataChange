using Microsoft.EntityFrameworkCore;
using RoundTheCode.LogDataChange.Data.Attributes;
using RoundTheCode.LogDataChange.Data.Entities.Data.BaseObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.LogDataChange.Testing.CudOperations.Entities
{
    public partial class TestEngineDeveloperCompanySize : Base
    {
        [ChangeReference]
        public virtual string Size { get; set; }
    }
}
