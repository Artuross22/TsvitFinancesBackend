using Data.Db;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsvitFinances.Dto.Asset;
using TsvitFinances.Dto.AssetDto;
using TsvitFinances.Extensions;

namespace TsvitFinances.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
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
            var asset = await _mainDb.Set<Asset>()
                .ToListAsync();

            return asset;
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

        [HttpGet("AddAsset")]
        public ActionResult<AssetPreCreationDataDto> AddAsset()
        {
            return new AssetPreCreationDataDto
            {
                InvestmentTerms = EnumHelper.GetSelectListFromEnum<InvestmentTerm>(),
                Markets = EnumHelper.GetSelectListFromEnum<Market>(),
                Sectors = EnumHelper.GetSelectListFromEnum<Sector>(),
            };
        }

        [HttpPost]
        public async Task<ActionResult> AddAsset(AddAssetDto model)
        {
            var user = await _mainDb.Users.SingleAsync(u => u.Id == model.UserPublicId);

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                var asset = new Asset
                {
                    PublicId = Guid.NewGuid(),
                    AppUserId = user.Id,
                    AppUser = user,
                    Sector = (Sector)model.Sector,
                    Market = (Market)model.Market,
                    Term = (InvestmentTerm)model.InvestmentTerm,
                    Name = model.Name,
                    Ticker = model.Ticker,
                    AddedAt = DateTime.UtcNow,
                    CurrentPrice = model.CurrentPrice,
                    Quantity = model.Quantity,
                    InterestOnCurrentDeposit = 0,
                    BoughtFor = model.BoughtFor,
                    IsActive = true,
                    ClosedAt = null,
                    SoldFor = null,
                    Charts = null!
                };

                _mainDb.Add(asset);
                await _mainDb.SaveChangesAsync();

               await _uploadFiles(model.Charts, asset.Id);

            }
            catch (DbUpdateException db)
            {
                Console.WriteLine(db);
                return BadRequest(new { message = $"Failed to add the asset to data base. The name of the asset is {model.Name}" });
            }

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<Asset>> UpdateAsset([FromBody] AssetUpdateDto model)
        {
            var user = await _mainDb.Users.SingleAsync(u => u.Id == model.UserPublicId);

            if (user == null)
            {
                return NotFound();
            }

            var asset = await _mainDb.Set<Asset>()
                 .Where(a => a.Id == model.Id)
                 .SingleOrDefaultAsync();

            if (asset is null)
            {
                return NotFound();
            }

            asset.Name = model.Name;
            asset.CurrentPrice = model.CurrentPrice;
            asset.BoughtFor = model.BoughtFor;

            foreach (var chart in asset.Charts)
            {
                var chartDto = model.Charts.Single(a => a.Id == chart.Id);

                chart.Title = chartDto.Title;
                chart.Description = chartDto.Description;
            }

            await _mainDb.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("SellAsset/{id}")]
        public async Task<IActionResult> SellAsset(int id)
        {
            var asset = await _mainDb.Set<Asset>()
                 .Include(a => a.AppUser)
                 .Where(a => a.Id == id)
                 .SingleOrDefaultAsync();

            if (asset is null)
            {
                return NotFound();
            }

            var now = DateTime.UtcNow;

            asset.SoldFor = asset.CurrentPrice;
            asset.ClosedAt = now;
            asset.IsActive = false;

            _mainDb.Add(new BalanceFlow
            {
                AppUser = asset.AppUser,
                Sum = asset.CurrentPrice,
                CreatedOn = now,
                Balance = Balance.InternalRevenue,
            });

            await _mainDb.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Asset>> DeleteAsset(int id)
        {
            var asset = await _mainDb.Set<Asset>()
                 .Where(a => a.Id == id)
                 .SingleOrDefaultAsync();

            if (asset is null)
            {
                return NotFound();
            }

            _mainDb.Remove(asset);
            await _mainDb.SaveChangesAsync();

            return Ok();
        }

        private async Task _uploadFiles(List<IFormFile> files, int assetId)
        {
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine("D:\\Backend\\Uploads", formFile.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    var fileEntity = new Chart
                    {
                        AssetId = assetId,
                        Asset = null!,
                        FileName = formFile.FileName,
                        FilePath = filePath,
                        FileSize = formFile.Length,
                        UploadedDate = DateTime.UtcNow
                    };

                    _mainDb.Add(fileEntity);
                }
            }

            await _mainDb.SaveChangesAsync();
        }
    }
}
