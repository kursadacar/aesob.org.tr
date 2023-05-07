using System;
using System.Runtime.CompilerServices;

namespace aesob.org.tr.Models
{
	[AttributeUsage(AttributeTargets.Property)]
	public class AesobEntityDataAttribute : Attribute
	{
		public string AttributeName { get; set; }

		public AesobEntityDataType DataType { get; set; }

		public int RangeMin { get; set; } = -1;


		public int RangeMax { get; set; } = -1;


		public string ImageDirectory { get; set; }

		public string UploadedFileDirectory { get; set; }

		public string UploadedFileTypeFilter { get; set; }

		public AesobEntityDataAttribute([CallerMemberName] string memberName = null)
		{
			if (memberName != null)
			{
				AttributeName = memberName;
			}
		}
	}
}
