using aesob.org.tr.Models;
using aesob.org.tr.Services;
using aesob.org.tr.Services.Sms;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

            if(!sendImmediate && result?.Result == ServiceActionResult.ActionResult.Success)
            {
                return ServiceActionResult.CreateSuccess($"İleri tarihli mesaj başarı ile kaydedildi: {result.Message}");
            }

            if(result?.Result == ServiceActionResult.ActionResult.Success)
            {
                return ServiceActionResult.CreateSuccess($"Toplu mesaj başarı ile gönderildi: {result.Message}");
            }

            return result;
        }

        public async Task<MemberInfoResult> OnGetDisplayMemberInfo(int genderFilter)
        {
            var infos = SMSHelper.GetMemberInfoForAESOB((SMSHelper.MemberGender)genderFilter);

            //List<SMSHelper.MemberInfo> infos = new List<SMSHelper.MemberInfo>();
            //for(int i = 0; i< 10; i++)
            //{
            //    infos.Add(new SMSHelper.MemberInfo()
            //    {
            //        SicilNo = i,
            //        AdSoyad = "Ad Soyad " + i,
            //        TelefonNumarasi = "Telefon Numarası " + i,
            //        TCKimlikNo = "TC Kimlik No " + i,
            //    });
            //}

            return new MemberInfoResult(infos);
        }
    }

    public class MemberInfoResult : JsonResult
    {
        public List<MemberInfo> Info { get; set; }

        public MemberInfoResult(object value) : base(value)
        {
            Info = new List<MemberInfo>();
        }
    }
}
