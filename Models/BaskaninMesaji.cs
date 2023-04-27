using System;

#nullable disable

namespace aesob.org.tr.Models
{
    public partial class BaskaninMesaji : IAesobEntity
    {
        int IAesobEntity.EntityId { get => Id; set => Id = value; }

        [AesobEntityData(AttributeName = "ID", DataType = AesobEntityDataType.Hidden)]
        public int Id { get; set; }

        [AesobEntityData(AttributeName = "Başlık")]
        public string Baslik { get; set; }

        [AesobEntityData(AttributeName = "İçerik", DataType = AesobEntityDataType.TextArea)]
        public string Icerik { get; set; }

        [AesobEntityData(AttributeName = "SEO Link")]
        public string Seolink { get; set; }

        [AesobEntityData(AttributeName = "Açıklama")]
        public string Description { get; set; }

        [AesobEntityData(AttributeName = "Anahtar Kelimeler")]
        public string Keywords { get; set; }

        [AesobEntityData(AttributeName = "Tarih", DataType = AesobEntityDataType.Date)]
        public DateTime? Tarih { get; set; }

        [AesobEntityData(AttributeName = "Okunma Sayısı", DataType = AesobEntityDataType.Disabled)]
        public int? Okunmasayisi { get; set; }
    }
}
