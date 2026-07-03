using System;
using System.Collections.Generic;

namespace YemekSitesiDbFirst.Models;

public partial class Kategoriler
{
    public int KategoriId { get; set; }

    public string KategoriAd { get; set; } = null!;

    public string? Aciklama { get; set; }

    public virtual ICollection<Yemekler> Yemeklers { get; set; } = new List<Yemekler>();
}
