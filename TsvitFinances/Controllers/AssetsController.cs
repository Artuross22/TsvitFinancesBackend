using Data.Db;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
        {
            return await _mainDb.Assets.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Asset>> GetAsset(int id)
        {
            var product = await _mainDb.Assets.FirstOrDefaultAsync(a => a.Id == id);

            if (product is null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Asset>> AddAsset([FromBody] Asset asset)
        {
            try
            {
                asset.AddedAt = DateTime.UtcNow;
                asset.PublicId = Guid.NewGuid();
                asset.IsActive = true;

                _mainDb.Assets.Add(asset);
                await _mainDb.SaveChangesAsync();
            }
            catch (DbUpdateException db)
            {
                Console.WriteLine(db);
                return BadRequest(new { message = $"Failed to add the asset to data base. The name of the asset is {asset.Name}"});
            }

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<Asset>> UpdateAsset([FromBody] Asset asset)
        {
            var dbAsset = await _mainDb.Assets
                 .Where(a => a.Id == asset.Id)
                 .SingleOrDefaultAsync();

            if (dbAsset is null)
            {
                return NotFound();
            }

            dbAsset.Name = asset.Name;
            dbAsset.CurrentPrice = asset.CurrentPrice;
            dbAsset.AddedAt = asset.AddedAt;
            dbAsset.ClosedAt = asset.ClosedAt;
            dbAsset.IsActive = asset.IsActive;
            dbAsset.BoughtFor = asset.BoughtFor;
            dbAsset.SoldFor = asset.SoldFor;

            await _mainDb.SaveChangesAsync();

            return Ok(await GetAssets());
        }

        [HttpDelete]
        public async Task<ActionResult<Asset>> DeleteAsset(int id)
        {
            var dbAsset = await _mainDb.Assets
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
