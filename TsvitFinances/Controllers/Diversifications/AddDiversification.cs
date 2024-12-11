using Data.Db;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Diversifications;

public class AddDiversification : Controller
{
    readonly protected MainDb _mainDb;

    public AddDiversification(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    public async Task<ActionResult> Index(BindingModel model)
    {
        var riskManagement = await _mainDb.Set<RiskManagement>()
            .FirstOrDefaultAsync(a => a.PublicId == model.RiskManagementPublicId);

        if (riskManagement == null)
        {
            return NotFound();
        }

        _mainDb.Add(new Diversification
        {
            NichePercentage = model.RecommendedNichePercentage,
            Sector = model.Sector,
            RiskManagementId = riskManagement.Id,
            RiskManagement = riskManagement
        });

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class BindingModel
    {
        public Guid RiskManagementPublicId { get; set; }

        public decimal RecommendedNichePercentage { get; set; }

        public required Sector Sector { get; set; }
    }
}
