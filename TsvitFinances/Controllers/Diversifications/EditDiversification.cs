using Data;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Diversifications
{
    public class EditDiversification : Controller
    {
        readonly protected MainDb _mainDb;

        public EditDiversification(MainDb mainDb)
        {
            _mainDb = mainDb;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid publicId)
        {
            var diversification = await _mainDb.Set<Diversification>()
                .Where(a => a.PublicId == publicId)
                .Select(a => new BindingModel
                {
                    PublicId = a.PublicId,
                    NichePercentage = a.NichePercentage,
                    Sector = a.Sector
                })
                .FirstOrDefaultAsync();

            if (diversification == null)
            {
                return NotFound();
            }

            await _mainDb.SaveChangesAsync();

            return Ok(diversification);
        }

        [HttpPost]
        public async Task<ActionResult> Index(BindingModel model)
        {
            var diversification = await _mainDb.Set<Diversification>()
                .SingleAsync(a => a.PublicId == model.PublicId);

            if (diversification == null)
            {
                return NotFound();
            }

            diversification.NichePercentage = model.NichePercentage;
            diversification.Sector = model.Sector;

            await _mainDb.SaveChangesAsync();

            return Ok();
        }
        public class BindingModel
        {
            public Guid PublicId { get; set; }

            public decimal NichePercentage { get; set; }

            public required Sector Sector { get; set; }
        }
    }
}
