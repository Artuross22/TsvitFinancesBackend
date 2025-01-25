using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.InvestmentIdeas;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class DeleteInvestmentIdea : Controller
{
    protected readonly MainDb _mainDb;

    public DeleteInvestmentIdea(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpDelete("{publicId}")]
    public async Task<IActionResult> Index(Guid publicId)
    {
        var investmentIdea = await _mainDb.Set<InvestmentIdea>()
            .Where(ii => ii.PublicId == publicId)
            .FirstOrDefaultAsync();

        if(investmentIdea == null)
        {
            return NotFound();
        }

        var assets = await _mainDb.Set<Asset>()
            .Where(a => a.InvestmentIdeaId == investmentIdea.Id)
            .ToListAsync();

        foreach (var asset in assets)
        {
            asset.InvestmentIdeaId = null;
        }

        _mainDb.Remove(investmentIdea);
        await _mainDb.SaveChangesAsync();

        return Ok();
    }
}