using Data;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.UserManagement;

[Route("api/[controller]")]
[ApiController]
public class AddBalanceFlow : Controller
{
    readonly protected MainDb _mainDb;

    public AddBalanceFlow(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPost]
    public async Task<IActionResult> Invoke(BindingModel model)
    {
        var user = await _mainDb.Set<AppUser>()
            .Where(u => u.Id == model.AppUserId)
            .SingleOrDefaultAsync();

        if (user == null)
        {
            return NotFound();
        }

        foreach (var balanceFlow in model.BalanceFlows)
        {
            _mainDb.Add(new BalanceFlow
            {
                AppUserId = model.AppUserId,
                AppUser = user,
                Sum = balanceFlow.Sum,
                Balance = balanceFlow.BalanceType,
                CreatedOn = DateTime.Now
            });
        }

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class BindingModel
    {
        public required string AppUserId { get; set; }

        public List<_BalanceFlow> BalanceFlows { get; set; }

        public class _BalanceFlow
        {
            public required decimal Sum { get; set; }

            public required Balance BalanceType { get; set; }

        }
    }
}