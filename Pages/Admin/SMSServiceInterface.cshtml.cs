using aesob.org.tr.Models;
using aesob.org.tr.Services;
using aesob.org.tr.Services.Sms;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public async Task<ServiceActionResult> OnGetSendMessage(string message, DateTime sendDate, int genderFilter)
        {
            bool sendImmediate = false;
            if(sendDate <= DateTime.Now.AddMinutes(1))
            {
                sendDate = DateTime.Now;
                sendImmediate = true;
            }

            var result = await SMSService.SendMassSms(message, sendDate, genderFilter);

            if(!sendImmediate && result.Result == ServiceActionResult.ActionResult.Success)
            {
                return ServiceActionResult.CreateSuccess($"İleri tarihli mesaj başarı ile kaydedildi: {result.Message}");
            }

            if(result.Result == ServiceActionResult.ActionResult.Success)
            {
                return ServiceActionResult.CreateSuccess($"Toplu mesaj başarı ile gönderildi: {result.Message}");
            }

            return result;
        }
    }
}
