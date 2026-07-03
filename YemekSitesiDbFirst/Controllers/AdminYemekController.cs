using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Helpers;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using YemekSitesiDbFirst.Models;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace YemekSitesiDbFirst.Controllers
{
    public class AdminYemekController : Controller
    {
        private readonly AppDbContext dbcontext;

        public AdminYemekController(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            var yemekler = dbcontext.Yemeklers
                .Include(x => x.Kategori)
                .ToList();

            return View(yemekler);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(dbcontext.Kategorilers, "KategoriId", "KategoriAd");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Yemekler yemek)
        {
            dbcontext.Yemeklers.Add(yemek);
            dbcontext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var yemek = dbcontext.Yemeklers.Find(id);

            if (yemek == null)
            {
                return NotFound();
            }

            ViewBag.KategoriId = new SelectList(dbcontext.Kategorilers, "KategoriId", "KategoriAd", yemek.KategoriId);

            return View(yemek);
        }

        [HttpPost]
        public IActionResult Edit(Yemekler yemek)
        {
            dbcontext.Yemeklers.Update(yemek);
            dbcontext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var yemek = dbcontext.Yemeklers
                .Include(x => x.Kategori)
                .FirstOrDefault(x => x.YemekId == id);

            if (yemek == null)
            {
                return NotFound();
            }

            return View(yemek);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var yemek = dbcontext.Yemeklers.Find(id);

            if (yemek != null)
            {
                dbcontext.Yemeklers.Remove(yemek);
                dbcontext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    
    // PDF İndirme Butonunun Tetikleyeceği Metot
public IActionResult ExportToPdf()
        {
            // 1. Veri tabanından güncel yemek listesini çekiyoruz
            var yemekler = dbcontext.Yemeklers.ToList();

            // 2. QuestPDF ile PDF dökümanını tasarlıyoruz
            var pdfDocument = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    // Üst Bilgi
                    page.Header()
                        .Text("Yemek Listesi Raporu")
                        .SemiBold()
                        .FontSize(20)
                        .FontColor(Colors.Blue.Medium);

                    // İçerik
                    page.Content()
                        .PaddingTop(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            // Sütun genişlikleri
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);   // ID
                                columns.RelativeColumn();     // Yemek Adı
                                columns.RelativeColumn();     // Hazırlama Süresi
                                columns.RelativeColumn();     // Pişme Süresi
                                columns.RelativeColumn();     // Kişi Sayısı
                                columns.ConstantColumn(60);   // Durum
                            });

                            // Tablo Başlıkları
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("ID").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Yemek Adı").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Hazırlama").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Pişme").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Kişi").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Durum").Bold();
                            });

                            // Veri Satırları
                            foreach (var item in yemekler)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.YemekId.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.YemekAd ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.HazirlamaSuresi ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.PismeSuresi ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.KisiSayisi ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Durum == true ? "Aktif" : "Pasif");
                            }
                        });

                    // Alt Bilgi
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Sayfa ");
                            x.CurrentPageNumber();
                        });
                });
            });

            // 3. PDF'i byte dizisine çevirip tarayıcıya indirtme
            var pdfBytes = pdfDocument.GeneratePdf();

            return File(
                pdfBytes,
                "application/pdf",
                $"Yemek_Listesi_{DateTime.Now:yyyyMMdd}.pdf"
            );
        }

        // Excel İndirme Butonunun Tetikleyeceği Metot
        public IActionResult ExportToExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Backend softito");

            // 1. Veri tabanından güncel yemek listesini çekiyoruz
            var yemekler = dbcontext.Yemeklers.ToList();

            // 2. Bellekte boş bir Excel dosyası oluşturuyoruz
            using (var package = new ExcelPackage())
            {
                // Excel içinde "Yemek Listesi" adında bir sayfa açıyoruz
                var worksheet = package.Workbook.Worksheets.Add("Yemek Listesi");

                // 3. Tablo başlıklarını yazıyoruz
                worksheet.Cells[1, 1].Value = "Yemek ID";
                worksheet.Cells[1, 2].Value = "Yemek Adı";
                worksheet.Cells[1, 3].Value = "Malzemeler";
                worksheet.Cells[1, 4].Value = "Hazırlama Süresi";
                worksheet.Cells[1, 5].Value = "Pişme Süresi";
                worksheet.Cells[1, 6].Value = "Kişi Sayısı";
                worksheet.Cells[1, 7].Value = "Durum";

                // 4. Başlık satırını şekillendiriyoruz
                using (var range = worksheet.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(41, 128, 185));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // 5. Verileri döngü ile Excel satırlarına yazıyoruz
                int rowNumber = 2;

                foreach (var item in yemekler)
                {
                    worksheet.Cells[rowNumber, 1].Value = item.YemekId;
                    worksheet.Cells[rowNumber, 2].Value = item.YemekAd;
                    worksheet.Cells[rowNumber, 3].Value = item.Malzemeler;
                    worksheet.Cells[rowNumber, 4].Value = item.HazirlamaSuresi;
                    worksheet.Cells[rowNumber, 5].Value = item.PismeSuresi;
                    worksheet.Cells[rowNumber, 6].Value = item.KisiSayisi;
                    worksheet.Cells[rowNumber, 7].Value = item.Durum == true ? "Aktif" : "Pasif";

                    rowNumber++;
                }

                // 6. Sütun genişliklerini içeriğe göre otomatik ayarlıyoruz
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // 7. Excel dosyasını byte dizisine çevirip tarayıcıya indirtiyoruz
                var fileBytes = package.GetAsByteArray();

                string fileName = $"Yemek_Listesi_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(
                    fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
        }
    }
}