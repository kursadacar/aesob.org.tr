using System;

#nullable disable

namespace aesob.org.tr.Models
{
    public partial class Youtubevideo : IAesobEntity
    {
        int IAesobEntity.EntityId { get => VideoId; set => VideoId = value; }

        [AesobEntityData(AttributeName = "ID", DataType = AesobEntityDataType.Hidden)]
        public int VideoId { get; set; }

        [AesobEntityData(AttributeName = "Kategori")]
        public string Kategori { get; set; }

        [AesobEntityData(AttributeName = "Youtube Video Kodu", DataType = AesobEntityDataType.YoutubePreview)]
        public string YoutubeVideoNumber { get; set; }

        [AesobEntityData(AttributeName = "Youtube Thumbnail Boyutu*")]
        public string YoutubeThumbSize { get; set; }

        [AesobEntityData(AttributeName = "Program Adı")]
        public string ProgramAdi { get; set; }

        [AesobEntityData(AttributeName = "Program Açıklama")]
        public string ProgramAciklama { get; set; }

        [AesobEntityData(AttributeName = "Tarih", DataType = AesobEntityDataType.Date)]
        public DateTime? Tarih { get; set; }

        [AesobEntityData(AttributeName = "Açıklama")]
        public string Description { get; set; }

        [AesobEntityData(AttributeName = "Anahtar Kelimeler")]
        public string Keywords { get; set; }

        [AesobEntityData(AttributeName = "İzlenme Sayısı", DataType = AesobEntityDataType.Hidden)]
        public int? Izlenme { get; set; }
    }
}
