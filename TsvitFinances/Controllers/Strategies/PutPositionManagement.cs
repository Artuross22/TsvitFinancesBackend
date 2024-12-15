using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Strategies;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class PutPositionManagement : Controller
{
    readonly protected MainDb _mainDb;
    public PutPositionManagement(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPut]
    public async Task<IActionResult> Index(PositionManagementDto model)
    {
        var positionManagement = await _mainDb.Set<PositionManagement>()
            .Where(s => s.PublicId == model.PublicId)
            .FirstOrDefaultAsync();

        if (positionManagement == null)
        {
            return NotFound();
        }

        positionManagement.ScalingOut = model.ScalingOut;
        positionManagement.ScalingIn = model.ScalingIn;
        positionManagement.AverageLevel = model.AverageLevel;

        _mainDb.SaveChanges();

        return Ok();
    }

    public class PositionManagementDto
    {
        public required Guid PublicId { get; init; }

        public required decimal? ScalingOut { get; set; }

        public required decimal? ScalingIn { get; set; }

        public required decimal AverageLevel { get; set; }
    }
}
