using Microsoft.AspNetCore.Mvc;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Api.Controller;

[Route("api/[controller]")]
public sealed class TeamsController : CrudControllerBase<Team>
{
    public TeamsController(ICrudService<Team> service) : base(service) { }
}
