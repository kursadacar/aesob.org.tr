namespace aesob.org.tr.Models
{
    public partial class EBulten : IAesobEntity
	{
		int IAesobEntity.EntityId { get => Id; set => Id = value; }

        public int Id { get; set; }

		public string Name { get; set; }

		public string EMail { get; set; }

		public string Phone { get; set; }

		public string Union { get; set; }

		public string Group { get; set; }

		public bool IsEmailActive { get; set; }

		public bool IsPhoneActive { get; set; }
	}
}
