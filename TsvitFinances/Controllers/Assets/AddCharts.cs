using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Assets;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AddCharts : Controller
{
    readonly protected MainDb _mainDb;

    public AddCharts(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromForm] AddChartDto model)
    {
        var positionEntryNote = await _mainDb.Set<PositionEntryNote>()
          .Where(pen => pen.Asset.PublicId == model.AssetId)
          .Include(pen => pen.Charts)
          .FirstOrDefaultAsync();

        if (positionEntryNote is null)
        {
            return NotFound();
        }

        if (model.Charts == null)
        {
            _mainDb.Add(new PositionEntryNote
            {
                AssetId = positionEntryNote.Asset.Id,
                Asset = positionEntryNote.Asset,
                CreateAt = DateTime.UtcNow,
                Note = model.Note,
                Charts = null!
            });
        }
        else
        {
            await _uploadFiles(model.Charts, positionEntryNote.Id);
        }

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    private async Task _uploadFiles(List<ChartUpload> charts, int positionEntryNoteId)
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
                UploadedDate = DateTime.UtcNow,
                Description = chart.Description
            };

            _mainDb.Add(fileEntity);
        }
    }

    public class AddChartDto
    {
        public Guid AssetId { get; set; }

        public string? Note { get; set; }

        public List<ChartUpload> Charts { get; set; } = [];
    }

    public class ChartUpload
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile File { get; set; }
    }
}
