using Data.Models;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TsvitFinances.Dto.User;

namespace TsvitFinances.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtProvider _jwtProvider;

    public AuthController(UserManager<AppUser> userManager, JwtProvider jwtProvider)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModelDto model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = _jwtProvider.GenerateJwtToken(user);

                return Ok(new { Token = token, user.Email });
            }

            return Unauthorized(new { Message = "Invalid credentials" });
        }
        return BadRequest(ModelState);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModelDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new AppUser
        {
            CreatedOn = DateTime.Now,
            UserName = model.Email,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            var token = _jwtProvider.GenerateJwtToken(user);

            return Ok(new { Token = token });
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return BadRequest(ModelState);
    }
}

