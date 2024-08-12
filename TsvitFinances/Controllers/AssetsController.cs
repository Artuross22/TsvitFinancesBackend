using Data.Db;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsvitFinances.Dto;

namespace TsvitFinances.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : Controller
    {
        readonly protected MainDb _mainDb;

        public AssetsController(MainDb mainDb)
        {
            _mainDb = mainDb;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
        {
            return await _mainDb.Set<Asset>()
                .Include(c => c.Charts)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Asset>> GetAsset(int id)
        {
            var product = await _mainDb.Set<Asset>()
                .Include(c => c.Charts)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (product is null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult> AddAsset([FromBody] AssetDto model)
        {
            var user = await _mainDb.Users.SingleAsync(u => u.PublicId == model.UserPublicId);

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                model.AddedAt = DateTime.UtcNow;
                model.PublicId = Guid.NewGuid();
                model.IsActive = true;

                var asset = new Asset
                {
                    PublicId = Guid.NewGuid(),
                    AppUserId = user.Id,
                    AppUser = user,
                    Sector = model.Sector,
                    Name = model.Name,
                    Ticker = model.Ticker,
                    AddedAt = DateTime.UtcNow,
                    CurrentPrice = model.CurrentPrice,
                    BoughtFor = model.BoughtFor,
                    IsActive = true,
                    ClosedAt = null,
                    SoldFor = null,
                };

                _mainDb.Add(asset);
                await _mainDb.SaveChangesAsync();

                foreach (var chartDto in model.Charts)
                {
                    _mainDb.Add(new Chart
                    {
                        AssetId = asset.Id,
                        Asset = asset,
                        Title = chartDto.Title,
                        Description = chartDto.Description,
                        ImageData = chartDto.ImageData,
                    });
                }

                await _mainDb.SaveChangesAsync();
            }
            catch (DbUpdateException db)
            {
                Console.WriteLine(db);
                return BadRequest(new { message = $"Failed to add the asset to data base. The name of the asset is {model.Name}" });
            }

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<Asset>> UpdateAsset([FromBody] AssetDto model)
        {
            var asset = await _mainDb.Set<Asset>()
                 .Where(a => a.Id == model.Id)
                 .SingleOrDefaultAsync();

            if (asset is null)
            {
                return NotFound();
            }

            asset.Name = model.Name;
            asset.CurrentPrice = model.CurrentPrice;
            asset.AddedAt = model.AddedAt;
            asset.ClosedAt = model.ClosedAt;
            asset.IsActive = model.IsActive;
            asset.BoughtFor = model.BoughtFor;
            asset.SoldFor = model.SoldFor;

            foreach (var chart in asset.Charts)
            {
                var chartDto = model.Charts.Single(a => a.Id == chart.Id);

                chart.Title = chartDto.Title;
                chart.ImageData = chartDto.ImageData;
                chart.Description = chartDto.Description;
            }

            await _mainDb.SaveChangesAsync();

            return Ok(await GetAssets());
        }

        [HttpDelete]
        public async Task<ActionResult<Asset>> DeleteAsset(int id)
        {
            var dbAsset = await _mainDb.Set<Asset>()
                 .Where(a => a.Id == id)
                 .SingleOrDefaultAsync();

            if (dbAsset is null)
            {
                return NotFound();
            }

            _mainDb.Remove(dbAsset);
            await _mainDb.SaveChangesAsync();

            return Ok(await GetAssets());
        }
    }
}
