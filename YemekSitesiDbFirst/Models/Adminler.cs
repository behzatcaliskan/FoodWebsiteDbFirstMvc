using System;
using System.Collections.Generic;

namespace YemekSitesiDbFirst.Models;

public partial class Adminler
{
    public int AdminId { get; set; }

    public string KullaniciAdi { get; set; } = null!;

    public string Sifre { get; set; } = null!;

    public string? AdSoyad { get; set; }
}
