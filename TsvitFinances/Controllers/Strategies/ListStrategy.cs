using Data;
using Data.Modelsl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Strategies;

[Route("api/[controller]")]
[ApiController]
public class ListStrategy : Controller
{
    readonly protected MainDb _mainDb;
    public ListStrategy(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> Invoke(string userId)
    {
        var strategies = await _mainDb.Set<Strategy>()
            .Where(s => s.AppUser.Id == userId)
            .Select(s => new
            {
                s.PublicId,
                s.Name,
            })
            .ToListAsync();

        if (strategies == null)
        {
            return NotFound();
        }

        return Json(strategies);
    }
}
