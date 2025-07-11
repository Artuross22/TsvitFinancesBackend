using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.InvestmentIdeas;

[Route("api/[controller]")]
[ApiController]
public class ViewInvestmentIdea : Controller
{
    protected readonly MainDb _mainDb;

    public ViewInvestmentIdea(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{publicId}")]
    public async Task<IActionResult> Invoke(Guid publicId)
    {
        var investmentIdea = await _mainDb.Set<InvestmentIdea>()
            .Where(ii => ii.PublicId == publicId)
            .Select(ii => new BindingModel
            {
                PublicId = ii.PublicId,
                Name = ii.Name,
                Description = ii.Description,
                ExpectedReturn = ii.ExpectedReturn,
                Profit = ii.Profit,
                CreatedAt = ii.CreatedAt,
                Assets = ii.Assets.Select(a => new _Asset
                {
                    PublicId = a.PublicId,
                    Name = a.Name
                }).ToList()
            })
            .SingleOrDefaultAsync();

        if (investmentIdea == null)
        {
            return NotFound();
        }

        return Ok(investmentIdea);
    }

    public class BindingModel
    {
        public Guid PublicId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal ExpectedReturn { get; set; }
        public decimal? Profit { get; set; }
        public required DateTime CreatedAt { get; set; }

        public List<_Asset>? Assets { get; set; }
    }

    public class _Asset
    {
        public required Guid PublicId { get; set; }
        public required string Name { get; set; }
    }
}