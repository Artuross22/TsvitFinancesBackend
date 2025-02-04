using Data;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.UserManagement;

public class TransferBalanceFlow : Controller
{
    readonly protected MainDb _mainDb;

    public TransferBalanceFlow(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPost]
    public async Task<IActionResult> Index(BindingModel model)
    {
        var fromBalanceFlowSum = await _mainDb.Set<BalanceFlow>()
            .Where(b => b.Balance == model.ToBalanceFlow.BalanceType)
            .Where(b => b.AppUserId == model.AppUserId)
            .SumAsync(b => b.Sum);

        var toBalanceFlow = await _mainDb.Set<BalanceFlow>()
            .Where(u => u.Id == model.ToBalanceFlow.Id)
            .Where(u => u.AppUserId == model.AppUserId)
            .SingleOrDefaultAsync();

        if (toBalanceFlow == null)
        {
            return NotFound();
        }

        var fromBalanceSum = fromBalanceFlowSum - model.FromBalanceFlow.Sum;

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
                    AppUser = toBalanceFlow.AppUser,
                    CreatedOn = DateTime.Now
                });

                _mainDb.Add(new BalanceFlow
                {
                    Sum = model.ToBalanceFlow.Sum,
                    Balance = model.ToBalanceFlow.BalanceType,
                    AppUserId = model.AppUserId,
                    AppUser = toBalanceFlow.AppUser,
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
            public required int Id { get; set; }

            public required decimal Sum { get; set; }

            public required Balance BalanceType { get; set; }
        }
    }
}
