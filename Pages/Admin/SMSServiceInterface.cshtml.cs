using aesob.org.tr.Models;
using aesob.org.tr.Services;
using aesob.org.tr.Services.Sms;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aesob.org.tr.Pages.Admin
{
    public class SMSServiceInterfaceModel : AesobModelBase
    {
        public SMSServiceInterfaceModel(AesobDbContext context) : base(context)
        {
        }

        public IActionResult OnGet(int pageIndex = 0)
        {
            return Page();
        }

        public async Task<ServiceActionResult> OnGetSendMessage(string message)
        {
            return await SMSService.SendMassSms(message);
        }
    }
}
