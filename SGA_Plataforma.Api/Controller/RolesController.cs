using Microsoft.AspNetCore.Mvc;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Api.Controller;

[Route("api/[controller]")]
public sealed class RolesController : CrudControllerBase<Role>
{
    public RolesController(ICrudService<Role> service) : base(service) { }
}
