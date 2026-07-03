using Microsoft.AspNetCore.Mvc;
using YemekSitesiDbFirst.Models;

namespace YemekSitesiDbFirst.Controllers
{
    public class AdminMesajController : Controller
    {
        private readonly AppDbContext dbcontext;

        public AdminMesajController(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            var mesajlar = dbcontext.Mesajlars
                .OrderByDescending(x => x.Tarih)
                .ToList();

            return View(mesajlar);
        }

        public IActionResult Details(int id)
        {
            var mesaj = dbcontext.Mesajlars.Find(id);

            if (mesaj == null)
            {
                return NotFound();
            }

            return View(mesaj);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var mesaj = dbcontext.Mesajlars.Find(id);

            if (mesaj == null)
            {
                return NotFound();
            }

            return View(mesaj);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var mesaj = dbcontext.Mesajlars.Find(id);

            if (mesaj != null)
            {
                dbcontext.Mesajlars.Remove(mesaj);
                dbcontext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}