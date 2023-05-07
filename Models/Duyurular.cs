using System;

namespace aesob.org.tr.Models
{
	public partial class Duyurular : IAesobEntity
	{
		int IAesobEntity.EntityId { get => Id; set => Id = value; }

        [AesobEntityData(AttributeName = "ID", DataType = AesobEntityDataType.Hidden)]
		public int Id { get; set; }

		[AesobEntityData(AttributeName = "Başlık")]
		public string Baslik { get; set; }

		[AesobEntityData(AttributeName = "İçerik", DataType = AesobEntityDataType.TextArea)]
		public string Icerik { get; set; }

		[AesobEntityData(AttributeName = "Eklenme Tarihi", DataType = AesobEntityDataType.Date)]
		public DateTime? Eklemetarihi { get; set; }

		[AesobEntityData(AttributeName = "Durum", DataType = AesobEntityDataType.Toggle)]
		public bool Durum { get; set; }
	}
}
