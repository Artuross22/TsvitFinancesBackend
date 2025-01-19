//using Data;
//using Data.Models;
//using Data.Models.Enums;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace TsvitFinances.Controllers.Diversifications;

//[AllowAnonymous]
//[Route("api/[controller]")]
//[ApiController]
//public class ListDiversification : Controller
//{
//    readonly protected MainDb _mainDb;

//    public ListDiversification(MainDb mainDb)
//    {
//        _mainDb = mainDb;
//    }

//    [HttpGet]
//    public async Task<ActionResult> Index(Guid publicId)
//    {
//        var diversifications = await _mainDb.Set<Diversification>()
//            .Where(a => a.PublicId == publicId)
//            .Select(a => new BindingModel
//            {
//                PublicId = a.PublicId,
//                NichePercentage = a.NichePercentage,
//                Sector = a.Sector
//            })
//            .ToListAsync();

//        if (diversifications == null)
//        {
//            return NotFound();
//        }

//        await _mainDb.SaveChangesAsync();

//        return Ok(diversifications);
//    }

//    private class BindingModel
//    {
//        public Guid PublicId { get; set; }

//        public decimal NichePercentage { get; set; }

//        public required Sector Sector { get; set; }
//    }
//}
