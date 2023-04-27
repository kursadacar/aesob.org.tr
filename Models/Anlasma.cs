using System;
using System.Collections.Generic;

#nullable disable

namespace aesob.org.tr.Models
{
    public partial class Anlasma : IAesobEntity
    {
        int IAesobEntity.EntityId { get => Id; set => Id = value; }

        [AesobEntityData(AttributeName = "ID", DataType = AesobEntityDataType.Hidden)]
        public int Id { get; set; }

        [AesobEntityData(AttributeName = "Taraf")]
        public string Taraf { get; set; }

        [AesobEntityData(AttributeName = "Başlangıç Tarihi", DataType = AesobEntityDataType.Date)]
        public string Bastarihi { get; set; }

        [AesobEntityData(AttributeName = "Bitiş Tarihi", DataType = AesobEntityDataType.Date)]
        public string Bittarihi { get; set; }

        [AesobEntityData(AttributeName = "İçerik", DataType = AesobEntityDataType.TextArea)]
        public string Icerik { get; set; }

        [AesobEntityData(AttributeName = "Taraf Adres")]
        public string Tarafadres { get; set; }

        [AesobEntityData(AttributeName = "Taraf Telefon")]
        public string Taraftelefon { get; set; }

        [AesobEntityData(AttributeName = "Ekleme Tarihi" , DataType = AesobEntityDataType.Date)]
        public DateTime? Eklemetarihi { get; set; }

        [AesobEntityData(AttributeName = "Durum", DataType = AesobEntityDataType.Toggle)]
        public bool Durum { get; set; }
    }
}
