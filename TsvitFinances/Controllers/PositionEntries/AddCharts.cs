using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace TsvitFinances.Controllers.PositionEntries;

[Route("api/[controller]")]
[ApiController]
public class AddCharts : Controller
{
     protected readonly MainDb _mainDb;

    public AddCharts(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPost]
    public async Task<IActionResult> Invoke([FromForm] AddChartDto model)
    {
        if (Request.HasJsonContentType())
        {
            using var reader = new StreamReader(Request.Body);

            var body = await reader.ReadToEndAsync();

            if (string.IsNullOrEmpty(body))
            {
                return BadRequest();
            }

            model = JsonConvert.DeserializeObject<AddChartDto>(body)!;
        }

        var asset = await _mainDb.Set<Asset>()
          .Where(pen => pen.PublicId == model.AssetId)
          .FirstOrDefaultAsync();

        if (asset is null)
        {
            return NotFound();
        }

        var positionEntryNote = new PositionEntryNote
        {
            AssetId = asset.Id,
            PublicId = Guid.NewGuid(),
            Asset = null!,
            CreateAt = DateTime.UtcNow,
            Note = model.Note,
            Charts = null!
        };

        _mainDb.Add(positionEntryNote);

        await _mainDb.SaveChangesAsync();

        if (model.Charts.Count != 0)
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
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile File { get; set; }
    }
}