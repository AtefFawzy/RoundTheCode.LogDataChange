using System;
using System.Collections.Generic;
using System.Text;
using RoundTheCode.LogDataChange.Services.Change.BaseObjects;
using RoundTheCode.LogDataChange.Testing.CudOperations.DbContexts;
using RoundTheCode.LogDataChange.Testing.CudOperations.Entities;

namespace RoundTheCode.LogDataChange.Testing.CudOperations.Services
{
    public partial class TestVideoGameService : BaseChangeService<TestDataDbContext, TestChangeDbContext, TestVideoGame>, ITestVideoGameService
    {
        public TestVideoGameService(TestDataDbContext mainDbContext, TestChangeDbContext changeDbContext) : base(mainDbContext, changeDbContext) { }
    }
}
