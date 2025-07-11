using Data;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Diversifications;

[Route("api/[controller]")]
[ApiController]
public class ViewDiversification : Controller
{
    readonly protected MainDb _mainDb;

    public ViewDiversification(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet]
    public async Task<ActionResult> Invoke(Guid publicId)
    {
        var diversifications = await _mainDb.Set<Diversification>()
            .Where(a => a.PublicId == publicId)
            .Select(a => new BindingModel
            {
                PublicId = a.PublicId,
                NichePercentage = a.NichePercentage,
                MinimumAssetsPerNiche = a.MinimumAssetsPerNiche,
                Sector = a.Sector
            })
            .ToListAsync();

        if (diversifications == null)
        {
            return NotFound();
        }

        await _mainDb.SaveChangesAsync();

        return Ok(diversifications);
    }

    private class BindingModel
    {
        public Guid PublicId { get; set; }

        public decimal NichePercentage { get; set; }

        public required int MinimumAssetsPerNiche { get; set; }

        public required Sector Sector { get; set; }
    }
}