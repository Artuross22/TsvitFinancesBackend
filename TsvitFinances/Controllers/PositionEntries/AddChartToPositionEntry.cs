using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.PositionEntries;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AddChartToPositionEntry : Controller
{
    readonly protected MainDb _mainDb;
    public AddChartToPositionEntry(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromForm] BindingModel model)
    {
        var positionEntry = await _mainDb.Set<PositionEntryNote>()
            .FirstOrDefaultAsync(c => c.PublicId == model.PublicId);

        if (positionEntry is null)
        {
            return NotFound();
        }

        await _uploadFiles(model.Charts, positionEntry.Id);

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

    public class BindingModel
    {
        public Guid PublicId { get; set; }

        public List<ChartUpload> Charts { get; set; } = [];
    }

    public class ChartUpload
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile File { get; set; }
    }
}
