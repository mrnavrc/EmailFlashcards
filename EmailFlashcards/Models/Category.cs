using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace EmailFlashcards.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        [Required]
        [Display(Name = "Flashcard Category Name")]
        public string? FlashcardsCategoryName { get; set; }

        //Virtuals
        public virtual User? User { get; set; }
        public virtual ICollection<Flashcard> Flashcards { get; set; } = new HashSet<Flashcard>();

    }
}
