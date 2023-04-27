using Microsoft.EntityFrameworkCore;

#nullable disable

namespace aesob.org.tr.Models
{
    public partial class AesobDbContext : DbContext
    {
        public AesobDbContext()
        {
        }

        public AesobDbContext(DbContextOptions<AesobDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Anlasma> Anlasmas { get; set; }
        public virtual DbSet<Ayarlar> Ayarlars { get; set; }
        public virtual DbSet<BaskaninMesaji> BaskaninMesajis { get; set; }
        public virtual DbSet<Dergi> Dergis { get; set; }
        public virtual DbSet<Duyurular> Duyurulars { get; set; }
        public virtual DbSet<Epostalistesi> Epostalistesis { get; set; }
        public virtual DbSet<Firmabilgileri> Firmabilgileris { get; set; }
        public virtual DbSet<Gbilgiedinme> Gbilgiedinmes { get; set; }
        public virtual DbSet<Genelgeler> Genelgelers { get; set; }
        public virtual DbSet<Haberler> Haberlers { get; set; }
        public virtual DbSet<Baskanlar> Baskanlars { get; set; }
        public virtual DbSet<Icerikler> Iceriklers { get; set; }
        public virtual DbSet<Kullanicilar> Kullanicilars { get; set; }
        public virtual DbSet<KurulUyeleri> KurulUyeleris { get; set; }
        public virtual DbSet<Menuler> Menulers { get; set; }
        public virtual DbSet<Odalar> Odalars { get; set; }
        public virtual DbSet<Sozler> Sozlers { get; set; }
        public virtual DbSet<Tbilgiedinme> Tbilgiedinmes { get; set; }
        public virtual DbSet<Youtubevideo> Youtubevideos { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#if DEBUG
                optionsBuilder.UseSqlServer("AesobDebugSiteConnection");
#else
                optionsBuilder.UseSqlServer("AesobReleaseSiteConnection");
#endif
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Turkish_CI_AS");

            modelBuilder.Entity<Anlasma>(entity =>
            {
                entity.ToTable("anlasma");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Bastarihi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("bastarihi");

                entity.Property(e => e.Bittarihi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("bittarihi");

                entity.Property(e => e.Durum).HasColumnName("durum");

                entity.Property(e => e.Eklemetarihi)
                    .HasColumnType("datetime")
                    .HasColumnName("eklemetarihi");

                entity.Property(e => e.Icerik)
                    .HasMaxLength(3000)
                    .HasColumnName("icerik");

                entity.Property(e => e.Taraf)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("taraf");

                entity.Property(e => e.Tarafadres)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("tarafadres");

                entity.Property(e => e.Taraftelefon)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("taraftelefon");
            });

            modelBuilder.Entity<Ayarlar>(entity =>
            {
                entity.ToTable("AYARLAR");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Dovizkurlari).HasColumnName("DOVIZKURLARI");

                entity.Property(e => e.Firmano).HasColumnName("FIRMANO");

                entity.Property(e => e.FtpAdres)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FtpKullanici)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FtpPassword).HasMaxLength(50);

                entity.Property(e => e.GoogleAnalytics)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Haberebatbuyuk).HasColumnName("HABEREBATBUYUK");

                entity.Property(e => e.Haberebatkucuk).HasColumnName("HABEREBATKUCUK");

                entity.Property(e => e.Haritanokta1)
                    .HasMaxLength(50)
                    .HasColumnName("HARITANOKTA1");

                entity.Property(e => e.Haritanokta2)
                    .HasMaxLength(50)
                    .HasColumnName("HARITANOKTA2");

                entity.Property(e => e.Haritazoom).HasColumnName("HARITAZOOM");

                entity.Property(e => e.Havadurumu).HasColumnName("HAVADURUMU");

                entity.Property(e => e.Havadurumuborder)
                    .HasMaxLength(6)
                    .HasColumnName("HAVADURUMUBORDER");

                entity.Property(e => e.Havadurumuil)
                    .HasMaxLength(50)
                    .HasColumnName("HAVADURUMUIL");

                entity.Property(e => e.Havadurumuzemin)
                    .HasMaxLength(6)
                    .HasColumnName("HAVADURUMUZEMIN");

                entity.Property(e => e.Iletisimformu).HasColumnName("ILETISIMFORMU");

                entity.Property(e => e.Kroki)
                    .HasMaxLength(1000)
                    .HasColumnName("KROKI");

                entity.Property(e => e.Slaytebat).HasColumnName("SLAYTEBAT");

                entity.Property(e => e.Urunebatbuyuk).HasColumnName("URUNEBATBUYUK");

                entity.Property(e => e.Urunebatkucuk).HasColumnName("URUNEBATKUCUK");
            });

