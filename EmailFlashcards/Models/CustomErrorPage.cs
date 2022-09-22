using System.ComponentModel.DataAnnotations.Schema;

namespace EmailFlashcards.Models
{
    [NotMapped]
    public class CustomErrorPage
    {
        public int code { get; set; }
        public string? message { get; set; }
    }
}
