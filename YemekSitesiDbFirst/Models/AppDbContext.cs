using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace YemekSitesiDbFirst.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Adminler> Adminlers { get; set; }

    public virtual DbSet<Kategoriler> Kategorilers { get; set; }

    public virtual DbSet<Mesajlar> Mesajlars { get; set; }

    public virtual DbSet<Yemekler> Yemeklers { get; set; }

    public virtual DbSet<Yorumlar> Yorumlars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=Default");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adminler>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Adminler__719FE488C3B74654");

            entity.ToTable("Adminler");

            entity.Property(e => e.AdSoyad).HasMaxLength(100);
            entity.Property(e => e.KullaniciAdi).HasMaxLength(50);
            entity.Property(e => e.Sifre).HasMaxLength(50);
        });

        modelBuilder.Entity<Kategoriler>(entity =>
        {
            entity.HasKey(e => e.KategoriId).HasName("PK__Kategori__1782CC722DDEB4EC");

            entity.ToTable("Kategoriler");

            entity.Property(e => e.Aciklama).HasMaxLength(250);
            entity.Property(e => e.KategoriAd).HasMaxLength(100);
        });

        modelBuilder.Entity<Mesajlar>(entity =>
        {
            entity.HasKey(e => e.MesajId).HasName("PK__Mesajlar__0CB8DCA0B19D64A4");

            entity.ToTable("Mesajlar");

            entity.Property(e => e.AdSoyad).HasMaxLength(100);
            entity.Property(e => e.Konu).HasMaxLength(100);
            entity.Property(e => e.Mail).HasMaxLength(100);
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Yemekler>(entity =>
        {
            entity.HasKey(e => e.YemekId).HasName("PK__Yemekler__4A92D2B1117C1129");

            entity.ToTable("Yemekler");

            entity.Property(e => e.Durum).HasDefaultValue(true);
            entity.Property(e => e.HazirlamaSuresi).HasMaxLength(50);
            entity.Property(e => e.KisiSayisi).HasMaxLength(50);
            entity.Property(e => e.PismeSuresi).HasMaxLength(50);
            entity.Property(e => e.ResimUrl).HasMaxLength(250);
            entity.Property(e => e.YemekAd).HasMaxLength(100);

            entity.HasOne(d => d.Kategori).WithMany(p => p.Yemeklers)
                .HasForeignKey(d => d.KategoriId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Yemekler_Kategoriler");
        });

        modelBuilder.Entity<Yorumlar>(entity =>
        {
            entity.HasKey(e => e.YorumId).HasName("PK__Yorumlar__F2BE14E8DA85C8DD");

            entity.ToTable("Yorumlar");

            entity.Property(e => e.AdSoyad).HasMaxLength(100);
            entity.Property(e => e.Durum).HasDefaultValue(false);
            entity.Property(e => e.Mail).HasMaxLength(100);
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Yemek).WithMany(p => p.Yorumlars)
                .HasForeignKey(d => d.YemekId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Yorumlar_Yemekler");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
