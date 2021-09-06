using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RoundTheCode.LogDataChange.Data.Attributes;
using RoundTheCode.LogDataChange.Data.DbContexts;
using RoundTheCode.LogDataChange.Data.Entities.Change;
using RoundTheCode.LogDataChange.Data.Entities.Change.BaseObjects;
using RoundTheCode.LogDataChange.Data.Entities.Data.BaseObjects;
using RoundTheCode.LogDataChange.Services.Data.BaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoundTheCode.LogDataChange.Services.Change.BaseObjects
{
    public abstract partial class BaseChangeService<TDataDbContext, TChangeDbContext, TEntity> : BaseService<TDataDbContext, TEntity>, IBaseChangeService<TDataDbContext, TChangeDbContext, TEntity>
            where TDataDbContext : DataDbContext
            where TChangeDbContext : ChangeDbContext
            where TEntity : class, IChangeEntity<TEntity>, IBase, new()
    {
        protected readonly TChangeDbContext _changeDbContext;

        protected BaseChangeService([NotNull] TDataDbContext dataDbContext, [NotNull] TChangeDbContext changeDbContext) : base(dataDbContext)
        {
            _changeDbContext = changeDbContext;
        }

        public override async Task<TEntity> CreateAsync(TEntity entity)
        {
            var changeData = new ChangeData(EntityState.Added);
            await AddEntityToContextAsync(entity); // Add entity to DbContext

            var depth = 0;
            var count = 0;
            GetChangeData(changeData, _dataDbContext.Entry(entity), EntityState.Added);
            GetChangeDataReference(changeData, _dataDbContext.Entry(entity), EntityState.Added, ChangePropertyTypeEnum.Current, ref depth, ref count);

            await SaveChangesAsync(); // Save changes to DbContext
            await CreateChangeAsync(entity.Id, changeData);

            return entity;
        }

        public override async Task<TEntity> UpdateAsync(int id, TEntity updateEntity)
        {
            // Check that the record exists.
            var entity = await ReadAsync(id);

            if (entity == null)
            {
                throw new Exception("Unable to find record with id '" + id + "'.");
            }

            var entityEntry = _dataDbContext.Entry(entity);

            // Store references before we update our entity.
            var depth = 0;
            var count = 0;
            var changeData = new ChangeData(EntityState.Modified);
            GetChangeDataReference(changeData, entityEntry, EntityState.Modified, ChangePropertyTypeEnum.Original, ref depth, ref count);

            // Update changes if any of the properties have been modified.
            _dataDbContext.Entry(entity).CurrentValues.SetValues(updateEntity);
            _dataDbContext.Entry(entity).State = EntityState.Modified;

            depth = 0;
            count = 0;
            GetChangeData(changeData, entityEntry, EntityState.Modified);
            GetChangeDataReference(changeData, entityEntry, EntityState.Modified, ChangePropertyTypeEnum.Current, ref depth, ref count);

            if (entityEntry.Properties.Any(property => property.IsModified))
            {
                await SaveChangesAsync();
                await CreateChangeAsync(entity.Id, changeData);
            }
            return entity;
        }

        public override async Task DeleteAsync(int id)
        {
            await base.DeleteAsync(id);

            var changeData = new ChangeData(EntityState.Modified);
            changeData.AddChangeProperty(new ChangeProperty("Deleted", true, false));

            await CreateChangeAsync(id, changeData);
        }
        public virtual async Task<IList<ChangeView<TEntity>>> ReadViewAsync(int referenceId)
        {
            // All change tables MUST have a table name set in the change database
            var attribute = (ChangeTableAttribute)typeof(TEntity).GetCustomAttributes(typeof(ChangeTableAttribute)).FirstOrDefault();

            if (attribute == null)
            {
                // Throw an error if no change table is specified
                throw new Exception("To read change records from " + typeof(TEntity).Name + ", you must include the [ChangeTable] attribute in your class, specifying which table you wish to write your change information to");
            }

            return await _changeDbContext.Set<ChangeView<TEntity>>().FromSqlRaw(
                $"SELECT Id AS ChangeId, ReferenceId, JSON_VALUE(change.ChangeData, '$.CUD') AS CUD, ChangeData.PropertyName, ChangeData.DisplayName, ChangeData.[Current], ChangeData.Original, ChangeData.Reference, ChangeData.ReferenceDisplayName, Created from [" + attribute.Schema + "].[" + attribute.Name + "] [change]" +
                " CROSS APPLY OPENJSON([change].ChangeData, N'$.ChangeProperties')" +
                " WITH(" +
                "PropertyName NVARCHAR(100) '$.PropertyName'," +
                "DisplayName NVARCHAR(100) '$.DisplayName'," +
                "[Current] NVARCHAR(100) '$.Current'," +
                "[Original] NVARCHAR(100) '$.Original'," +
                "[Reference] NVARCHAR(100) '$.Reference'," +
                "[ReferenceDisplayName] NVARCHAR(100) '$.ReferenceDisplayName'" +
                ") AS ChangeData"
                ).Where(changeView => changeView.ReferenceId == referenceId).ToListAsync();
        }


        protected async Task CreateChangeAsync(int id, ChangeData changeData)
        {
            if (changeData.ChangeProperties == null || changeData.ChangeProperties.Count == 0)
            {
                return;
            }
            await _changeDbContext.AddAsync(new BaseChange<TEntity>(id, changeData));
            await _changeDbContext.SaveChangesAsync();
        }

        protected void GetChangeData([NotNull] ChangeData changeData, EntityEntry entry, EntityState entityState)
        {
            PopulateChangeProperties(changeData, entityState, entry.CurrentValues, ChangePropertyTypeEnum.Current);

            if (entityState == EntityState.Modified)
            {
                PopulateChangeProperties(changeData, entityState, entry.OriginalValues, ChangePropertyTypeEnum.Original);
            }
        }

        protected void GetChangeDataReference([NotNull] ChangeData changeData, EntityEntry entry, EntityState entityState, ChangePropertyTypeEnum changePropertyType, ref int depth, ref int count)
        {
            var references = entry.References;
            depth += 1;
            count += 1;

            if (count > 100)
            {
                throw new Exception("GetChangeDataReference has been called more than 100 times, so there may be an issue with logging references");
            }

            if (references == null || depth > 2)
            {
                if (depth > 0)
                {
                    depth -= 1;
                }
                return;
            }

            foreach (var entryReference in references)
            {
                if (!entryReference.Metadata.PropertyInfo.GetCustomAttributes(typeof(ChangeReferenceAttribute), true).Any())
                {
                    continue;
                }

                entryReference.Load();

                if (entryReference.TargetEntry == null)
                {
                    continue;
                }

                string referenceDisplayName = null;
                if (entryReference.Metadata.PropertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true).Any())
                {
                    referenceDisplayName = ((DisplayNameAttribute)entryReference.Metadata.PropertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault()).DisplayName;
                }

                PopulateChangeProperties(changeData, entityState, changePropertyType == ChangePropertyTypeEnum.Original ? entryReference.TargetEntry.OriginalValues : entryReference.TargetEntry.CurrentValues, changePropertyType, entryReference.Metadata.PropertyInfo.Name, referenceDisplayName);

                GetChangeDataReference(changeData, entryReference.TargetEntry, entityState, changePropertyType, ref depth, ref count);
            }

            if (depth > 0)
            {
                depth -= 1;
            }
        }

        protected void PopulateChangeProperties([NotNull] ChangeData changeData, EntityState entityState, PropertyValues propertyValues, ChangePropertyTypeEnum changePropertyType, string reference = null, string referenceDisplayName = null)
        {
            foreach (var property in propertyValues.Properties)
            {
                if (property.PropertyInfo.GetCustomAttributes(typeof(ChangeIgnoreAttribute), true).Any() && (reference == null || !property.PropertyInfo.GetCustomAttributes(typeof(ChangeReferenceAttribute), true).Any()))
                {
                    // Exclude any properties that contain the [ChangeIgnore] attribute
                    continue;
                }
                if (reference != null && !property.PropertyInfo.GetCustomAttributes(typeof(ChangeReferenceAttribute), true).Any())
                {
                    continue;
                }

                string displayName = null;
                if (property.PropertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true).Any())
                {
                    displayName = ((DisplayNameAttribute)property.PropertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault()).DisplayName;
                }

                var changePropertyName = property.Name;
                var changeProperty = reference != null ? changeData[reference, changePropertyName] : changeData[changePropertyName];
                var changePropertyValue = propertyValues[property];

                if (changeProperty != null)
                {
                    changeProperty.SetValue(changePropertyValue, changePropertyType);

                    // Remove if the values remain the same.
                    if (entityState == EntityState.Modified && Equals(changeProperty.Current, changeProperty.Original))
                    {
                        changeData.ChangeProperties.Remove(changeProperty);
                    }
                }
                else
                {
                    changeData.AddChangeProperty(new ChangeProperty(changePropertyName, changePropertyValue, changePropertyType, displayName, reference, referenceDisplayName));
                }
            }
        }
    }
}
