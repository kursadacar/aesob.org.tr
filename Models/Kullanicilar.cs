#nullable disable

using Microsoft.AspNetCore.Mvc;

namespace aesob.org.tr.Models
{
    public partial class Kullanicilar : IAesobEntity
    {
        int IAesobEntity.EntityId { get => Id; set => Id = value; }
        [HiddenInput]
        public int Id { get; set; }
        public string Kullaniciadi { get; set; }
        public string Sifre { get; set; }
        public string Rol { get; set; }
        public bool? Durum { get; set; }
    }
}
