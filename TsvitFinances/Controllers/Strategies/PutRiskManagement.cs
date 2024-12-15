using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Strategies;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class PutRiskManagement : Controller
{
    readonly protected MainDb _mainDb;
    public PutRiskManagement(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPut]
    public async Task<IActionResult> Index(RiskManagementDto model)
    {
        var riskManagement = await _mainDb.Set<RiskManagement>()
            .Where(s => s.PublicId == model.PublicId)
            .FirstOrDefaultAsync();

        if (riskManagement == null)
        {
            return NotFound();
        }

        riskManagement.RiskToRewardRatio = model.RiskToRewardRatio;
        riskManagement.BaseRiskPercentage = model.BaseRiskPercentage;

        _mainDb.SaveChanges();

        return Ok();
    }

    public class RiskManagementDto
    {
        public required Guid PublicId { get; set; }
        public required decimal RiskToRewardRatio { get; set; }
        public required decimal BaseRiskPercentage { get; set; }
    }
}
