using Data;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Macroeconomic;

[Route("api/[controller]")]
[ApiController]
public class CreateMacroeconomic : Controller
{
    protected readonly MainDb _mainDb;

    public CreateMacroeconomic(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPost]
    public async Task<IActionResult> Index(BindingModel model)
    {
        var user = await _mainDb.Set<AppUser>()
            .FirstOrDefaultAsync(u => u.Id == model.UserId);

        if (user == null)
        {
            return NotFound();
        }

        _mainDb.Add(new MacroeconomicAnalysis
        {
            PublicId = model.PublicId,
            AppUser = user,
            AppUserId = user.Id,
            Description = model.Description,
            EconomicType = model.EconomicType,
            Title = model.Title,
            ArchivedAt = null,
        });

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class BindingModel
    {
        public required string UserId { get; set; }

        public required Guid PublicId { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public required EconomicType EconomicType { get; set; }
    }
}
