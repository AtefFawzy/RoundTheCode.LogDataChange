using RoundTheCode.LogDataChange.Data.DbContexts;
using RoundTheCode.LogDataChange.Data.Entities.Data;
using RoundTheCode.LogDataChange.Services.Change.BaseObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.LogDataChange.Services.Data
{
    public partial interface IVideoGameService : IBaseChangeService<DataDbContext, ChangeDbContext, VideoGame>
    {

    }
}
