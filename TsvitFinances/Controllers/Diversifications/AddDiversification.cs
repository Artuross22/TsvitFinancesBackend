using Data;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Diversifications;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AddDiversification : Controller
{
    readonly protected MainDb _mainDb;

    public AddDiversification(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPost]
    public async Task<ActionResult> Index(BindingModel model)
    {
        var riskManagement = await _mainDb.Set<RiskManagement>()
            .FirstOrDefaultAsync(a => a.PublicId == model.PublicId);

        if (riskManagement == null)
        {
            return NotFound();
        }

        foreach (var diversification in model.Diversifications)
        {
            _mainDb.Add(new Diversification
            {
                PublicId = Guid.NewGuid(),
                NichePercentage = diversification.NichePercentage,
                Sector = diversification.Sector,
                RiskManagementId = riskManagement.Id,
                RiskManagement = riskManagement
            });
        }

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class BindingModel
    {
        public Guid PublicId { get; set; }

        public List<_Diversification> Diversifications { get; set; } = [];

        public class _Diversification
        {
            public decimal NichePercentage { get; set; }

            public required Sector Sector { get; set; }
        }
    }
}
