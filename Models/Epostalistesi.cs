using Microsoft.AspNetCore.Mvc;
using System;

#nullable disable

namespace aesob.org.tr.Models
{
    public partial class Epostalistesi : IAesobEntity
    {
        int IAesobEntity.EntityId { get => Id; set => Id = value; }
        [HiddenInput]
        public int Id { get; set; }
        public string Adsoyad { get; set; }
        public string Eposta { get; set; }
        public DateTime? Tarih { get; set; }
        public bool? Durum { get; set; }
    }
}
