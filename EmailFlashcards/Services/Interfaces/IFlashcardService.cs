using EmailFlashcards.Models;

namespace EmailFlashcards.Services.Interfaces
{
    public interface IFlashcardService
    {
        Task AddFlashcardToCategoryAsync(int categoryId, int flashcardId);
        Task RemoveFlashcardFromCategoryAsync(int categoryId, int flashcardId);
        Task<bool> IsFlashcardInCategory(int categoryId, int flashcardId);
        Task<IEnumerable<Category>> GetUserCategoriesAsync(string userId);
        Task<IEnumerable<Category>> GetFlashcardCategoriesAsync(int flashcardId);
        Task<ICollection<int>> GetFlashcardCategoryIdAsync(int flashcardId);
        IEnumerable<Flashcard> SearchForFlashcard(string searchString, string userId);
    }
}
