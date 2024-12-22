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
        var chart = await _mainDb.Set<Chart>()
          .Where(c => c.Asset.PublicId == model.AssetId)
          .FirstOrDefaultAsync();

        if (chart is null)
        {
            return NotFound();
        }

        await _uploadFiles(model.Charts, chart.AssetId);

        return Ok();
    }

    private async Task _uploadFiles(List<ChartUpload> charts, int assetId)
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

    public class AddChartDto
    {
        public Guid AssetId { get; set; }

        public List<ChartUpload> Charts { get; set; } = [];
    }

    public class ChartUpload
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile File { get; set; }
    }
}
