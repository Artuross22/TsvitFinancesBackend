using Data.Modelsl;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TsvitFinances.Controllers.Strategies;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class GetStrategy : Controller
{
    readonly protected MainDb _mainDb;
    public GetStrategy(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{publicId}/{userId}")]
    public async Task<IActionResult> Index(Guid publicId, string userId)
    {
        var strategy = await _mainDb.Set<Strategy>()
            .Include(s => s.RiskManagement)
            .Include(s => s.PositionManagement)
            .Where(s => s.PublicId == publicId)
            .Where(s => s.AppUser.Id == userId)
            .FirstOrDefaultAsync();

        if (strategy == null)
        {
            return NotFound();
        }

        return Json(strategy);
    }
}
