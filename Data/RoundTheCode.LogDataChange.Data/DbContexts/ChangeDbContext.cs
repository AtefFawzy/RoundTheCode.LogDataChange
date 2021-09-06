using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RoundTheCode.LogDataChange.Data.Attributes;
using RoundTheCode.LogDataChange.Data.Entities.Change;
using RoundTheCode.LogDataChange.Data.Entities.Change.BaseObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoundTheCode.LogDataChange.Data.DbContexts
{
    public class ChangeDbContext : BaseDbContext
    {
        // Constructers inherited from BaseDbContext
        public ChangeDbContext() : base() { }
        public ChangeDbContext([NotNull] IConfiguration configuration) : base(configuration) { }

        protected virtual IList<Assembly> Assemblies
        {
            get
            {
                return new List<Assembly>()
                {
                    {
                        Assembly.Load("RoundTheCode.LogDataChange.Data")
                    }
                };
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var assembly in Assemblies)
            {
                // Gets all the classes that inherit IChangeEntity<>
                var changeEntityClasses = assembly.GetTypes().Where(s => s.GetInterfaces().Any(_interface => _interface.IsGenericType && _interface.GetGenericTypeDefinition().Equals(typeof(IChangeEntity<>))) && s.IsClass && !s.IsAbstract && s.IsPublic);

                foreach (var changeEntityClass in changeEntityClasses)
                {
                    // Creates a new instance of BaseChange, passing in the change entity class as the generic type
                    var baseChangeGenericClass = typeof(BaseChange<>).MakeGenericType(changeEntityClass);

                    // Uses the [ChangeTable] attribute to bind a table to the change database
                    SetChangeTable(builder, changeEntityClass, baseChangeGenericClass);

                    // Looks for a static 'OnModelCreating' method in the BaseChange class
                    CustomOnModelCreating(builder, baseChangeGenericClass);

                    // Creates a new instance of ChangeView, passing in the change entity class as the generic type
                    var changeViewGenericClass = typeof(ChangeView<>).MakeGenericType(changeEntityClass);

                    // Looks for static 'OnModelCreating' method in the ChangeView class
                    CustomOnModelCreating(builder, changeViewGenericClass);
                }
            }
        }

        protected void SetChangeTable(ModelBuilder builder, Type changeEntityClass, Type baseChangeGenericClass)
        {
            if (Database.IsInMemory())
            {
                return;
            }

            // All change tables MUST have a table name set in the change database
            var attribute = (ChangeTableAttribute)changeEntityClass.GetCustomAttributes(typeof(ChangeTableAttribute)).FirstOrDefault();

            if (attribute == null)
            {
                // Throw an error if no change table is specified
                throw new Exception("As you are including changes in " + changeEntityClass.Name + ", you must include the [ChangeTable] attribute in your class, specifying which table you wish to write your change information to");
            }

            // Set the table and schema name to the change database.
            builder.Entity(baseChangeGenericClass).ToTable(attribute.Name, attribute.Schema);
        }

        protected void CustomOnModelCreating(ModelBuilder builder, Type baseChangeGenericClass)
        {
            // Get the static OnModelCreating from BaseChange
            var onModelCreatingMethod = baseChangeGenericClass.GetMethods().FirstOrDefault(x => x.Name == "OnModelCreating" && x.IsStatic);

            if (onModelCreatingMethod != null)
            {
                // Runs BaseChange.OnModelCreating static method
                onModelCreatingMethod.Invoke(baseChangeGenericClass, new object[] { builder });
            }

        }

        protected override string ConnectionStringLocation { get => "ConnectionStrings:ChangeDbContext"; }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("Created").CurrentValue = DateTimeOffset.Now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
