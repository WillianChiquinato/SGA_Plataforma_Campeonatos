using Microsoft.AspNetCore.Mvc;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Api.Controller;

[Route("api/[controller]")]
public sealed class RoundsController : CrudControllerBase<Round>
{
    public RoundsController(ICrudService<Round> service) : base(service) { }
}
