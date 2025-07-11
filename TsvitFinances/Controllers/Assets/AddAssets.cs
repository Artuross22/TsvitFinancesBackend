using Data;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TsvitFinances.Extensions;

namespace TsvitFinances.Controllers.Assets;

[Route("api/[controller]")]
[ApiController]
public class AddAssets : Controller
{
    readonly protected MainDb _mainDb;

    public AddAssets(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet]
    public ActionResult<AssetPreCreationDataDto> Invoke()
    {
        return new AssetPreCreationDataDto
        {
            InvestmentTerms = EnumHelper.GetSelectListFromEnum<InvestmentTerm>(),
            Markets = EnumHelper.GetSelectListFromEnum<Market>(),
            Sectors = EnumHelper.GetSelectListFromEnum<Sector>(),
        };
    }

    [HttpPost]
    public async Task<ActionResult> Invoke([FromForm] AddAssetDto model)
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
                Goal = model.MainGoal,
                Ticker = model.Ticker,
                AddedAt = DateTime.UtcNow,
                CurrentPrice = model.CurrentPrice,
                Quantity = model.Quantity,
                InterestOnCurrentDeposit = 0,
                BoughtFor = model.BoughtFor,
                IsActive = true,
                ClosedAt = null,
                SoldFor = null,
                PositionEntryNotes = null!,
                StrategyId = null!,
                Strategy = null!,
                InvestmentIdeaId = null!,
                InvestmentIdea = null!,
                SalesLevels = null!,
                PurchaseLevels = null!,
            };

            var positionEntryNote = new PositionEntryNote
            {
                Asset = asset,
                AssetId = asset.Id,
                PublicId = Guid.NewGuid(),
                Charts = null!,
                Note = null!,
                CreateAt = DateTime.UtcNow
            };

            _mainDb.Add(positionEntryNote);
            _mainDb.Add(asset);
            await _mainDb.SaveChangesAsync();

            if (model.Charts != null)
            {
                await _uploadFiles(model.Charts, positionEntryNote.Id);
            }
        }
        catch (DbUpdateException db)
        {
            Console.WriteLine(db);
            return BadRequest(new { message = $"Failed to add the asset to data base. The name of the asset is {model.Name}" });
        }

        return Ok();
    }

    private async Task _uploadFiles(List<ChartDto> charts, int positionEntryNoteId)
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
                PositionEntryNoteId = positionEntryNoteId,
                PositionEntryNote = null!,
                FileName = chart.Name,
                FilePath = filePath,
                FileSize = chart.File.Length,
                UploadedDate = DateTime.UtcNow
            };

            _mainDb.Add(fileEntity);
        }

        await _mainDb.SaveChangesAsync();
    }

    public class AddAssetDto
    {
        public required string UserPublicId { get; set; }

        public required string Name { get; set; }

        public required string MainGoal { get; set; }

        public required string Ticker { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal BoughtFor { get; set; }

        public decimal Quantity { get; set; }

        public required int Sector { get; set; }

        public required int InvestmentTerm { get; set; }

        public required int Market { get; set; }

        public List<ChartDto>? Charts { get; set; }
    }

    public class ChartDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile File { get; set; }
    }

    public class AssetPreCreationDataDto
    {
        public required List<SelectListItem> Sectors { get; set; }
        public required List<SelectListItem> Markets { get; set; }
        public required List<SelectListItem> InvestmentTerms { get; set; }
    }
}
