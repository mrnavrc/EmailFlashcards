using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using EmailFlashcards.Models;
using Microsoft.Extensions.Options;

namespace EmailFlashcards.Controllers
{
    public class ContactUs : Controller
    {
        private readonly EmailSettings _emailSettings;

        public ContactUs(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        
        public IActionResult Index(string SuccessMessage = null)
        {
            ViewData["SuccessEmail"] = SuccessMessage;
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail(string CustomerName, 
                                       string CustomerEmail, 
                                       string CustomerEmailText, 
                                       string CustomerEmailSubject)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(CustomerName, CustomerEmail));
            message.To.Add(new MailboxAddress("To Admin",_emailSettings.ContactEmail));
            message.Subject = CustomerEmailSubject;

            message.Body = new TextPart("plain")
            {
                Text = @CustomerEmailText
            };

            using (var client = new SmtpClient())
            {
                client.Connect(_emailSettings.ContactClient, _emailSettings.ContactPort, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_emailSettings.ContactEmail, _emailSettings.ContactPassword);

                client.Send(message);
                client.Disconnect(true);
            }

            return RedirectToAction("Index", new { SuccessMessage = "SuccesEmail" });

        }
    }
}
