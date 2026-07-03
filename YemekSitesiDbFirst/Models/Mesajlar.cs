using System;
using System.Collections.Generic;

namespace YemekSitesiDbFirst.Models;

public partial class Mesajlar
{
    public int MesajId { get; set; }

    public string AdSoyad { get; set; } = null!;

    public string? Mail { get; set; }

    public string? Konu { get; set; }

    public string? MesajIcerik { get; set; }

    public DateTime? Tarih { get; set; }
}
