using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.InvestmentIdeas;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class GetAssetsForIdea : Controller
{
    protected readonly MainDb _mainDb;

    public GetAssetsForIdea(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{userPublicId}")]
    public async Task<IActionResult> Index(string userPublicId)
    {
        var assets = await _mainDb.Set<Asset>()
            .Where(a => a.AppUserId == userPublicId)
            .Select(a => new BindingModel
            {
                PublicId = a.PublicId,
                Name = a.Name
            })
            .ToListAsync();

        if (assets == null)
        {
            return NotFound();
        }

        return Ok(assets);
    }

    private class BindingModel
    {
        public required Guid PublicId { get; set; }
        public required string Name { get; set; }
    }

}