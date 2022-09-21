using System.ComponentModel.DataAnnotations;

namespace EmailFlashcards.Models
{
    public class FlashcardSetting
    {
        [Key]
        public int FlashcardSettingsId { get; set; }
        public string? FlashcardEmailAdress { get; set; }
        public int FlashcardsPerDay { get; set; } = 5;
        public TimeOnly Time { get; set; } = new TimeOnly(09, 00);

        public string? UserId { get; set; }
    }
}
