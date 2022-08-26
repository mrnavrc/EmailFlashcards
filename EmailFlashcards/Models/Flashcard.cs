using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EmailFlashcards.Data.Enum;

namespace EmailFlashcards.Models
{
    public class Flashcard
    {
        [Key]
        public int Flashcard_Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Text { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created")]
        public DateTime? CardCreatedDate { get; set; }

        // Enum
        public FlashcardsCategory FlashcardsCategory { get; set; }
        
        // Virtuals FK
        public virtual User? User { get; set; }

    }
}
