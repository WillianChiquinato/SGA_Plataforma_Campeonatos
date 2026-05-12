using Microsoft.AspNetCore.Mvc;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Api.Controller;

[Route("api/[controller]")]
public sealed class StagesController : CrudControllerBase<Stage>
{
    public StagesController(ICrudService<Stage> service) : base(service) { }
}
