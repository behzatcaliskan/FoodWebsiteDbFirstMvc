using System;
using System.Collections.Generic;

namespace YemekSitesiDbFirst.Models;

public partial class Yemekler
{
    public int YemekId { get; set; }

    public string YemekAd { get; set; } = null!;

    public string? Malzemeler { get; set; }

    public string? Tarif { get; set; }

    public string? ResimUrl { get; set; }

    public string? HazirlamaSuresi { get; set; }

    public string? PismeSuresi { get; set; }

    public string? KisiSayisi { get; set; }

    public bool? Durum { get; set; }

    public int KategoriId { get; set; }

    public virtual Kategoriler Kategori { get; set; } = null!;

    public virtual ICollection<Yorumlar> Yorumlars { get; set; } = new List<Yorumlar>();
}
