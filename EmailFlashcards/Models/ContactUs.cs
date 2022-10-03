using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace EmailFlashcards.Models
{
    [NotMapped]
    public class ContactUs
    {
        //[Required]
        public string? CustomerName { get; set; }

        //[Required]
        //[EmailAddress]
        public string? CustomerEmail { get; set; }
        public string? CustomerEmailSubject { get; set; }
        public string? CustomerEmailText { get; set; }

    }
}
