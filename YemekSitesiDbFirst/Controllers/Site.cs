using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekSitesiDbFirst.Models;

namespace YemekSitesiDbFirst.Controllers
{
    public class SiteController : Controller
    {
        private readonly AppDbContext dbcontext;

        public SiteController(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            var yemekler = dbcontext.Yemeklers
                .Include(x => x.Kategori)
                .Where(x => x.Durum == true)
                .ToList();

            return View(yemekler);
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(Mesajlar mesaj)
        {
            mesaj.Tarih = DateTime.Now;

            dbcontext.Mesajlars.Add(mesaj);
            dbcontext.SaveChanges();

            return RedirectToAction("Contact");
        }

        [HttpPost]
        public IActionResult MesajGonder(Mesajlar mesaj)
        {
            mesaj.Tarih = DateTime.Now;

            dbcontext.Mesajlars.Add(mesaj);
            dbcontext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}