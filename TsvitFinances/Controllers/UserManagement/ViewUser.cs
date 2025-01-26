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
    public async Task<IActionResult> Index(string userId)
    {
        var user = await _mainDb.Set<AppUser>()
            .Include(u => u.BalanceFlows)
            .Where(u => u.Id == userId)
            .Select(u => new BindingModel
            {
                Id = u.Id,
                Email = u.Email!,
                Nickname = u.Nickname,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                CreatedOn = u.CreatedOn,
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound();
        }

        if (user.BalanceFlows != null)
        {
            user.TotalBalance = user.BalanceFlows.Sum(bf => bf.Sum);
            user.BalanceFlows = user.BalanceFlows.Select(bf => new _BalanceFlow
            {
                Id = bf.Id,
                Sum = bf.Sum,
                BalanceType = bf.BalanceType,
                CreatedOn = bf.CreatedOn
            })
            .ToList();
        }

        return Ok(user);
    }

    public class BindingModel
    {
        public string Id { get; set; }
        public required string Email { get; set; }
        public required string Nickname { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public required DateTime CreatedOn { get; set; }

        public decimal TotalBalance { get; set; }

        public virtual ICollection<_BalanceFlow>? BalanceFlows { get; set; }
    }

    public class _BalanceFlow
    {
        public int Id { get; set; }
        public required decimal Sum { get; set; }
        public required Balance BalanceType { get; set; }
        public required DateTime CreatedOn { get; set; }
    }
}