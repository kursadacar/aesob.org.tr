using Microsoft.AspNetCore.Mvc;
using System;

#nullable disable

namespace aesob.org.tr.Models
{
    public partial class Gbilgiedinme : IAesobEntity
    {
        int IAesobEntity.EntityId { get => Gid; set => Gid = value; }
        [HiddenInput]
        public int Gid { get; set; }
        public string Adsoyad { get; set; }
        public string Adres { get; set; }
        public string Tckimlik { get; set; }
        public string Eposta { get; set; }
        public string Yanitkanali { get; set; }
        public string Telefon { get; set; }
        public string Fax { get; set; }
        public string Istek { get; set; }
        public DateTime? Tarih { get; set; }
        public bool? Cevap { get; set; }
    }
}
