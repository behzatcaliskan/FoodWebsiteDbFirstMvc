using Microsoft.AspNetCore.Mvc;
using YemekSitesiDbFirst.Models;

namespace YemekSitesiDbFirst.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext dbcontext;

        public AdminController(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string kullaniciAdi, string sifre)
        {
            var admin = dbcontext.Adminlers
                .FirstOrDefault(x => x.KullaniciAdi == kullaniciAdi && x.Sifre == sifre);

            if (admin != null)
            {
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.Hata = "Kullanıcı adı veya şifre hatalı";
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}