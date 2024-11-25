using Data.Db;
using Data.Models;
using Data.Models.Enums;
using Data.Modelsl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsvitFinances.Dto.Asset;
using TsvitFinances.Dto.Asset.Output;
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
        public async Task<ActionResult<GetAssetsDto>> GetAsset(Guid id)
        {
            var asset = await _mainDb.Set<Asset>()
                .Include(c => c.Charts)
                .Include(c => c.Strategy)
                .FirstOrDefaultAsync(a => a.PublicId == id);

            if (asset == null)
            {
                return NotFound();
            }

            var output = new GetAssetsDto
            {
                PublicId = asset.PublicId,
                StrategyPublicId = asset.Strategy?.PublicId,
                StrategyName = asset.Strategy?.Name,
                AddedAt = asset.AddedAt,
                BoughtFor = asset.BoughtFor,
                CurrentPrice = asset.CurrentPrice,
                InterestOnCurrentDeposit = asset.InterestOnCurrentDeposit,
                Market = asset.Market.ToString(),
                Sector = asset.Sector.ToString(),
                Term = asset.Term.ToString(),
                Name = asset.Name,
                Ticker = asset.Ticker,
                Quantity = asset.Quantity,
            };

            foreach (var item in asset.Charts)
            {
                output.Charts?.Add(new GetAssetsDto._Chart
                {
                    Name = item.FileName,
                    Description = item.Description,
                    ChartsPath = item.FilePath.Substring(item.FilePath.IndexOf("public") + "public".Length).Replace("\\", "/"),
                });
            }


            if (asset is null)
            {
                return NotFound();
            }

            return output;
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
        public async Task<ActionResult> AddAsset([FromForm] AddAssetDto model)
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
                    Charts = null!,
                    StrategyId = null!,
                    Strategy = null!,           
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
        public async Task<ActionResult<Asset>> UpdateAsset(AssetUpdateDto model)
        {
            var user = await _mainDb.Users.SingleAsync(u => u.Id == model.UserPublicId);

            if (user == null)
            {
                return NotFound();
            }

            var asset = await _mainDb.Set<Asset>()
                 .Where(a => a.PublicId == model.PublicId)
                 .SingleOrDefaultAsync();

            if (asset is null)
            {
                return NotFound();
            }

            asset.Name = model.Name;
            asset.CurrentPrice = model.CurrentPrice;
            asset.BoughtFor = model.BoughtFor;


            await _mainDb.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("SellAsset/{id}")]
        public async Task<IActionResult> SellAsset(Guid id)
        {
            var asset = await _mainDb.Set<Asset>()
                 .Include(a => a.AppUser)
                 .Where(a => a.PublicId == id)
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
                AppUserId = asset.AppUserId,
                Sum = asset.CurrentPrice,
                CreatedOn = now,
                Balance = Balance.InternalRevenue,
            });

            await _mainDb.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Asset>> DeleteAsset(Guid id)
        {
            var asset = await _mainDb.Set<Asset>()
                 .Where(a => a.PublicId == id)
                 .SingleOrDefaultAsync();

            if (asset is null)
            {
                return NotFound();
            }

            _mainDb.Remove(asset);
            await _mainDb.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("DeleteChart/{id}/{assetId}")]
        public async Task<IActionResult> DeleteChart(int id, Guid assetId)
        {
            var chart = await _mainDb.Set<Chart>()
                .Where(c => c.Asset.PublicId == assetId)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chart is null)
            {
                return NotFound();
            }

            _mainDb.Remove(chart);
            await _mainDb.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("AddChart")]
        public async Task<IActionResult> AddChart([FromForm] AddChartDto model)
        {
            var chart = await _mainDb.Set<Chart>()
              .Where(c => c.Asset.PublicId == model.AssetId)
              .FirstOrDefaultAsync();

            if(chart is null)
            {
                return NotFound();
            }

            await _uploadFiles(model.Charts, chart.AssetId);

            return Ok();
        }


        [HttpPut("UpdateChart")]
        public async Task<IActionResult> UpdateChart(UpdateChartDto model)
        {
            var chart = await _mainDb.Set<Chart>().FirstOrDefaultAsync(c => c.Id == model.Id);

            if (chart is null)
            {
                return NotFound();
            }

            chart.FileName = model.Name;
            chart.Description = model.Description;

            await _mainDb.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("GetChartsByAssetId/{id}")]
        public async Task<IActionResult> GetChartsByAssetId(Guid id)
        {
            var charts = await _mainDb.Set<Asset>().Where(a => a.PublicId == id)
                .Include(a => a.Charts)
                .FirstOrDefaultAsync();

            if (charts is null)
            {
                return NotFound();
            }

            var output = new GetCharts
            {
                AssetPublicId = id,
            };

            foreach (var item in charts.Charts)
            {
                output.Charts?.Add(new GetCharts._Chart
                {
                    Id = item.Id,
                    Name = item.FileName,
                    Description = item.Description,
                    ChartsPath = item.FilePath.Substring(item.FilePath.IndexOf("public") + "public".Length).Replace("\\", "/"),
                });
            }

            return Ok(output);
        }

        private async Task _uploadFiles(List<ChartDto> charts, int assetId)
        {
            string now = DateTime.UtcNow.Date.ToString("dd/MM/yyyy");

            string directoryPath = Path.Combine("D:\\TsvitFund\\TsvitFinances\\tsvit\\public\\uploads\\", now);

            Directory.CreateDirectory(directoryPath);

            foreach (var chart in charts)
            {
                var filePath = Path.Combine(directoryPath, chart.Name);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await chart.File.CopyToAsync(stream);
                }

                var fileEntity = new Chart
                {
                    AssetId = assetId,
                    Asset = null!,
                    FileName = chart.Name,
                    FilePath = filePath,
                    FileSize = chart.File.Length,
                    UploadedDate = DateTime.UtcNow
                };

                _mainDb.Add(fileEntity);
            }

            await _mainDb.SaveChangesAsync();
        }
    }
}