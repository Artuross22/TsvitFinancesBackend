using Data;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Strategies;

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
    public async Task<IActionResult> Invoke(PositionManagementDto model)
    {
        var positionManagement = await _mainDb.Set<PositionManagement>()
            .Include(s => s.PositionScalings)
            .Where(s => s.PublicId == model.PublicId)
            .FirstOrDefaultAsync();

        if (positionManagement == null)
        {
            return NotFound();
        }

        positionManagement.AverageLevel = model.AverageLevel;

        var modelPositionScalingIds = model.PositionScalings.Select(a => a.PublicId);

        foreach (var positionScaling in positionManagement.PositionScalings)
        {
            if (!modelPositionScalingIds.Contains(positionScaling.PublicId))
            {
                positionManagement.PositionScalings.Remove(positionScaling);
            }
        }

        foreach (var positionScaling in model.PositionScalings)
        {
            var existingPositionScaling = positionManagement.PositionScalings
                .FirstOrDefault(ps => ps.PublicId == positionScaling.PublicId);

            if (existingPositionScaling != null)
            {
                existingPositionScaling.EquityPercentage = positionScaling.EquityPercentage;
                existingPositionScaling.PositionType = positionScaling.PositionType;
            }
            else
            {
                positionManagement.PositionScalings.Add(new PositionScaling
                {
                    PublicId = Guid.NewGuid(),
                    EquityPercentage = positionScaling.EquityPercentage,
                    PositionType = positionScaling.PositionType,
                    PositionManagementId = positionManagement.Id,
                    PositionManagement = positionManagement,
                });
            }
        }

        await _mainDb.SaveChangesAsync();

        _mainDb.SaveChanges();

        return Ok();
    }

    public class PositionManagementDto
    {
        public required Guid PublicId { get; init; }

        public required decimal AverageLevel { get; set; }

        public required IList<_PositionScaling> PositionScalings { get; set; }
    }

    public class _PositionScaling
    {
        public required Guid PublicId { get; init; }

        public required decimal EquityPercentage { get; set; }

        public required PositionType PositionType { get; set; }
    }
}