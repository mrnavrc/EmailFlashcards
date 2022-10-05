using System.ComponentModel.DataAnnotations;


namespace EmailFlashcards.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        [Display(Name = "Flashcard Category Name")]
        public string? FlashcardCategoryName { get; set; }

        public virtual ICollection<Flashcard>? Flashcards { get; set; } = new HashSet<Flashcard>();

    }
}
