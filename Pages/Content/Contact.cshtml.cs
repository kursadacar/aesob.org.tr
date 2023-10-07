using System.Linq;
using aesob.org.tr.Models;
using aesob.org.tr.Services;
using Aesob.Web.Library.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace aesob.org.tr.Pages.Content
{
    public class ContactModel : AesobModelBase
    {
        public ContactModel(AesobDbContext context)
            : base(context)
        {
            Haberler temp = context.Haberlers.FirstOrDefault();
        }

        public void OnPostSubmitForm(IFormCollection data)
        {
            StringValues name = data["name"];
            StringValues phone = data["phone"];
            StringValues email = data["email"];
            StringValues innerSubject = data["subject"];
            StringValues message = data["message"];
            string alias = "Başkana Mesaj";
            string subject = string.Concat(name, " tarafından Başkana Mesaj. (", innerSubject, ")");
            string content = string.Concat(message, "<br><br><br>Adı Soyadı: ", name, "<br>E-Posta Adresi: ", email, "<br>Telefon: ", phone, "<br>");
            string targetAddress = "admin@aesob.org.tr";
            EmailService.SendContactEmail(new EMailData(alias, subject, content, targetAddress));
        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
