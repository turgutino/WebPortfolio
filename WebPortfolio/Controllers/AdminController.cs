using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using WebPortfolio.Models.Data;
using WebPortfolio.Models.Entities;

namespace WebPortfolio.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [Authorize]
    public class AdminController : Controller
    {
        Context context2 = new Context();

        [Authorize]
        public ActionResult Index()
        {
            var data = context2.Menu.ToList();
            return View(data);
        }

        public ActionResult ComeMenu(int id)
        {
            var current = context2.Menu.Find(id);
            return View("ComeMenu", current);
        }

        [HttpPost]
        public ActionResult MenuUpdate(Menu menu, IFormFile ProfilPhotoFile)
        {
            var a = context2.Menu.Find(menu.Id);

            if (ProfilPhotoFile != null && ProfilPhotoFile.Length > 0)
            {
                var fileName = Path.GetFileName(ProfilPhotoFile.FileName);

                
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ProfilPhotoFile.CopyTo(stream);
                }

               
                a.ProfilPhoto = "/uploads/" + fileName;
            }

            a.Name = menu.Name;
            a.Description = menu.Description;
            a.Position = menu.Position;
            a.Contact = menu.Contact;

            context2.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult IconList()
        {
            var datas = context2.Icons.ToList();
            return View(datas);
        }
        [HttpGet]
        public ActionResult AddIcon()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddIcon(Icon icon)
        {
            context2.Icons.Add(icon);
            context2.SaveChanges();
            return RedirectToAction("IconList");

        }

        public ActionResult ComeIcon(int id)
        {
            var data3 = context2.Icons.Find(id);
            return View("ComeIcon", data3);
        }

        public ActionResult UpdateIcon(Icon icon2)
        {
            var datas3 = context2.Icons.Find(icon2.Id);
            datas3.Name = icon2.Name;
            datas3.Link = icon2.Link;
            context2.SaveChanges();
            return RedirectToAction("IconList");
        }

        public ActionResult DeleteIcon(int id)
        {
            var datas5 = context2.Icons.Find(id);
            context2.Icons.Remove(datas5);
            context2.SaveChanges();
            return RedirectToAction("IconList");
        }
    }
}
