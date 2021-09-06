using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoundTheCode.LogDataChange.Data.Entities.Change;
using RoundTheCode.LogDataChange.Data.Entities.Change.BaseObjects;
using RoundTheCode.LogDataChange.Testing.CudOperations.DbContexts;
using RoundTheCode.LogDataChange.Testing.CudOperations.Entities;
using RoundTheCode.LogDataChange.Testing.CudOperations.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoundTheCode.LogDataChange.Testing.CudOperations
{
    public abstract partial class BaseTest
    {
        protected IHost _builder;
        protected IServiceScope _scope;
        protected ITestVideoGameService _testVideoGameService;
        protected TestDataDbContext _testDataDbContext;
        protected TestChangeDbContext _testChangeDbContext;

        protected BaseTest()
        {
            string[] args = null;

            // Creates the application
            _builder = Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TestStartup>();
                }).Build();

            _scope = _builder.Services.CreateScope();

            _testVideoGameService = (ITestVideoGameService)_scope.ServiceProvider.GetRequiredService(typeof(ITestVideoGameService));
            _testDataDbContext = (TestDataDbContext)_scope.ServiceProvider.GetRequiredService(typeof(TestDataDbContext));
            _testChangeDbContext = (TestChangeDbContext)_scope.ServiceProvider.GetRequiredService(typeof(TestChangeDbContext));

            // Engine Developer Company Size
            _testDataDbContext.Set<TestEngineDeveloperCompanySize>().Add(new TestEngineDeveloperCompanySize
            {
                Id = 1,
                Size = "Under 200 employees"
            });
            _testDataDbContext.Set<TestEngineDeveloperCompanySize>().Add(new TestEngineDeveloperCompanySize
            {
                Id = 2,
                Size = "200-300 employees"
            });
            _testDataDbContext.Set<TestEngineDeveloperCompanySize>().Add(new TestEngineDeveloperCompanySize
            {
                Id = 3,
                Size = "300+ employees"
            });

            // Engine Developer
            _testDataDbContext.Set<TestEngineDeveloper>().Add(new TestEngineDeveloper
            {
                Id = 1,
                Name = "Epic Games",
                TestEngineDeveloperCompanySizeId = 3,
                TestSecondaryEngineDeveloperCompanySizeId = 2
            });
            _testDataDbContext.Set<TestEngineDeveloper>().Add(new TestEngineDeveloper
            {
                Id = 2,
                Name = "Unity Technologies",
                TestEngineDeveloperCompanySizeId = 2,
                TestSecondaryEngineDeveloperCompanySizeId = 3
            });

            // Engine
            _testDataDbContext.Set<TestEngine>().Add(new TestEngine
            {
                Id = 1,
                Name = "Unreal Engine 4",
                TestEngineDeveloperId = 1,
                TestSecondaryEngineDeveloperId = 2
            });
            _testDataDbContext.Set<TestEngine>().Add(new TestEngine
            {
                Id = 2,
                Name = "Unity",
                TestEngineDeveloperId = 2,
                TestSecondaryEngineDeveloperId = 1
            });

            // Games
            _testDataDbContext.Set<TestVideoGame>().Add(new TestVideoGame
            {
                Id = 1,
                Title = "Friday The 13th: The Game",
                TestEngineId = 1
            });
            _testDataDbContext.Set<TestVideoGame>().Add(new TestVideoGame
            {
                Id = 2,
                Title = "Ace Combat 7: Skies Unknown",
                TestEngineId = 1
            });
            _testDataDbContext.Set<TestVideoGame>().Add(new TestVideoGame
            {
                Id = 3,
                Title = "Wasteland 2",
                TestEngineId = 2
            });
            _testDataDbContext.SaveChanges();
        }

        protected async Task<BaseChange<TEntity>> RunChangeTestsAsync<TEntity>(int expectedChangeRecords)
            where TEntity : class, IChangeEntity<TEntity>, new()
        {
            Assert.Equal(expectedChangeRecords, await _testChangeDbContext.Set<BaseChange<TEntity>>().CountAsync());

            // Get latest change record
            return await _testChangeDbContext.Set<BaseChange<TEntity>>().OrderByDescending(baseChange => baseChange.Id).FirstOrDefaultAsync();
        }

        protected ChangeProperty GetChangeProperty(ChangeData changeData, string propertyName, string reference = null)
        {
            return reference != null ? changeData[reference, propertyName] : changeData[propertyName];
        }

        protected void RunChangePropertyTests(ChangeData changeData, string propertyName, string reference, bool expectedNull, string expectedDisplayName = null, object expectedCurrent = null, object expectedOriginal = null, string expectedReferenceDisplayName = null)
        {
            var changeProperty = (reference != null ? changeData[reference, propertyName] : changeData[propertyName]);

            if (expectedNull)
            {
                Assert.Null(changeProperty);
                return;
            }
            else
            {
                Assert.NotNull(changeProperty);
            }

            Assert.Equal(propertyName, changeProperty.PropertyName);

            RunChangePropertyTest(reference, changeProperty.Reference);
            RunChangePropertyTest(expectedDisplayName, changeProperty.DisplayName);
            RunChangePropertyTest(expectedCurrent, changeProperty.Current);
            RunChangePropertyTest(expectedOriginal, changeProperty.Original);
            RunChangePropertyTest(expectedReferenceDisplayName, changeProperty.ReferenceDisplayName);
        }

        protected void RunChangePropertyTest(object expected, object actual)
        {
            if (expected == null)
            {
                Assert.Null(actual);
            }
            else
            {
                Assert.Equal(expected, actual);
            }
        }
    }
}
