using Microsoft.AspNetCore.Mvc;

namespace aesob.org.tr.Models
{
    public partial class Sozler : IAesobEntity
	{
		int IAesobEntity.EntityId { get => Id; set => Id = value; }

        [HiddenInput]
		public int Id { get; set; }

		public string Soz { get; set; }

		public string Yazar { get; set; }
	}
}
