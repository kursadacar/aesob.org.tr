using Microsoft.AspNetCore.Mvc;
using aesob.org.tr.Models;

namespace aesob.org.tr.Pages.Content
{
    public enum UnionType : int
    {
        MerkezOda = 0,
        IlceOda = 1,
        NumUnions
    }

    public class UnionsModel : AesobModelBase
    {
        public bool IsCentral { get; private set; }
        public int UnionID { get; private set; }

        public UnionsModel(AesobDbContext context) : base(context)
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
