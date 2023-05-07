using System;
using Microsoft.AspNetCore.Mvc;

namespace aesob.org.tr.Models
{
    public partial class Tbilgiedinme : IAesobEntity
	{
		int IAesobEntity.EntityId { get => Tid; set => Tid = value; }

        [HiddenInput]
		public int Tid { get; set; }

		public string Unvan { get; set; }

		public string Adres { get; set; }

		public string Adsoyad { get; set; }

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
