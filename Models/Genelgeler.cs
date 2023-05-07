using System;

namespace aesob.org.tr.Models
{
    public partial class Genelgeler : IAesobEntity
	{
		int IAesobEntity.EntityId { get => Id; set => Id = value; }

        [AesobEntityData(AttributeName = "ID", DataType = AesobEntityDataType.Hidden)]
		public int Id { get; set; }

		[AesobEntityData(AttributeName = "Yıl")]
		public string Yil { get; set; }

		[AesobEntityData(AttributeName = "Tarih", DataType = AesobEntityDataType.Date)]
		public DateTime? Tarih { get; set; }

		[AesobEntityData(AttributeName = "Konu")]
		public string Konu { get; set; }

		[AesobEntityData(AttributeName = "Sayı")]
		public string Sayi { get; set; }

		[AesobEntityData(AttributeName = "Genelge Dosyası", DataType = AesobEntityDataType.File, UploadedFileDirectory = "images/Genelgeler", UploadedFileTypeFilter = ".pdf")]
		public string Yolu { get; set; }

		[AesobEntityData(AttributeName = "Eklenme Tarihi", DataType = AesobEntityDataType.Date)]
		public DateTime? Eklemetarihi { get; set; }
	}
}
