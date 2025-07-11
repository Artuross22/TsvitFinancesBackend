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
public class ViewUser : Controller
{
    readonly protected MainDb _mainDb;

    public ViewUser(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> Invoke(string userId)
    {
        var user = await _mainDb.Set<AppUser>()
            .Include(u => u.BalanceFlows)
            .Where(u => u.Id == userId)
            .SingleOrDefaultAsync();

        if (user == null)
        {
            return NotFound();
        }

        var model = new BindingModel
        {
            Id = user!.Id,
            Email = user.Email,
            Nickname = user.Nickname,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            CreatedOn = user.CreatedOn,
        };

        if (user.BalanceFlows != null)
        {
            model.TotalBalance = user.BalanceFlows
                .Where(bf => bf.Balance != Balance.Outcome)
                .Where(bf => bf.Balance != Balance.NetInternalIncome)
                .Sum(bf => bf.Sum);

            model.GroupedBalanceFlows = user.BalanceFlows
               .GroupBy(bf => bf.Balance)
               .Select(g => new GroupedBalanceFlow
               {
                   BalanceType = g.Key.ToString(),
                   TotalSum = g.Sum(bf => bf.Sum),
                   BalanceFlows = g.Select(bf => new _BalanceFlow
                   {
                       Id = bf.Id,
                       Sum = bf.Sum,
                       BalanceType = bf.Balance.ToString(),
                       CreatedOn = bf.CreatedOn
                   }).ToList()
               }).ToList();
        }

        return Ok(model);
    }

    public class GroupedBalanceFlow
    {
        public required string BalanceType { get; set; }
        public required decimal TotalSum { get; set; }
        public virtual ICollection<_BalanceFlow>? BalanceFlows { get; set; }
    }

    public class BindingModel
    {
        public required string Id { get; set; }
        public string? Email { get; set; }
        public required string Nickname { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public required DateTime CreatedOn { get; set; }

        public decimal TotalBalance { get; set; }

        public virtual ICollection<GroupedBalanceFlow>? GroupedBalanceFlows { get; set; }
    }

    public class _BalanceFlow
    {
        public int Id { get; set; }
        public required decimal Sum { get; set; }
        public required string BalanceType { get; set; }
        public required DateTime CreatedOn { get; set; }
    }
}