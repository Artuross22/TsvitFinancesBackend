//using Data;
//using Data.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace TsvitFinances.Controllers.UserManagement;

//public class ViewBalance : Controller
//{
//    readonly protected MainDb _mainDb;

//    public ViewBalance(MainDb mainDb)
//    {
//        _mainDb = mainDb;
//    }

//    [HttpGet("{userId}")]
//    public async Task<IActionResult> Index(string userId)
//    {
//        var user = await _mainDb.Set<AppUser>()
//            .Include(u => u.BalanceFlows)
//            .Where(u => u.Id == userId)
//            .SingleOrDefaultAsync();

//        if (user == null)
//        {
//            return NotFound();
//        }

//        var model = new BindingModel
//        {
//            Id = user!.Id,
//            TotalBalance = user.BalanceFlows.Sum(bf => bf.Sum),
//            BalanceFlows = user.BalanceFlows.Select(bf => new _BalanceFlow
//            {
//                Id = bf.Id,
//                Sum = bf.Sum,
//                BalanceType = bf.Balance.ToString(),
//                CreatedOn = bf.CreatedOn
//            })
//            .ToList()
//        };
//        return Ok(model);
//    }

//    public class BindingModel
//    {
//        public string Id { get; set; }
//        public decimal TotalBalance { get; set; }
//        public List<_BalanceFlow> BalanceFlows { get; set; }
//        public class _BalanceFlow
//        {
//            public string Id { get; set; }
//            public decimal Sum { get; set; }
//            public string BalanceType { get; set; }
//            public DateTime CreatedOn { get; set; }
//        }
//    }
//}
