#nullable disable

using Microsoft.AspNetCore.Mvc;

namespace aesob.org.tr.Models
{
    public partial class Menuler : IAesobEntity
    {
        int IAesobEntity.EntityId { get => Id; set => Id = value; }
        [HiddenInput]
        public int Id { get; set; }
        public string Menuadi { get; set; }
        public int? Parentid { get; set; }
        public string Url { get; set; }
    }
}
