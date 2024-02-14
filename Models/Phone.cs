namespace aesob.org.tr.Models
{
    public partial class Phone : IAesobEntity
	{
		int IAesobEntity.EntityId { get => Id; set => Id = value; }

        [AesobEntityData(AttributeName = "ID", DataType = AesobEntityDataType.Hidden)]
        public int Id { get; set; }

        [AesobEntityData(AttributeName = "İsim", DataType = AesobEntityDataType.Text)]
        public string Name { get; set; }

        [AesobEntityData(AttributeName = "Telefon Numarası", DataType = AesobEntityDataType.Text)]
        public string PhoneNumber { get; set; }

        [AesobEntityData(AttributeName = "Oda", DataType = AesobEntityDataType.Text)]
        public string Union { get; set; }

        [AesobEntityData(AttributeName = "Oda Telefon Numarası", DataType = AesobEntityDataType.Text)]
        public string UnionPhone { get; set; }

        [AesobEntityData(AttributeName = "Oda Fax Numarası", DataType = AesobEntityDataType.Text)]
        public string UnionFax { get; set; }

        [AesobEntityData(AttributeName = "Aktif", DataType = AesobEntityDataType.Toggle)]
        public bool IsActive { get; set; }
    }
}
