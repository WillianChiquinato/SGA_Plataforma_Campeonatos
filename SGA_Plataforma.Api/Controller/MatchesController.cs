using Microsoft.AspNetCore.Mvc;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Api.Controller;

[Route("api/[controller]")]
public sealed class MatchesController : CrudControllerBase<Match>
{
    public MatchesController(ICrudService<Match> service) : base(service) { }
}
