using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekSitesiDbFirst.Models;

namespace YemekSitesiDbFirst.Controllers
{
    public class AdminRaporController : Controller
    {
        private readonly AppDbContext dbcontext;

        public AdminRaporController(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            ViewBag.ToplamKategori = dbcontext.Kategorilers.Count();
            ViewBag.ToplamYemek = dbcontext.Yemeklers.Count();
            ViewBag.AktifYemek = dbcontext.Yemeklers.Count(x => x.Durum == true);
            ViewBag.PasifYemek = dbcontext.Yemeklers.Count(x => x.Durum == false);
            ViewBag.ToplamMesaj = dbcontext.Mesajlars.Count();
            ViewBag.ToplamYorum = dbcontext.Yorumlars.Count();
            ViewBag.OnayliYorum = dbcontext.Yorumlars.Count(x => x.Durum == true);
            ViewBag.OnayBekleyenYorum = dbcontext.Yorumlars.Count(x => x.Durum == false);

            var kategoriRapor = dbcontext.Yemeklers
                .Include(x => x.Kategori)
                .GroupBy(x => x.Kategori.KategoriAd)
                .Select(x => new
                {
                    KategoriAd = x.Key,
                    YemekSayisi = x.Count()
                })
                .ToList();

            ViewBag.KategoriRapor = kategoriRapor;

            var sonYemekler = dbcontext.Yemeklers
                .OrderByDescending(x => x.YemekId)
                .Take(5)
                .ToList();

            ViewBag.SonYemekler = sonYemekler;

            return View();
        }
    }
}