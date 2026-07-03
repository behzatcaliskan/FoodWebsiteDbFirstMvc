using Microsoft.AspNetCore.Mvc;
using YemekSitesiDbFirst.Models;

namespace YemekSitesiDbFirst.Controllers
{
    public class AdminKategoriController : Controller
    {
        private readonly AppDbContext dbcontext;

        public AdminKategoriController(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            var kategoriler = dbcontext.Kategorilers.ToList();
            return View(kategoriler);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Kategoriler kategori)
        {
            dbcontext.Kategorilers.Add(kategori);
            dbcontext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var kategori = dbcontext.Kategorilers.Find(id);

            if (kategori == null)
            {
                return NotFound();
            }

            return View(kategori);
        }

        [HttpPost]
        public IActionResult Edit(Kategoriler kategori)
        {
            dbcontext.Kategorilers.Update(kategori);
            dbcontext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var kategori = dbcontext.Kategorilers.Find(id);

            if (kategori == null)
            {
                return NotFound();
            }

            return View(kategori);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var kategori = dbcontext.Kategorilers.Find(id);

            if (kategori != null)
            {
                dbcontext.Kategorilers.Remove(kategori);
                dbcontext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}