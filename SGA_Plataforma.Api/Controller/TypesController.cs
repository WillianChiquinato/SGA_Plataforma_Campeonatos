using Microsoft.AspNetCore.Mvc;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Infrastructure.Models;
using TypeEntity = SGA_Plataforma.Infrastructure.Models.Type;

namespace SGA_Plataforma.Api.Controller;

[Route("api/[controller]")]
public sealed class TypesController : CrudControllerBase<TypeEntity>
{
    public TypesController(ICrudService<TypeEntity> service) : base(service) { }
}
