using aesob.org.tr.Models;
using aesob.org.tr.Services;
using aesob.org.tr.Services.Sms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static aesob.org.tr.Pages.Content.NewsFeedModel;
using System.Collections.Generic;
using System;

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

        public ServiceActionResult OnGetSendMessage(string message)
        {
            return SMSService.SendMassSms(message);
        }
    }
}
