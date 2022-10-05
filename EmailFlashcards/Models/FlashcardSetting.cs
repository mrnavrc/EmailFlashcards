using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmailFlashcards.Models
{
    public class FlashcardSetting
    {
        [Key]
        public int FlashcardSettingsId { get; set; }

        public string? FlashcardEmailAdress { get; set; }

        public int FlashcardsPerDay { get; set; } = 5;

        [NotMapped]
        public string Hour { get; set; }

        public TimeOnly Time { get; set; }

        public string? UserId { get; set; }

        public User? User { get; set; }

    }
}
