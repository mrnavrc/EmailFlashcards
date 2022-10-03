using System.ComponentModel.DataAnnotations.Schema;

namespace EmailFlashcards.Models
{
    [NotMapped]
    public class EmailSettings
    {
        public string? ContactEmail { get; set; }
        public string? ContactPassword { get; set; }
        public string? ContactClient { get; set; }
        public int ContactPort { get; set; }

    }
}
