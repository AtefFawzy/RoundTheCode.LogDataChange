using RoundTheCode.LogDataChange.Data.DbContexts;
using RoundTheCode.LogDataChange.Data.Entities.Data.BaseObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RoundTheCode.LogDataChange.Services.Data.BaseObjects
{
    public partial interface IBaseService<TDataDbContext, TEntity>
        where TEntity : class, IBase
        where TDataDbContext : DataDbContext
    {
        Task<TEntity> ReadAsync(int id, bool track = true);

        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(int id, TEntity updateEntity);

        Task DeleteAsync(int id);
    }
}
