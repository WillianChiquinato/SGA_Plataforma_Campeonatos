using Microsoft.AspNetCore.Mvc;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Api.Controller;

[Route("api/[controller]")]
public sealed class UserController : CrudControllerBase<User>
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService) : base(userService) 
    {
        _userService = userService;
    }
}
