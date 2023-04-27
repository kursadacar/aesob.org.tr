namespace aesob.org.tr.Models
{
    public partial class Baskanlar : IAesobEntity
    {
        int IAesobEntity.EntityId { get => ID; set => ID = value; }

        [AesobEntityData(AttributeName = "ID", DataType = AesobEntityDataType.Hidden)]
        public int ID { get; set; }

        [AesobEntityData(AttributeName = "İsim")]
        public string Isim { get; set; }

        [AesobEntityData(AttributeName = "Twitter Hesabı")]
        public string Twitter { get; set; }

        [AesobEntityData(AttributeName = "Facebook Hesabı")]
        public string Facebook { get; set; }

        [AesobEntityData(AttributeName = "Instagram Hesabı")]
        public string Instagram { get; set; }

        [AesobEntityData(AttributeName = "Mesajı", DataType = AesobEntityDataType.TextArea)]
        public string Mesaj { get; set; }

        [AesobEntityData(AttributeName = "Aktif", DataType = AesobEntityDataType.Toggle)]
        public bool Aktif { get; set; }
    }
}
