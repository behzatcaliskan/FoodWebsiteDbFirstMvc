using System;
using System.Collections.Generic;

namespace YemekSitesiDbFirst.Models;

public partial class Yorumlar
{
    public int YorumId { get; set; }

    public string AdSoyad { get; set; } = null!;

    public string? Mail { get; set; }

    public string? YorumIcerik { get; set; }

    public DateTime? Tarih { get; set; }

    public bool? Durum { get; set; }

    public int YemekId { get; set; }

    public virtual Yemekler Yemek { get; set; } = null!;
}
