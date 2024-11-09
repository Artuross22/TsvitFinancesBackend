using Data.Db;
using Data.Models;
using Data.Modelsl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsvitFinances.Dto.Strategy;

namespace TsvitFinances.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class StrategiesController : Controller
{
    readonly protected MainDb _mainDb;
    public StrategiesController(MainDb mainDb)
    {
       _mainDb = mainDb;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStrategies(string id)
    {
        var strategies = await _mainDb.Set<Strategy>()
            .Where(s => s.AppUser.Id == id)
            .Select(s => new ListStrategies
            {
              PubliceId = s.PublicId,
              Name = s.Name,
            })
            .ToListAsync();

        if (strategies == null)
        {
            return NotFound();
        }

        return View(strategies);
    }

    [HttpPost]
    public async Task<IActionResult> AddStrategy(AddStrategy model)
    {
        var user = await _mainDb.Set<AppUser>()
            .FirstOrDefaultAsync(c => c.Id == model.UserPublicId);

        if (user == null)
        {
            return NotFound();
        }

        _mainDb.Add(new Strategy
        {   
            AppUser = user,
            AppUserId = user.Id,
            Name = model.Name,
            PositionManagementId = null!,
            PositionManagement = null!,
            RiskManagementId = null!,
            RiskManagement = null!,
        });

        await _mainDb.SaveChangesAsync();

        return Ok();
    }
}
