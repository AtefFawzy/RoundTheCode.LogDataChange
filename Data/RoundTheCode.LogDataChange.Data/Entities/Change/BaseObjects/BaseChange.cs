using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.LogDataChange.Data.Entities.Change.BaseObjects
{
    public partial class BaseChange<TChangeEntity> : IBaseChange<TChangeEntity>
        where TChangeEntity : class, IChangeEntity<TChangeEntity>, new()
    {
        public virtual int Id { get; set; }

        public virtual int ReferenceId { get; set; }

        public virtual ChangeData ChangeData { get; set; }

        public virtual DateTimeOffset Created { get; set; }

        public BaseChange(int referenceId, ChangeData changeData)
        {
            ReferenceId = referenceId;
            ChangeData = changeData;
            Created = DateTime.Now;
        }

        public static void OnModelCreating(ModelBuilder builder)
        {
            // Makes the ChangeData unicode
            builder.Entity<BaseChange<TChangeEntity>>().Property(m => m.ChangeData).HasConversion(ChangeJsonValueConverter());
        }

        protected static ValueConverter<ChangeData, string> ChangeJsonValueConverter()
        {
            return new ValueConverter<ChangeData, string>(
                changeData => JsonConvert.SerializeObject(changeData, Formatting.None, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                }),
                text => JsonConvert.DeserializeObject<ChangeData>(text)
            );
        }
    }
}
