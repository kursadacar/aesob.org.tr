using System;
using Microsoft.AspNetCore.Mvc;

namespace aesob.org.tr.Models
{
    public partial class Firmabilgileri : IAesobEntity
	{
		int IAesobEntity.EntityId { get => Id; set => Id = value; }

        [HiddenInput]
		public int Id { get; set; }

		public string Firmaadi { get; set; }

		public string Adres { get; set; }

		public string Tel { get; set; }

		public string Fax { get; set; }

		public string Web { get; set; }

		public string Eposta { get; set; }

		public DateTime? Sonkullanmatarihi { get; set; }

		public string Epostahesapadi { get; set; }

		public string Epostahesapsifresi { get; set; }

		public int? Epostaport { get; set; }
	}
}
