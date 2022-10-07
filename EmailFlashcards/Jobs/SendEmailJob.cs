using EmailFlashcards.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Quartz;
using System.Runtime.CompilerServices;
using EmailFlashcards.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Data.Entity.SqlServer;

namespace EmailFlashcards.Jobs
{
    public class SendEmailJob : IJob
    {
        private readonly EmailSettings _emailSettings;
        private readonly ApplicationDbContext _context;

        public SendEmailJob(IOptions<EmailSettings> emailSettings,
                                    ApplicationDbContext context)
        {
            _emailSettings = emailSettings.Value;
            _context = context;

        }
        public Task Execute(IJobExecutionContext jobcontext)
        {

            string cronTime = jobcontext.FireTimeUtc.Hour.ToString() + ":00:00";

            var usersByTime = _context.FlashcardsSettings
                                      .Where(f => f.Time == TimeOnly.Parse(cronTime))
                                      .ToList()
                                      .Select(u => (u.FlashcardEmailAdress, u.UserId, u.FlashcardsPerDay))
                                      .ToList();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("FlashcardEmailApp", "navratilm@outlook.com"));

            foreach (var emailAdress in usersByTime.Select(u => (u.FlashcardEmailAdress, u.UserId)))
            {
                message.To.Clear();
                message.To.Add(new MailboxAddress("To Our Flashcards Student", emailAdress.FlashcardEmailAdress));
                message.Subject = "Todays flashcards for you";

                int numberOfFlashcardsPerDay = usersByTime.Where(u => u.UserId == emailAdress.UserId)
                                                          .Select(f => f.FlashcardsPerDay)
                                                          .FirstOrDefault();

                var emailMessageHTML = _context.Flashcards.Where(u => u.UserId == emailAdress.UserId)
                                                          .Select(f => f.FlashcardText)
                                                          .Take(numberOfFlashcardsPerDay)
                                                          .ToList()
                                                          .OrderBy(o => new Random().Next());

                message.Body = new TextPart("html")
                {

                    Text = String.Join("<hr>", emailMessageHTML)

                };

                using (var client = new SmtpClient())
                {
                    client.Connect(_emailSettings.ContactClient, _emailSettings.ContactPort, false);
                    client.Authenticate(_emailSettings.ContactEmail, _emailSettings.ContactPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            return Task.FromResult(true);
        }
    }
}
