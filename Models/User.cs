using Microsoft.AspNetCore.Mvc;

namespace aesob.org.tr.Models
{
    public partial class User : IAesobEntity
	{
		int IAesobEntity.EntityId { get => ID; set => ID = value; }

        [HiddenInput]
		public int ID { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public string PermissionLevel { get; set; }
	}
}
