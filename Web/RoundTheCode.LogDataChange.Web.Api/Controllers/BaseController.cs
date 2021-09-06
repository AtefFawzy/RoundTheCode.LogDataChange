using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RoundTheCode.LogDataChange.Data.DbContexts;
using RoundTheCode.LogDataChange.Data.Entities.Data.BaseObjects;
using RoundTheCode.LogDataChange.Services.Data.BaseObjects;

namespace RoundTheCode.LogDataChange.Web.Api.Controllers
{
    [ApiController]
    public abstract partial class BaseController<TDataDbContext, TEntity, TService> : ControllerBase
        where TDataDbContext : DataDbContext
        where TEntity : class, IBase
        where TService : class, IBaseService<TDataDbContext, TEntity>
    {
        protected readonly TService _service;

        public BaseController([NotNull] TService service)
        {
            _service = service;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ReadAsync(int id)
        {
            var entity = await _service.ReadAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(TEntity entity)
        {
            entity = await _service.CreateAsync(entity);

            return Ok(entity);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdatePartialAsync(int id, [FromBody] JsonPatchDocument<TEntity> patchEntity)
        {
            var entity = await _service.ReadAsync(id, false);

            if (entity == null)
            {
                return NotFound();
            }

            patchEntity.ApplyTo(entity, ModelState);
            entity = await _service.UpdateAsync(id, entity);

            return Ok(entity);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var entity = await _service.ReadAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(id);

            return Ok();
        }
    }
}
