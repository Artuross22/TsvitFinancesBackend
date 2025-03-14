using Data;
using Data.Models;
using FinancialData.APIs.FPM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.InvestmentIdeas;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class ListInvestmentIdeas : Controller
{
    protected readonly MainDb _mainDb;
    protected readonly FpmConnection _connection;

    public ListInvestmentIdeas(MainDb mainDb, FpmConnection connection)
    {
        _mainDb = mainDb;
        _connection = connection;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string id)
    {
        var ss = await _connection.GetCrypto("WLDUSD");

        var investmentIdeas = await _mainDb.Set<InvestmentIdea>()
            .Where(a => a.AppUser.Id == id)
            .Select(a => new BindingModel
            {
                PublicId = a.PublicId,
                Name = a.Name,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();

        return Ok(investmentIdeas);
    }

    public class BindingModel
    {
        public required Guid PublicId { get; set; }
        public required string Name { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}