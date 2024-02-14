using aesob.org.tr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace aesob.org.tr.Pages.Admin
{
    public class PhoneEditorModel : AdminModelBase<Phone>
    {
        public PhoneEditorModel(AesobDbContext context) : base(context)
        {
        }
    }
}
