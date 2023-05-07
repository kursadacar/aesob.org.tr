namespace aesob.org.tr.Models
{
    public partial class Dergi : IAesobEntity
	{
		int IAesobEntity.EntityId { get => Id; set => Id = value; }

        [AesobEntityData(AttributeName = "ID", DataType = AesobEntityDataType.Hidden)]
		public int Id { get; set; }

		[AesobEntityData(AttributeName = "Kapak Resmi", DataType = AesobEntityDataType.Image, ImageDirectory = "Dergi/", UploadedFileDirectory = "images/Dergi/")]
		public string Kapakresimyolu { get; set; }

		[AesobEntityData(AttributeName = "Dosya Yolu", DataType = AesobEntityDataType.File, UploadedFileDirectory = "images/Dergi/", UploadedFileTypeFilter = ".pdf")]
		public string Dosyayolu { get; set; }

		[AesobEntityData(AttributeName = "Dergi Sayısı")]
		public string Dergisayisi { get; set; }

		[AesobEntityData(AttributeName = "Dergi Yılı", DataType = AesobEntityDataType.Number, RangeMin = 2010, RangeMax = -1)]
		public int? Dergiyil { get; set; }
	}
}
