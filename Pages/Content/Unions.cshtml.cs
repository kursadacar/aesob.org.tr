using aesob.org.tr.Models;
using Microsoft.AspNetCore.Mvc;

namespace aesob.org.tr.Pages.Content
{
    public enum UnionType
    {
        MerkezOda,
        IlceOda,
        NumUnions
    }

    public class UnionsModel : AesobModelBase
    {
        public bool IsCentral { get; private set; }

        public int UnionID { get; private set; }

        public UnionsModel(AesobDbContext context)
            : base(context)
        {
        }

        public IActionResult OnGet(int type, int union)
        {
            IsCentral = type == 0;
            UnionID = union;
            return Page();
        }
    }
}
