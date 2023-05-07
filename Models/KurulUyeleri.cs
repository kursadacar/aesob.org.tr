namespace aesob.org.tr.Models
{
    public partial class KurulUyeleri : IAesobEntity
	{
		int IAesobEntity.EntityId { get => ID; set => ID = value; }

        [AesobEntityData(AttributeName = "ID", DataType = AesobEntityDataType.Hidden)]
		public int ID { get; set; }

		[AesobEntityData(AttributeName = "İsim")]
		public string Isim { get; set; }

		[AesobEntityData(AttributeName = "Mevki")]
		public string Mevki { get; set; }

		[AesobEntityData(AttributeName = "Derece")]
		public int Derece { get; set; }

		[AesobEntityData(AttributeName = "Derece Açıklama")]
		public string DereceAciklama { get; set; }

		[AesobEntityData(AttributeName = "Resim", DataType = AesobEntityDataType.Image, ImageDirectory = "Resimler/", UploadedFileDirectory = "images/Resimler/")]
		public string Resim { get; set; }

		[AesobEntityData(AttributeName = "Kurul ID", DataType = AesobEntityDataType.Number, RangeMin = 0, RangeMax = 2)]
		public int Kurul { get; set; }
	}
}
