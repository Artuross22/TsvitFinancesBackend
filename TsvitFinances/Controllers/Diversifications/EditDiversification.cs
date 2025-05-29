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
public class EditDiversification : Controller
{
    readonly protected MainDb _mainDb;

    public EditDiversification(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{publicId}")]
    public async Task<ActionResult> Index(Guid publicId)
    {
        var diversification = await _mainDb.Set<Diversification>()
            .Where(a => a.RiskManagement.PublicId == publicId)
            .Select(a => new _Diversification
            {
                Id = a.Id,
                NichePercentage = a.NichePercentage,
                Sector = a.Sector
            })
            .ToListAsync();

        if (diversification == null)
        {
            return NotFound();
        }

        return Ok(new BindingModel
        {
            PublicId = publicId,
            Diversifications = diversification
        });
    }

    [HttpPost]
    public async Task<ActionResult> Index(BindingModel model)
    {
        var riskManagement = await _mainDb.Set<RiskManagement>()
            .Include(s => s.Diversification)
            .FirstOrDefaultAsync(a => a.PublicId == model.PublicId);

        if (riskManagement == null)
        {
            return NotFound();
        }

        var diversificationIds = riskManagement.Diversification
            .Select(d => d.Id)
            .ToList();

        foreach (var diversification in model.Diversifications)
        {
            if (diversificationIds.Contains(diversification.Id))
            {
                var currentDiversification = riskManagement.Diversification.Single(r => r.Id == diversification.Id);

                currentDiversification.Sector = diversification.Sector;
                currentDiversification.NichePercentage = diversification.NichePercentage;
            }
            else
            {
                _mainDb.Add(new Diversification
                {
                    PublicId = Guid.NewGuid(),
                    NichePercentage = diversification.NichePercentage,
                    MinimumAssetsPerNiche = diversification.MinimumAssetsPerNiche,
                    Sector = diversification.Sector,
                    RiskManagementId = riskManagement.Id,
                    RiskManagement = riskManagement
                });
            }
        }

        foreach (var diversificationId in diversificationIds)
        {
            if (!model.Diversifications.Select(d => d.Id).Contains(diversificationId))
            {
                _mainDb.Remove(riskManagement.Diversification.Single(d => d.Id == diversificationId));
            }
        }

        await _mainDb.SaveChangesAsync();

        return Ok(riskManagement.PublicId);
    }
    public class BindingModel
    {
        public Guid PublicId { get; set; }

        public List<_Diversification> Diversifications { get; set; } = [];
    }
    public class _Diversification
    {
        public int Id { get; set; }
        public decimal NichePercentage { get; set; }
        public int MinimumAssetsPerNiche { get;set; }
        public required Sector Sector { get; set; }
    }
}
