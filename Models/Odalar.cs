#nullable disable


namespace aesob.org.tr.Models
{
    public partial class Odalar : IAesobEntity
    {
        int IAesobEntity.EntityId { get => Id; set => Id = value; }

        [AesobEntityData(AttributeName = "ID", DataType = AesobEntityDataType.Hidden)]
        public int Id { get; set; }

        [AesobEntityData(AttributeName = "Oda Adı")]
        public string OdaAdi { get; set; }

        [AesobEntityData(AttributeName = "Adres")]
        public string Adres { get; set; }

        [AesobEntityData(AttributeName = "Telefon")]
        public string Tel { get; set; }

        [AesobEntityData(AttributeName = "Fax")]
        public string Fax { get; set; }

        [AesobEntityData(AttributeName = "Genel Sekreter")]
        public string GenelSekreter { get; set; }

        [AesobEntityData(AttributeName = "Web Sitesi")]
        public string Web { get; set; }

        [AesobEntityData(AttributeName = "E-Mail")]
        public string Email { get; set; }

        [AesobEntityData(AttributeName = "Başkan Resim", DataType = AesobEntityDataType.Image, ImageDirectory = "Resimler/", UploadedFileDirectory = "images/Resimler/")]
        public string BaskanResim { get; set; }

        [AesobEntityData(AttributeName = "Başkan")]
        public string Baskan { get; set; }

        [AesobEntityData(AttributeName = "Yönetim Kurulu")]
        public string YonetimKurulu { get; set; }

        [AesobEntityData(AttributeName = "Denetim Kurulu")]
        public string DenetimKurulu { get; set; }

        [AesobEntityData(AttributeName = "Kroki")]
        public string Kroki { get; set; }

        [AesobEntityData(AttributeName = "İlçe", DataType = AesobEntityDataType.Toggle)]
        public bool Merkezilce { get; set; }
    }
}
