#nullable disable


namespace aesob.org.tr.Models
{
    public partial class Ayarlar : IAesobEntity
    {
        int IAesobEntity.EntityId { get => Id; set => Id = value; }
        [AesobEntityData(AttributeName = "ID", DataType = AesobEntityDataType.Hidden)]
        public int Id { get; set; }
        public int? TemaNo { get; set; }
        public int? Iletisimformu { get; set; }
        public bool? Dovizkurlari { get; set; }
        public bool? Havadurumu { get; set; }
        public int? Slaytebat { get; set; }
        public int? Haberebatbuyuk { get; set; }
        public int? Haberebatkucuk { get; set; }
        public string Haritanokta1 { get; set; }
        public string Haritanokta2 { get; set; }
        public int? Haritazoom { get; set; }
        public string Kroki { get; set; }
        public string Havadurumuil { get; set; }
        public string Havadurumuborder { get; set; }
        public string Havadurumuzemin { get; set; }
        public int? MenuSayisi { get; set; }
        public string FtpAdres { get; set; }
        public string FtpKullanici { get; set; }
        public string FtpPassword { get; set; }
        public int? Firmano { get; set; }
        public int? Urunebatbuyuk { get; set; }
        public int? Urunebatkucuk { get; set; }
        public string GoogleAnalytics { get; set; }
        public int? AracEbatBuyuk { get; set; }
        public int? AracEbatOrta { get; set; }
        public int? AracEbatKucuk { get; set; }
    }
}
