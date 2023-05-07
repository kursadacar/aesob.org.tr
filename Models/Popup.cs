namespace aesob.org.tr.Models
{
    public partial class Popup : IAesobEntity
	{
		int IAesobEntity.EntityId { get => Id; set => Id = value; }

        [AesobEntityData(AttributeName = "ID", DataType = AesobEntityDataType.Hidden)]
		public int Id { get; set; }

		[AesobEntityData(AttributeName = "Başlık")]
		public string Title { get; set; }

		[AesobEntityData(AttributeName = "Açıklama")]
		public string Description { get; set; }

		[AesobEntityData(AttributeName = "Resim Linki", DataType = AesobEntityDataType.Image, ImageDirectory = "Popup/", UploadedFileDirectory = "images/Popup/")]
		public string ImageLink { get; set; }

		[AesobEntityData(AttributeName = "Video Linki", DataType = AesobEntityDataType.SelfHostedVideo, UploadedFileDirectory = "images/Popup/Video")]
		public string VideoLink { get; set; }

		[AesobEntityData(AttributeName = "Aktif", DataType = AesobEntityDataType.Toggle)]
		public bool IsActive { get; set; }
	}
}
