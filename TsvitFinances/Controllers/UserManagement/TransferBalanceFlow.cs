using Data;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.UserManagement;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class TransferBalanceFlow : Controller
{
    readonly protected MainDb _mainDb;

    public TransferBalanceFlow(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPost]
    public async Task<IActionResult> Invoke(BindingModel model)
    {
        var appUser = await _mainDb.Set<AppUser>()
            .Include(u => u.BalanceFlows)
            .Where(u => u.Id == model.AppUserId)
            .Where(u => u.BalanceFlows != null && u.BalanceFlows.Any(t => t.Balance == model.FromBalanceFlow.BalanceType))
            .SingleOrDefaultAsync();

        if (appUser == null)
        {
            return NotFound();
        }

        var fromBalanceSum = appUser.BalanceFlows?
            .Where(b => b.Balance == model.FromBalanceFlow.BalanceType)
            .Sum(s => s.Sum) - model.FromBalanceFlow.Sum;

        if (fromBalanceSum < 0)
        {
            return BadRequest();
        }

        using (var transaction = await _mainDb.Database.BeginTransactionAsync())
        {
            try
            {
                _mainDb.Add(new BalanceFlow
                {
                    Sum = -model.FromBalanceFlow.Sum,
                    Balance = model.FromBalanceFlow.BalanceType,
                    AppUserId = model.AppUserId,
                    AppUser = appUser,
                    CreatedOn = DateTime.Now
                });

                _mainDb.Add(new BalanceFlow
                {
                    Sum = model.ToBalanceFlow.Sum,
                    Balance = model.ToBalanceFlow.BalanceType,
                    AppUserId = model.AppUserId,
                    AppUser = appUser,
                    CreatedOn = DateTime.Now
                });

                await _mainDb.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        return Ok(model);
    }

    public class BindingModel
    {
        public required string AppUserId { get; set; }

        public required _BalanceFlow FromBalanceFlow { get; set; }

        public required _BalanceFlow ToBalanceFlow { get; set; }

        public class _BalanceFlow
        {
            public required decimal Sum { get; set; }

            public required Balance BalanceType { get; set; }
        }
    }
}