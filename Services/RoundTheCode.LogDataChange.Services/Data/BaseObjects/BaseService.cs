using Microsoft.EntityFrameworkCore;
using RoundTheCode.LogDataChange.Data.DbContexts;
using RoundTheCode.LogDataChange.Data.Entities.Data.BaseObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoundTheCode.LogDataChange.Services.Data.BaseObjects
{
    public abstract partial class BaseService<TDataDbContext, TEntity> : IBaseService<TDataDbContext, TEntity>
            where TDataDbContext : DataDbContext
            where TEntity : class, IBase
    {
        protected readonly TDataDbContext _dataDbContext;

        protected BaseService([NotNull] TDataDbContext dataDbContext)
        {
            _dataDbContext = dataDbContext;
        }

        public virtual async Task<TEntity> ReadAsync(int id, bool track = true)
        {
            var query = _dataDbContext.Set<TEntity>().AsQueryable();

            if (!track)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(entity => entity.Id == id && !entity.Deleted.HasValue);
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            await AddEntityToContextAsync(entity); // Add entity to DbContext
            await SaveChangesAsync(); // Save changes to DbContext

            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(int id, TEntity updateEntity)
        {
            // Check that the record exists.
            var entity = await ReadAsync(id);

            if (entity == null)
            {
                throw new Exception("Unable to find record with id '" + id + "'.");
            }

            // Update changes if any of the properties have been modified.
            _dataDbContext.Entry(entity).CurrentValues.SetValues(updateEntity);
            _dataDbContext.Entry(entity).State = EntityState.Modified;

            if (_dataDbContext.Entry(entity).Properties.Any(property => property.IsModified))
            {
                await SaveChangesAsync();
            }
            return entity;
        }

        public virtual async Task DeleteAsync(int id)
        {
            // Check that the record exists.
            var entity = await ReadAsync(id);

            if (entity == null)
            {
                throw new Exception("Unable to find record with id '" + id + "'.");
            }

            // Set the deleted flag.
            entity.Deleted = DateTimeOffset.Now;
            _dataDbContext.Entry(entity).State = EntityState.Modified;

            // Save changes to the Db Context.
            await SaveChangesAsync();
        }

        protected async Task AddEntityToContextAsync(TEntity entity)
        {
            await _dataDbContext.Set<TEntity>().AddAsync(entity);
        }

        protected async Task SaveChangesAsync()
        {
            await _dataDbContext.SaveChangesAsync();
        }
    }
}
