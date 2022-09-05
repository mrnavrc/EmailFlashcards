using EmailFlashcards.Data;
using EmailFlashcards.Models;
using EmailFlashcards.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmailFlashcards.Services
{
    public class FlashcardService : IFlashcardService
    {
        private readonly ApplicationDbContext _context;

        public FlashcardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddFlashcardToCategoryAsync(int categoryId, int flashcardId)
        {
            try
            {
                //check to see if the category is in flashcard already
                if (!await IsFlashcardInCategory(categoryId, flashcardId))
                {
                    Flashcard? flashcard = await _context.Flashcards.FindAsync(flashcardId);
                    Category? category = await _context.Categories.FindAsync(categoryId);

                        if(category != null && flashcard != null)
                    {
                        category.Flashcards.Add(flashcard);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
  
        public Task<IEnumerable<Category>> GetFlashcardCategoriesAsync(int flashcardId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<int>> GetFlashcardCategoryIdAsync(int flashcardId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetUserCategoriesAsync(string userId)
        {
            List<Category> categoryList = new List<Category>();
            try
            {
                categoryList = await _context.Categories.Where(c => c.UserId == userId)
                                                        .OrderBy(c => c.FlashcardCategoryName)
                                                        .ToListAsync();
            }
            catch
            {
                throw;
            }
            return categoryList;
        }

        public async Task<bool> IsFlashcardInCategory(int categoryId, int flashcardId)
        {
            Flashcard? flashcard = await _context.Flashcards.FindAsync(flashcardId);
            return await _context.Categories.Include(c => c.Flashcards)
                                            .Where(c => c.CategoryId == categoryId && c.Flashcards.Contains(flashcard))
                                            .AnyAsync();
        }

        public Task RemoveFlashcardFromCategoryAsync(int categoryId, int flashcardId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Flashcard> SearchForFlashcard(string searchString, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
