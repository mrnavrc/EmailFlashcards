using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace EmailFlashcards.Models
{
    public class Flashcard
    {
        [Key]
        public int FlashcardId { get; set; }

        public string? UserId { get; set; }


        [Required]
        [Display(Name = "Name of your flashcard")]
        public string? FlashcardTitle { get; set; }

        [Required]
        public string? FlashcardText { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created")]
        public DateTime? FlashcardCreatedDate { get; set; }
        public virtual ICollection<Category>? Categories { get; set; } = new HashSet<Category>();
    }
}
