using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekSitesiDbFirst.Models;

namespace YemekSitesiDbFirst.Controllers
{
    public class AdminYorumController : Controller
    {
        private readonly AppDbContext dbcontext;

        public AdminYorumController(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            var yorumlar = dbcontext.Yorumlars
                .Include(x => x.Yemek)
                .OrderByDescending(x => x.Tarih)
                .ToList();

            return View(yorumlar);
        }

        public IActionResult Onayla(int id)
        {
            var yorum = dbcontext.Yorumlars.Find(id);

            if (yorum != null)
            {
                yorum.Durum = true;
                dbcontext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult PasifYap(int id)
        {
            var yorum = dbcontext.Yorumlars.Find(id);

            if (yorum != null)
            {
                yorum.Durum = false;
                dbcontext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var yorum = dbcontext.Yorumlars
                .Include(x => x.Yemek)
                .FirstOrDefault(x => x.YorumId == id);

            if (yorum == null)
            {
                return NotFound();
            }

            return View(yorum);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var yorum = dbcontext.Yorumlars.Find(id);

            if (yorum != null)
            {
                dbcontext.Yorumlars.Remove(yorum);
                dbcontext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}