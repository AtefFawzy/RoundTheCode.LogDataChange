using RoundTheCode.LogDataChange.Data.DbContexts;
using RoundTheCode.LogDataChange.Data.Entities.Change;
using RoundTheCode.LogDataChange.Data.Entities.Data.BaseObjects;
using RoundTheCode.LogDataChange.Services.Data.BaseObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RoundTheCode.LogDataChange.Services.Change.BaseObjects
{
    public partial interface IBaseChangeService<TDataDbContext, TChangeDbContext, TEntity> : IBaseService<TDataDbContext, TEntity>
        where TDataDbContext : DataDbContext
        where TChangeDbContext : ChangeDbContext
        where TEntity : class, IChangeEntity<TEntity>, IBase, new()
    {
        Task<IList<ChangeView<TEntity>>> ReadViewAsync(int referenceId);
    }
}
