namespace aesob.org.tr.Models
{
    public partial class Phone : IAesobEntity
	{
		int IAesobEntity.EntityId { get => Id; set => Id = value; }

        public int Id { get; set; }

		public string Name { get; set; }

		public string PhoneNumber { get; set; }

		public string Union { get; set; }

		public string UnionPhone { get; set; }

		public string UnionFax { get; set; }

		public bool IsActive { get; set; }
	}
}
