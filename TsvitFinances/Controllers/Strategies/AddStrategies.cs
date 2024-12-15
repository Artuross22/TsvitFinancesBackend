using Data.Models.Enums;
using Data.Models;
using Data.Modelsl;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TsvitFinances.Controllers.Strategies;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AddStrategies : Controller
{

    readonly protected MainDb _mainDb;
    public AddStrategies(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPost]
    public async Task<IActionResult> Index(AddStrategy model)
    {
        var user = await _mainDb.Set<AppUser>()
            .FirstOrDefaultAsync(c => c.Id == model.UserPublicId);

        if (user == null)
        {
            return NotFound();
        }

        var riskManagement = new RiskManagement
        {
            Name = string.Empty,
            BaseRiskPercentage = 2,
            RiskToRewardRatio = 3,
            HedgeId = null!,
            Hedge = null!,
            Diversification = null!,
        };

        _mainDb.Add(riskManagement);

        var positionManagement = new PositionManagement
        {
            PublicId = Guid.NewGuid(),
            ScalingIn = 0,
            ScalingOut = 20,
            AverageLevel = 5,
            SalesLevels = null!,
            PurchaseLevels = null!,
        };

        _mainDb.Add(positionManagement);

        var strategy = new Strategy
        {
            PublicId = Guid.NewGuid(),
            Category = RiskCategory.Low,
            Description = string.Empty,
            AppUser = user,
            AppUserId = user.Id,
            Name = model.Name,
            PositionManagementId = null!,
            PositionManagement = positionManagement,
            RiskManagementId = null!,
            RiskManagement = riskManagement,
        };

        _mainDb.Add(strategy);

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class AddStrategy
    {
        public required string UserPublicId { get; set; }
        public required string Name { get; set; }
    }
}
