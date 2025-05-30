using Microsoft.AspNetCore.Mvc;
using WebPortfolio.Models.Data;
using WebPortfolio.Models.ViewModels;

namespace WebPortfolio.Controllers
{
    
    public class MenuController : Controller
    {
        Context context=new Context();
        public IActionResult Index()
        {
            var menus = context.Menu.ToList();
            var icons = context.Icons.ToList();
            ViewBag.Icons = icons;
            return View(menus);
        }
        public PartialViewResult Icons()
        {
            var icons = context.Icons.ToList();
            return PartialView("_Icons", icons);
        }


    }

}
