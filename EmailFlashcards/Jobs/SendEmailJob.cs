using EmailFlashcards.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Quartz;
using System.Runtime.CompilerServices;
using EmailFlashcards.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmailFlashcards.Jobs
{
    public class SendEmailJob : IJob //SendEmailJob extends the IJob interface and implements Execute method
    {
        private readonly EmailSettings _emailSettings;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public SendEmailJob(IOptions<EmailSettings> emailSettings,
                                    ApplicationDbContext context,
                                    UserManager<User> userManager)
        {
            _emailSettings = emailSettings.Value;
            _context = context;
            _userManager = userManager;
        }
        public Task Execute(IJobExecutionContext jobcontext)
        {




            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("FlashcardEmailApp", "navratilm@outlook.com"));
            //message.To.Add(new MailboxAddress("To Admin", "navratilm@outlook.com"));
            //message.Subject = "test job";

            //message.Body = new TextPart("html")
            //{
            //    Text = "test job"
            //};

            ////SMTP
            //using (var client = new SmtpClient())
            //{
            //    client.Connect(_emailSettings.ContactClient, _emailSettings.ContactPort, false);
            //    client.Authenticate(_emailSettings.ContactEmail, _emailSettings.ContactPassword);
            //    client.Send(message);
            //    client.Disconnect(true);
            //}

            return Task.FromResult(true);
        }
    }
}
