using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using aesob.org.tr.Models;
using System.Net.Mail;
using System.Net;
using System;

namespace aesob.org.tr.Pages.Content
{
    public class ContactModel : AesobModelBase
    {
        public ContactModel(AesobDbContext context) : base(context)
        {
            var temp = context.Haberlers.FirstOrDefault();//Use database to ensure page initializes model values correctly
            //TODO_Kursad: Find a better solution
        }

        public IActionResult OnPostSubmitForm(IFormCollection data)
        {
            var name = data["name"];
            var phone = data["phone"];
            var email = data["email"];
            var subject = data["subject"];
            var message = data["message"];

            MailMessage mailMessage = new MailMessage();

            var originalProtocols = ServicePointManager.SecurityProtocol;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            try
            {
                mailMessage.From = new MailAddress("admin@testpages.aesob.org.tr", "Başkana Mesaj");
            }
            catch
            {
                return new ObjectResult("email-error");
            }
            mailMessage.Subject = name + " tarafından Başkana Mesaj. (" + subject + ")";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = message + "<br><br><br>Adı Soyadı: " + name + "<br>E-Posta Adresi: " + email + "<br>Telefon: " + phone + "<br>";

            try
            {
                mailMessage.To.Add("admin@testpages.aesob.org.tr");
            }
            catch (Exception e)
            {
                return new ObjectResult("error -> " + e.Message);
            }

            try
            {
                SmtpClient smtpClient = new SmtpClient("testpages.aesob.org.tr");
                smtpClient.Port = 587;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential()
                {
                    UserName = "admin@testpages.aesob.org.tr",
                    Password = "!Asperox123."
                };

                smtpClient.Send(mailMessage);
            }
            catch (Exception e)
            {
                return new ObjectResult("error -> " + e.Message);
            }

            ServicePointManager.SecurityProtocol = originalProtocols;
            return new ObjectResult("success");
        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
