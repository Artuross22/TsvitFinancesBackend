using Data;
using Data.Models;
using Data.Modelsl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        var ss = new FinanceData
        {
            PublicId = Guid.NewGuid()
        };

        _mainDb.Add(ss);
        await _mainDb.SaveChangesAsync();


        var strategy = await _mainDb.Set<Strategy>()
            .Include(s => s.FinanceData)
            .Include(s => s.RiskManagement)
            .Include(s => s.PositionManagement)
            .Where(s => s.PublicId == publicId)
            .Where(s => s.AppUser.Id == userId)
            .FirstOrDefaultAsync();

        strategy.FinanceDataId = ss.Id;

        await _mainDb.SaveChangesAsync();

        if (strategy == null)
        {
            return NotFound();
        }

        return Json(strategy);
    }
}
