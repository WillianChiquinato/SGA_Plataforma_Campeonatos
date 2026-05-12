using Microsoft.AspNetCore.Mvc;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Api.Controller;

[Route("api/[controller]")]
public sealed class MatchMapsController : CrudControllerBase<MatchMap>
{
    public MatchMapsController(ICrudService<MatchMap> service) : base(service) { }
}
