using System;
using System.Collections.Generic;
using System.Text;
using RoundTheCode.LogDataChange.Services.Change.BaseObjects;
using RoundTheCode.LogDataChange.Testing.CudOperations.DbContexts;
using RoundTheCode.LogDataChange.Testing.CudOperations.Entities;

namespace RoundTheCode.LogDataChange.Testing.CudOperations.Services
{
    public partial interface ITestVideoGameService : IBaseChangeService<TestDataDbContext, TestChangeDbContext, TestVideoGame>
    {

    }
}
