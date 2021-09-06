using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoundTheCode.LogDataChange.Data.DbContexts;
using RoundTheCode.LogDataChange.Data.Entities.Change;
using RoundTheCode.LogDataChange.Data.Entities.Data.BaseObjects;
using RoundTheCode.LogDataChange.Services.Change.BaseObjects;

namespace RoundTheCode.LogDataChange.Web.Api.Controllers
{
    public abstract class BaseChangeController<TDataDbContext, TChangeDbContext, TEntity, TService> : BaseController<TDataDbContext, TEntity, TService>
            where TDataDbContext : DataDbContext
            where TChangeDbContext : ChangeDbContext
            where TEntity : class, IChangeEntity<TEntity>, IBase, new()
            where TService : class, IBaseChangeService<TDataDbContext, TChangeDbContext, TEntity>
    {
        public BaseChangeController([NotNull] TService service) : base(service) { }

        [HttpGet("change/{referenceId:int}")]
        public async Task<IActionResult> ReadViewAsync(int referenceId)
        {
            var entities = await _service.ReadViewAsync(referenceId);

            return Ok(entities);
        }
    }
}