            modelBuilder.Entity<BaskaninMesaji>(entity =>
            {
                entity.ToTable("BaskaninMesaji");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Baslik)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BASLIK");

                entity.Property(e => e.Description)
                    .HasMaxLength(180)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Icerik)
                    .HasColumnType("text")
                    .HasColumnName("ICERIK");

                entity.Property(e => e.Keywords)
                    .HasMaxLength(180)
                    .IsUnicode(false)
                    .HasColumnName("KEYWORDS");

                entity.Property(e => e.Okunmasayisi)
                    .HasColumnName("OKUNMASAYISI")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Seolink)
                    .HasMaxLength(110)
                    .IsUnicode(false)
                    .HasColumnName("SEOLINK");

                entity.Property(e => e.Tarih)
                    .HasColumnType("datetime")
                    .HasColumnName("TARIH");
            });

            modelBuilder.Entity<Dergi>(entity =>
            {
                entity.ToTable("Dergi");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Dergisayisi)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("dergisayisi");

                entity.Property(e => e.Dergiyil).HasColumnName("dergiyil");

                entity.Property(e => e.Dosyayolu)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("dosyayolu");

                entity.Property(e => e.Kapakresimyolu)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("kapakresimyolu");
            });

            modelBuilder.Entity<Duyurular>(entity =>
            {
                entity.ToTable("Duyurular");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Baslik)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("baslik");

                entity.Property(e => e.Durum).HasColumnName("durum");

                entity.Property(e => e.Eklemetarihi)
                    .HasColumnType("datetime")
                    .HasColumnName("eklemetarihi");

                entity.Property(e => e.Icerik).HasColumnName("icerik");
            });

            modelBuilder.Entity<Epostalistesi>(entity =>
            {
                entity.ToTable("EPOSTALISTESI");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Adsoyad)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ADSOYAD");

                entity.Property(e => e.Durum).HasColumnName("DURUM");

                entity.Property(e => e.Eposta)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EPOSTA");

                entity.Property(e => e.Tarih)
                    .HasColumnType("datetime")
                    .HasColumnName("TARIH");
            });

            modelBuilder.Entity<Firmabilgileri>(entity =>
            {
                entity.ToTable("FIRMABILGILERI");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Adres)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("ADRES");

                entity.Property(e => e.Eposta)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EPOSTA");

                entity.Property(e => e.Epostahesapadi)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EPOSTAHESAPADI");

                entity.Property(e => e.Epostahesapsifresi)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EPOSTAHESAPSIFRESI");

                entity.Property(e => e.Epostaport).HasColumnName("EPOSTAPORT");

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FAX");

                entity.Property(e => e.Firmaadi)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("FIRMAADI");

                entity.Property(e => e.Sonkullanmatarihi)
                    .HasColumnType("datetime")
                    .HasColumnName("SONKULLANMATARIHI");

                entity.Property(e => e.Tel)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TEL");

                entity.Property(e => e.Web)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("WEB");
            });

            modelBuilder.Entity<Gbilgiedinme>(entity =>
            {
                entity.HasKey(e => e.Gid);

                entity.ToTable("gbilgiedinme");

                entity.Property(e => e.Gid).HasColumnName("gid");

                entity.Property(e => e.Adres)
                    .HasMaxLength(250)
                    .HasColumnName("adres");

                entity.Property(e => e.Adsoyad)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("adsoyad");

                entity.Property(e => e.Cevap).HasColumnName("cevap");

                entity.Property(e => e.Eposta)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("eposta");

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .HasColumnName("fax");

                entity.Property(e => e.Istek)
                    .HasMaxLength(1000)
                    .HasColumnName("istek");

                entity.Property(e => e.Tarih)
                    .HasColumnType("datetime")
                    .HasColumnName("tarih");

                entity.Property(e => e.Tckimlik)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("tckimlik")
                    .IsFixedLength(true);

                entity.Property(e => e.Telefon)
                    .HasMaxLength(50)
                    .HasColumnName("telefon");

                entity.Property(e => e.Yanitkanali)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("yanitkanali");
            });

            modelBuilder.Entity<Genelgeler>(entity =>
            {
                entity.ToTable("genelgeler");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Eklemetarihi)
                    .HasColumnType("datetime")
                    .HasColumnName("eklemetarihi");

                entity.Property(e => e.Konu)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("konu");

                entity.Property(e => e.Sayi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sayi");

                entity.Property(e => e.Tarih)
                    .HasColumnType("datetime")
                    .HasColumnName("tarih");

                entity.Property(e => e.Yil)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("yil");

                entity.Property(e => e.Yolu)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("yolu");
            });

            modelBuilder.Entity<Haberler>(entity =>
            {
                entity.ToTable("HABERLER");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Baslik)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BASLIK");

                entity.Property(e => e.Description)
                    .HasMaxLength(180)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Icerik)
                    .HasColumnType("text")
                    .HasColumnName("ICERIK");

                entity.Property(e => e.Keywords)
                    .HasMaxLength(180)
                    .IsUnicode(false)
                    .HasColumnName("KEYWORDS");

                entity.Property(e => e.Okunmasayisi)
                    .HasColumnName("OKUNMASAYISI")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Resimyolu)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("RESIMYOLU");

                entity.Property(e => e.Seolink)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SEOLINK");

                entity.Property(e => e.Tarih)
                    .HasMaxLength(20)
                    .HasColumnName("TARIH");

                entity.Property(e => e.Tarih2)
                    .HasColumnType("datetime")
                    .HasColumnName("TARIH2");
            });

            modelBuilder.Entity<Baskanlar>(entity =>
            {
                entity.ToTable("Baskanlar");

                entity.Property(e => e.ID)
                .HasColumnName("ID");

                entity.Property(e => e.Isim)
                .HasMaxLength(140)
                .IsUnicode(false)
                .HasColumnName("Isim");

                entity.Property(e => e.Twitter)
                .HasMaxLength(140)
                .IsUnicode(false)
                .HasColumnName("Twitter");

                entity.Property(e => e.Facebook)
                .HasMaxLength(140)
                .IsUnicode(false)
                .HasColumnName("Facebook");

                entity.Property(e => e.Instagram)
                .HasMaxLength(140)
                .IsUnicode(false)
                .HasColumnName("Instagram");

                entity.Property(e => e.Mesaj)
                .IsUnicode(false)
                .HasColumnName("Mesaj");

                entity.Property(e => e.Aktif)
                .HasColumnName("Aktif");
            });

            modelBuilder.Entity<Icerikler>(entity =>
            {
                entity.ToTable("ICERIKLER");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Baslik)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BASLIK");

                entity.Property(e => e.Description)
                    .HasMaxLength(180)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Icerik)
                    .HasColumnType("text")
                    .HasColumnName("ICERIK");

                entity.Property(e => e.Keywords)
                    .HasMaxLength(180)
                    .IsUnicode(false)
                    .HasColumnName("KEYWORDS");

                entity.Property(e => e.Okunmasayisi).HasColumnName("OKUNMASAYISI");

                entity.Property(e => e.Seolink)
                    .HasMaxLength(110)
                    .IsUnicode(false)
                    .HasColumnName("SEOLINK");
            });

            modelBuilder.Entity<KurulUyeleri>(entity =>
            {
                entity.ToTable("KurulUyeleri");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.Isim)
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasColumnName("Isim");

                entity.Property(e => e.Mevki)
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasColumnName("Mevki");

                entity.Property(e => e.Derece).HasColumnName("Derece");

                entity.Property(e => e.DereceAciklama)
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasColumnName("DereceAciklama");

                entity.Property(e => e.Resim)
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasColumnName("Resim");

                entity.Property(e => e.Kurul).HasColumnName("Kurul");
            });

            modelBuilder.Entity<Kullanicilar>(entity =>
            {
                entity.ToTable("KULLANICILAR");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Durum).HasColumnName("DURUM");

                entity.Property(e => e.Kullaniciadi)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("KULLANICIADI");

                entity.Property(e => e.Rol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ROL");

                entity.Property(e => e.Sifre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SIFRE");
            });

            modelBuilder.Entity<Menuler>(entity =>
            {
                entity.ToTable("menuler");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Menuadi)
                    .HasMaxLength(50)
                    .HasColumnName("menuadi");

                entity.Property(e => e.Parentid).HasColumnName("parentid");

                entity.Property(e => e.Url)
                    .HasMaxLength(100)
                    .HasColumnName("url");
            });

            modelBuilder.Entity<Odalar>(entity =>
            {
                entity.ToTable("Odalar");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Adres).HasMaxLength(100);

                entity.Property(e => e.Baskan).HasMaxLength(50);

                entity.Property(e => e.BaskanResim)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DenetimKurulu)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.GenelSekreter).HasMaxLength(50);

                entity.Property(e => e.Kroki).HasMaxLength(300);

                entity.Property(e => e.Merkezilce).HasColumnName("merkezilce");

                entity.Property(e => e.OdaAdi).HasMaxLength(100);

                entity.Property(e => e.Tel).HasMaxLength(50);

                entity.Property(e => e.Web).HasMaxLength(50);

                entity.Property(e => e.YonetimKurulu)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sozler>(entity =>
            {
                entity.ToTable("Sozler");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Soz)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Yazar)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tbilgiedinme>(entity =>
            {
                entity.HasKey(e => e.Tid);

                entity.ToTable("tbilgiedinme");

                entity.Property(e => e.Tid).HasColumnName("tid");

                entity.Property(e => e.Adres)
                    .HasMaxLength(250)
                    .HasColumnName("adres");

                entity.Property(e => e.Adsoyad)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("adsoyad");

                entity.Property(e => e.Cevap).HasColumnName("cevap");

                entity.Property(e => e.Eposta)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("eposta");

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .HasColumnName("fax");

                entity.Property(e => e.Istek)
                    .HasMaxLength(1000)
                    .HasColumnName("istek");

                entity.Property(e => e.Tarih)
                    .HasColumnType("datetime")
                    .HasColumnName("tarih");

                entity.Property(e => e.Tckimlik)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("tckimlik")
                    .IsFixedLength(true);

                entity.Property(e => e.Telefon)
                    .HasMaxLength(50)
                    .HasColumnName("telefon");

                entity.Property(e => e.Unvan)
                    .HasMaxLength(100)
                    .HasColumnName("unvan");

                entity.Property(e => e.Yanitkanali)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("yanitkanali");
            });

            modelBuilder.Entity<Youtubevideo>(entity =>
            {
                entity.HasKey(e => e.VideoId);

                entity.ToTable("YOUTUBEVIDEO");

                entity.Property(e => e.Description)
                    .HasMaxLength(180)
                    .IsUnicode(false);

                entity.Property(e => e.Kategori)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Keywords)
                    .HasMaxLength(180)
                    .IsUnicode(false);

                entity.Property(e => e.ProgramAciklama)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ProgramAdi)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tarih).HasColumnType("datetime");

                entity.Property(e => e.YoutubeThumbSize)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.YoutubeVideoNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("User");

                entity.Property(e => e.Username)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
