using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aesob.org.tr.Models
{
	public class AesobDbContext : DbContext
	{
		public virtual DbSet<Anlasma> Anlasmas { get; set; }

		public virtual DbSet<Ayarlar> Ayarlars { get; set; }

		public virtual DbSet<BaskaninMesaji> BaskaninMesajis { get; set; }

		public virtual DbSet<Dergi> Dergis { get; set; }

		public virtual DbSet<Duyurular> Duyurulars { get; set; }

		public virtual DbSet<EBulten> EBultens { get; set; }

		public virtual DbSet<Phone> Phones { get; set; }

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

		public virtual DbSet<Popup> Popups { get; set; }

		public virtual DbSet<Sozler> Sozlers { get; set; }

		public virtual DbSet<Tbilgiedinme> Tbilgiedinmes { get; set; }

		public virtual DbSet<Youtubevideo> Youtubevideos { get; set; }

		public virtual DbSet<User> Users { get; set; }

		public AesobDbContext()
		{
		}

		public AesobDbContext(DbContextOptions<AesobDbContext> options)
			: base(options)
		{
		}

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
			modelBuilder.Entity(delegate(EntityTypeBuilder<Anlasma> entity)
			{
				entity.ToTable("anlasma");
				entity.Property((Anlasma e) => e.Id).HasColumnName("id");
				entity.Property((Anlasma e) => e.Bastarihi).HasMaxLength(10).IsUnicode(false)
					.HasColumnName("bastarihi");
				entity.Property((Anlasma e) => e.Bittarihi).HasMaxLength(10).IsUnicode(false)
					.HasColumnName("bittarihi");
				entity.Property((Anlasma e) => e.Durum).HasColumnName("durum");
				entity.Property((Anlasma e) => e.Eklemetarihi).HasColumnType("datetime").HasColumnName("eklemetarihi");
				entity.Property((Anlasma e) => e.Icerik).HasColumnName("icerik");
				entity.Property((Anlasma e) => e.Taraf).HasMaxLength(250).IsUnicode(false)
					.HasColumnName("taraf");
				entity.Property((Anlasma e) => e.Tarafadres).HasMaxLength(500).IsUnicode(false)
					.HasColumnName("tarafadres");
				entity.Property((Anlasma e) => e.Taraftelefon).HasMaxLength(50).IsUnicode(false)
					.HasColumnName("taraftelefon");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Ayarlar> entity)
			{
				entity.ToTable("AYARLAR");
				entity.Property((Ayarlar e) => e.Id).HasColumnName("ID");
				entity.Property((Ayarlar e) => e.Dovizkurlari).HasColumnName("DOVIZKURLARI");
				entity.Property((Ayarlar e) => e.Firmano).HasColumnName("FIRMANO");
				entity.Property((Ayarlar e) => e.FtpAdres).HasMaxLength(100).IsUnicode(false);
				entity.Property((Ayarlar e) => e.FtpKullanici).HasMaxLength(50).IsUnicode(false);
				entity.Property((Ayarlar e) => e.FtpPassword).HasMaxLength(50);
				entity.Property((Ayarlar e) => e.GoogleAnalytics).HasMaxLength(1000).IsUnicode(false);
				entity.Property((Ayarlar e) => e.Haberebatbuyuk).HasColumnName("HABEREBATBUYUK");
				entity.Property((Ayarlar e) => e.Haberebatkucuk).HasColumnName("HABEREBATKUCUK");
				entity.Property((Ayarlar e) => e.Haritanokta1).HasMaxLength(50).HasColumnName("HARITANOKTA1");
				entity.Property((Ayarlar e) => e.Haritanokta2).HasMaxLength(50).HasColumnName("HARITANOKTA2");
				entity.Property((Ayarlar e) => e.Haritazoom).HasColumnName("HARITAZOOM");
				entity.Property((Ayarlar e) => e.Havadurumu).HasColumnName("HAVADURUMU");
				entity.Property((Ayarlar e) => e.Havadurumuborder).HasMaxLength(6).HasColumnName("HAVADURUMUBORDER");
				entity.Property((Ayarlar e) => e.Havadurumuil).HasMaxLength(50).HasColumnName("HAVADURUMUIL");
				entity.Property((Ayarlar e) => e.Havadurumuzemin).HasMaxLength(6).HasColumnName("HAVADURUMUZEMIN");
				entity.Property((Ayarlar e) => e.Iletisimformu).HasColumnName("ILETISIMFORMU");
				entity.Property((Ayarlar e) => e.Kroki).HasMaxLength(1000).HasColumnName("KROKI");
				entity.Property((Ayarlar e) => e.Slaytebat).HasColumnName("SLAYTEBAT");
				entity.Property((Ayarlar e) => e.Urunebatbuyuk).HasColumnName("URUNEBATBUYUK");
				entity.Property((Ayarlar e) => e.Urunebatkucuk).HasColumnName("URUNEBATKUCUK");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<BaskaninMesaji> entity)
			{
				entity.ToTable("BaskaninMesaji");
				entity.Property((BaskaninMesaji e) => e.Id).HasColumnName("ID");
				entity.Property((BaskaninMesaji e) => e.Baslik).HasMaxLength(100).IsUnicode(false)
					.HasColumnName("BASLIK");
				entity.Property((BaskaninMesaji e) => e.Description).HasMaxLength(180).IsUnicode(false)
					.HasColumnName("DESCRIPTION");
				entity.Property((BaskaninMesaji e) => e.Icerik).HasColumnType("text").HasColumnName("ICERIK");
				entity.Property((BaskaninMesaji e) => e.Keywords).HasMaxLength(180).IsUnicode(false)
					.HasColumnName("KEYWORDS");
				entity.Property((BaskaninMesaji e) => e.Okunmasayisi).HasColumnName("OKUNMASAYISI").HasDefaultValueSql("((1))");
				entity.Property((BaskaninMesaji e) => e.Seolink).HasMaxLength(110).IsUnicode(false)
					.HasColumnName("SEOLINK");
				entity.Property((BaskaninMesaji e) => e.Tarih).HasColumnType("datetime").HasColumnName("TARIH");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Dergi> entity)
			{
				entity.ToTable("Dergi");
				entity.Property((Dergi e) => e.Id).HasColumnName("id");
				entity.Property((Dergi e) => e.Dergisayisi).HasMaxLength(50).IsUnicode(false)
					.HasColumnName("dergisayisi");
				entity.Property((Dergi e) => e.Dergiyil).HasColumnName("dergiyil");
				entity.Property((Dergi e) => e.Dosyayolu).HasMaxLength(150).IsUnicode(false)
					.HasColumnName("dosyayolu");
				entity.Property((Dergi e) => e.Kapakresimyolu).HasMaxLength(50).IsUnicode(false)
					.HasColumnName("kapakresimyolu");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Duyurular> entity)
			{
				entity.ToTable("Duyurular");
				entity.Property((Duyurular e) => e.Id).HasColumnName("id");
				entity.Property((Duyurular e) => e.Baslik).HasMaxLength(100).IsUnicode(false)
					.HasColumnName("baslik");
				entity.Property((Duyurular e) => e.Durum).HasColumnName("durum");
				entity.Property((Duyurular e) => e.Eklemetarihi).HasColumnType("datetime").HasColumnName("eklemetarihi");
				entity.Property((Duyurular e) => e.Icerik).HasColumnName("icerik");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<EBulten> entity)
			{
				entity.ToTable("EBulten");
				entity.Property((EBulten e) => e.Id).HasColumnName("id");
				entity.Property((EBulten e) => e.Name).HasMaxLength(128).HasColumnName("Name");
				entity.Property((EBulten e) => e.EMail).HasMaxLength(128).HasColumnName("EMail");
				entity.Property((EBulten e) => e.Phone).HasMaxLength(13).HasColumnName("Phone");
				entity.Property((EBulten e) => e.Union).HasMaxLength(512).HasColumnName("RegisteredUnion");
				entity.Property((EBulten e) => e.Group).HasMaxLength(32).HasColumnName("RegisteredGroup");
				entity.Property((EBulten e) => e.IsEmailActive).HasColumnType("bit").HasColumnName("IsEmailActive");
				entity.Property((EBulten e) => e.IsPhoneActive).HasColumnType("bit").HasColumnName("IsPhoneActive");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Phone> entity)
			{
				entity.ToTable("Phone");
				entity.Property((Phone e) => e.Id).HasColumnName("Id");
				entity.Property((Phone e) => e.Name).HasMaxLength(100).HasColumnName("Name");
				entity.Property((Phone e) => e.PhoneNumber).HasMaxLength(21).HasColumnName("Phone");
				entity.Property((Phone e) => e.Union).HasMaxLength(256).HasColumnName("Union");
				entity.Property((Phone e) => e.UnionPhone).HasMaxLength(16).HasColumnName("UnionPhone");
				entity.Property((Phone e) => e.UnionFax).HasMaxLength(16).HasColumnName("UnionFax");
				entity.Property((Phone e) => e.IsActive).HasColumnType("bit").HasColumnName("IsActive");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Firmabilgileri> entity)
			{
				entity.ToTable("FIRMABILGILERI");
				entity.Property((Firmabilgileri e) => e.Id).HasColumnName("ID");
				entity.Property((Firmabilgileri e) => e.Adres).HasMaxLength(250).IsUnicode(false)
					.HasColumnName("ADRES");
				entity.Property((Firmabilgileri e) => e.Eposta).HasMaxLength(100).IsUnicode(false)
					.HasColumnName("EPOSTA");
				entity.Property((Firmabilgileri e) => e.Epostahesapadi).HasMaxLength(100).IsUnicode(false)
					.HasColumnName("EPOSTAHESAPADI");
				entity.Property((Firmabilgileri e) => e.Epostahesapsifresi).HasMaxLength(50).IsUnicode(false)
					.HasColumnName("EPOSTAHESAPSIFRESI");
				entity.Property((Firmabilgileri e) => e.Epostaport).HasColumnName("EPOSTAPORT");
				entity.Property((Firmabilgileri e) => e.Fax).HasMaxLength(50).IsUnicode(false)
					.HasColumnName("FAX");
				entity.Property((Firmabilgileri e) => e.Firmaadi).HasMaxLength(250).IsUnicode(false)
					.HasColumnName("FIRMAADI");
				entity.Property((Firmabilgileri e) => e.Sonkullanmatarihi).HasColumnType("datetime").HasColumnName("SONKULLANMATARIHI");
				entity.Property((Firmabilgileri e) => e.Tel).HasMaxLength(50).IsUnicode(false)
					.HasColumnName("TEL");
				entity.Property((Firmabilgileri e) => e.Web).HasMaxLength(100).IsUnicode(false)
					.HasColumnName("WEB");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Gbilgiedinme> entity)
			{
				entity.HasKey((Gbilgiedinme e) => e.Gid);
				entity.ToTable("gbilgiedinme");
				entity.Property((Gbilgiedinme e) => e.Gid).HasColumnName("gid");
				entity.Property((Gbilgiedinme e) => e.Adres).HasMaxLength(250).HasColumnName("adres");
				entity.Property((Gbilgiedinme e) => e.Adsoyad).HasMaxLength(50).IsUnicode(false)
					.HasColumnName("adsoyad");
				entity.Property((Gbilgiedinme e) => e.Cevap).HasColumnName("cevap");
				entity.Property((Gbilgiedinme e) => e.Eposta).HasMaxLength(50).IsUnicode(false)
					.HasColumnName("eposta");
				entity.Property((Gbilgiedinme e) => e.Fax).HasMaxLength(50).HasColumnName("fax");
				entity.Property((Gbilgiedinme e) => e.Istek).HasMaxLength(1000).HasColumnName("istek");
				entity.Property((Gbilgiedinme e) => e.Tarih).HasColumnType("datetime").HasColumnName("tarih");
				entity.Property((Gbilgiedinme e) => e.Tckimlik).HasMaxLength(11).IsUnicode(false)
					.HasColumnName("tckimlik")
					.IsFixedLength();
				entity.Property((Gbilgiedinme e) => e.Telefon).HasMaxLength(50).HasColumnName("telefon");
				entity.Property((Gbilgiedinme e) => e.Yanitkanali).HasMaxLength(20).IsUnicode(false)
					.HasColumnName("yanitkanali");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Genelgeler> entity)
			{
				entity.ToTable("genelgeler");
				entity.Property((Genelgeler e) => e.Id).HasColumnName("id");
				entity.Property((Genelgeler e) => e.Eklemetarihi).HasColumnType("datetime").HasColumnName("eklemetarihi");
				entity.Property((Genelgeler e) => e.Konu).HasMaxLength(512).IsUnicode(false)
					.HasColumnName("konu");
				entity.Property((Genelgeler e) => e.Sayi).HasMaxLength(10).IsUnicode(false)
					.HasColumnName("sayi");
				entity.Property((Genelgeler e) => e.Tarih).HasColumnType("datetime").HasColumnName("tarih");
				entity.Property((Genelgeler e) => e.Yil).HasMaxLength(4).IsUnicode(false)
					.HasColumnName("yil");
				entity.Property((Genelgeler e) => e.Yolu).HasMaxLength(15).IsUnicode(false)
					.HasColumnName("yolu");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Haberler> entity)
			{
				entity.ToTable("HABERLER");
				entity.Property((Haberler e) => e.Id).HasColumnName("ID");
				entity.Property((Haberler e) => e.Baslik).HasMaxLength(100).IsUnicode(false)
					.HasColumnName("BASLIK");
				entity.Property((Haberler e) => e.Description).HasMaxLength(180).IsUnicode(false)
					.HasColumnName("DESCRIPTION");
				entity.Property((Haberler e) => e.Icerik).HasColumnType("text").HasColumnName("ICERIK");
				entity.Property((Haberler e) => e.Keywords).HasMaxLength(180).IsUnicode(false)
					.HasColumnName("KEYWORDS");
				entity.Property((Haberler e) => e.Okunmasayisi).HasColumnName("OKUNMASAYISI").HasDefaultValueSql("((1))");
				entity.Property((Haberler e) => e.Resimyolu).HasMaxLength(100).IsUnicode(false)
					.HasColumnName("RESIMYOLU");
				entity.Property((Haberler e) => e.Seolink).HasMaxLength(100).IsUnicode(false)
					.HasColumnName("SEOLINK");
				entity.Property((Haberler e) => e.Tarih).HasMaxLength(20).HasColumnName("TARIH");
				entity.Property((Haberler e) => e.Tarih2).HasColumnType("datetime").HasColumnName("TARIH2");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Baskanlar> entity)
			{
				entity.ToTable("Baskanlar");
				entity.Property((Baskanlar e) => e.ID).HasColumnName("ID");
				entity.Property((Baskanlar e) => e.Isim).HasMaxLength(140).IsUnicode(false)
					.HasColumnName("Isim");
				entity.Property((Baskanlar e) => e.Twitter).HasMaxLength(140).IsUnicode(false)
					.HasColumnName("Twitter");
				entity.Property((Baskanlar e) => e.Facebook).HasMaxLength(140).IsUnicode(false)
					.HasColumnName("Facebook");
				entity.Property((Baskanlar e) => e.Instagram).HasMaxLength(140).IsUnicode(false)
					.HasColumnName("Instagram");
				entity.Property((Baskanlar e) => e.Mesaj).IsUnicode(false).HasColumnName("Mesaj");
				entity.Property((Baskanlar e) => e.Aktif).HasColumnName("Aktif");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Icerikler> entity)
			{
				entity.ToTable("ICERIKLER");
				entity.Property((Icerikler e) => e.Id).HasColumnName("ID");
				entity.Property((Icerikler e) => e.Baslik).HasMaxLength(100).IsUnicode(false)
					.HasColumnName("BASLIK");
				entity.Property((Icerikler e) => e.Description).HasMaxLength(180).IsUnicode(false)
					.HasColumnName("DESCRIPTION");
				entity.Property((Icerikler e) => e.Icerik).HasColumnType("text").HasColumnName("ICERIK");
				entity.Property((Icerikler e) => e.Keywords).HasMaxLength(180).IsUnicode(false)
					.HasColumnName("KEYWORDS");
				entity.Property((Icerikler e) => e.Okunmasayisi).HasColumnName("OKUNMASAYISI");
				entity.Property((Icerikler e) => e.Seolink).HasMaxLength(110).IsUnicode(false)
					.HasColumnName("SEOLINK");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<KurulUyeleri> entity)
			{
				entity.ToTable("KurulUyeleri");
				entity.Property((KurulUyeleri e) => e.ID).HasColumnName("ID");
				entity.Property((KurulUyeleri e) => e.Isim).HasMaxLength(140).IsUnicode(false)
					.HasColumnName("Isim");
				entity.Property((KurulUyeleri e) => e.Mevki).HasMaxLength(140).IsUnicode(false)
					.HasColumnName("Mevki");
				entity.Property((KurulUyeleri e) => e.Derece).HasColumnName("Derece");
				entity.Property((KurulUyeleri e) => e.DereceAciklama).HasMaxLength(140).IsUnicode(false)
					.HasColumnName("DereceAciklama");
				entity.Property((KurulUyeleri e) => e.Resim).HasMaxLength(140).IsUnicode(false)
					.HasColumnName("Resim");
				entity.Property((KurulUyeleri e) => e.Kurul).HasColumnName("Kurul");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Kullanicilar> entity)
			{
				entity.ToTable("KULLANICILAR");
				entity.Property((Kullanicilar e) => e.Id).HasColumnName("ID");
				entity.Property((Kullanicilar e) => e.Durum).HasColumnName("DURUM");
				entity.Property((Kullanicilar e) => e.Kullaniciadi).HasMaxLength(50).IsUnicode(false)
					.HasColumnName("KULLANICIADI");
				entity.Property((Kullanicilar e) => e.Rol).HasMaxLength(50).IsUnicode(false)
					.HasColumnName("ROL");
				entity.Property((Kullanicilar e) => e.Sifre).HasMaxLength(50).IsUnicode(false)
					.HasColumnName("SIFRE");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Menuler> entity)
			{
				entity.ToTable("menuler");
				entity.Property((Menuler e) => e.Id).HasColumnName("id");
				entity.Property((Menuler e) => e.Menuadi).HasMaxLength(50).HasColumnName("menuadi");
				entity.Property((Menuler e) => e.Parentid).HasColumnName("parentid");
				entity.Property((Menuler e) => e.Url).HasMaxLength(100).HasColumnName("url");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Odalar> entity)
			{
				entity.ToTable("Odalar");
				entity.Property((Odalar e) => e.Id).HasColumnName("id");
				entity.Property((Odalar e) => e.Adres).HasMaxLength(100);
				entity.Property((Odalar e) => e.Baskan).HasMaxLength(50);
				entity.Property((Odalar e) => e.BaskanResim).HasMaxLength(50).IsUnicode(false);
				entity.Property((Odalar e) => e.DenetimKurulu).HasMaxLength(100).IsUnicode(false);
				entity.Property((Odalar e) => e.Email).HasMaxLength(50);
				entity.Property((Odalar e) => e.Fax).HasMaxLength(50);
				entity.Property((Odalar e) => e.GenelSekreter).HasMaxLength(50);
				entity.Property((Odalar e) => e.Kroki).HasMaxLength(300);
				entity.Property((Odalar e) => e.Merkezilce).HasColumnName("merkezilce");
				entity.Property((Odalar e) => e.OdaAdi).HasMaxLength(100);
				entity.Property((Odalar e) => e.Tel).HasMaxLength(50);
				entity.Property((Odalar e) => e.Web).HasMaxLength(50);
				entity.Property((Odalar e) => e.YonetimKurulu).HasMaxLength(250).IsUnicode(false);
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Popup> entity)
			{
				entity.ToTable("Popup");
				entity.Property((Popup e) => e.Id).HasColumnName("Id");
				entity.Property((Popup e) => e.Title).HasMaxLength(4096);
				entity.Property((Popup e) => e.Description).HasColumnType("text");
				entity.Property((Popup e) => e.ImageLink).HasColumnName("Image").HasMaxLength(4096);
				entity.Property((Popup e) => e.VideoLink).HasColumnName("Video").HasMaxLength(4096);
				entity.Property((Popup e) => e.IsActive).HasColumnName("IsActive");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Sozler> entity)
			{
				entity.ToTable("Sozler");
				entity.Property((Sozler e) => e.Id).HasColumnName("id");
				entity.Property((Sozler e) => e.Soz).HasMaxLength(250).IsUnicode(false);
				entity.Property((Sozler e) => e.Yazar).HasMaxLength(50).IsUnicode(false);
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Tbilgiedinme> entity)
			{
				entity.HasKey((Tbilgiedinme e) => e.Tid);
				entity.ToTable("tbilgiedinme");
				entity.Property((Tbilgiedinme e) => e.Tid).HasColumnName("tid");
				entity.Property((Tbilgiedinme e) => e.Adres).HasMaxLength(250).HasColumnName("adres");
				entity.Property((Tbilgiedinme e) => e.Adsoyad).HasMaxLength(50).IsUnicode(false)
					.HasColumnName("adsoyad");
				entity.Property((Tbilgiedinme e) => e.Cevap).HasColumnName("cevap");
				entity.Property((Tbilgiedinme e) => e.Eposta).HasMaxLength(50).IsUnicode(false)
					.HasColumnName("eposta");
				entity.Property((Tbilgiedinme e) => e.Fax).HasMaxLength(50).HasColumnName("fax");
				entity.Property((Tbilgiedinme e) => e.Istek).HasMaxLength(1000).HasColumnName("istek");
				entity.Property((Tbilgiedinme e) => e.Tarih).HasColumnType("datetime").HasColumnName("tarih");
				entity.Property((Tbilgiedinme e) => e.Tckimlik).HasMaxLength(11).IsUnicode(false)
					.HasColumnName("tckimlik")
					.IsFixedLength();
				entity.Property((Tbilgiedinme e) => e.Telefon).HasMaxLength(50).HasColumnName("telefon");
				entity.Property((Tbilgiedinme e) => e.Unvan).HasMaxLength(100).HasColumnName("unvan");
				entity.Property((Tbilgiedinme e) => e.Yanitkanali).HasMaxLength(20).IsUnicode(false)
					.HasColumnName("yanitkanali");
			});
			modelBuilder.Entity(delegate(EntityTypeBuilder<Youtubevideo> entity)
			{
				entity.HasKey((Youtubevideo e) => e.VideoId);
				entity.ToTable("YOUTUBEVIDEO");
				entity.Property((Youtubevideo e) => e.Description).HasMaxLength(180).IsUnicode(false);
				entity.Property((Youtubevideo e) => e.Kategori).HasMaxLength(50).IsUnicode(false);
				entity.Property((Youtubevideo e) => e.Keywords).HasMaxLength(180).IsUnicode(false);
				entity.Property((Youtubevideo e) => e.ProgramAciklama).HasMaxLength(250).IsUnicode(false);
				entity.Property((Youtubevideo e) => e.ProgramAdi).HasMaxLength(50).IsUnicode(false);
				entity.Property((Youtubevideo e) => e.Tarih).HasColumnType("datetime");
				entity.Property((Youtubevideo e) => e.YoutubeThumbSize).HasMaxLength(50).IsUnicode(false);
				entity.Property((Youtubevideo e) => e.YoutubeVideoNumber).HasMaxLength(50).IsUnicode(false);
			});

			modelBuilder.Entity(delegate(EntityTypeBuilder<User> entity)
			{
				entity.HasKey((User e) => e.ID);
				entity.ToTable("User");
				entity.Property((User e) => e.Username).HasMaxLength(64);
				entity.Property((User e) => e.Password).HasMaxLength(128);
				entity.Property((User e) => e.PermissionLevel).HasMaxLength(16);
			});
		}
	}
}
