using Microsoft.AspNetCore.Mvc;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Utils;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;

namespace SGA_Plataforma.Api.Controller;

[ApiController]
public abstract class CrudControllerBase<TEntity> : ControllerBase where TEntity : BaseEntity
{
    private readonly ICrudService<TEntity> _service;

    protected CrudControllerBase(ICrudService<TEntity> service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual async Task<ActionResult<CustomResponse<IReadOnlyList<TEntity>>>> GetAll(CancellationToken cancellationToken)
    {
        var response = await _service.GetAllAsync(cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public virtual async Task<ActionResult<CustomResponse<TEntity>>> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _service.GetByIdAsync(id, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public virtual async Task<ActionResult<CustomResponse<TEntity>>> Create([FromBody] TEntity entity, CancellationToken cancellationToken)
    {
        var response = await _service.CreateAsync(EntityRequestSanitizer.StripNavigations(entity), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Result.Id }, response);
    }

    [HttpPut("{id:int}")]
    public virtual async Task<ActionResult<CustomResponse<TEntity>>> Update(int id, [FromBody] TEntity entity, CancellationToken cancellationToken)
    {
        var response = await _service.UpdateAsync(id, EntityRequestSanitizer.StripNavigations(entity), cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpDelete("{id:int}")]
    public virtual async Task<ActionResult<CustomResponse<bool>>> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _service.DeleteAsync(id, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }
}