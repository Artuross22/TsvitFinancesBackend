using Data;
using Microsoft.AspNetCore.Mvc;

namespace TsvitFinances.Controllers.InvestmentIdeas
{
    public class EditInvestmentIdea : Controller
    {
        protected readonly MainDb _mainDb;

        public EditInvestmentIdea(MainDb mainDb) 
        {
            _mainDb = mainDb;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
