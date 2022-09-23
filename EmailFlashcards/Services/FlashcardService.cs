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
  
        public async Task<IEnumerable<Category>> GetFlashcardCategoriesAsync(int flashcardId)
        {
            try
            {
                Flashcard? flashcard = await _context.Flashcards.Include(f => f.Categories).FirstOrDefaultAsync(c => c.FlashcardId == flashcardId);
                return flashcard.Categories;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<int>> GetFlashcardCategoryIdAsync(int flashcardId)
        {
            try
            {
                var flashcard = await _context.Flashcards.Include(f => f.Categories)
                                                     .FirstOrDefaultAsync(c => c.FlashcardId == flashcardId);
                List<int> categoryIds = flashcard.Categories.Select(c => c.CategoryId).ToList();
                return categoryIds;
            }
            catch (Exception)
            {
                throw;
            }
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

        public async Task RemoveFlashcardFromCategoryAsync(int categoryId, int flashcardId)
        {
            try
            {
                if (await IsFlashcardInCategory(categoryId, flashcardId))
                {
                    Flashcard flashcard = await _context.Flashcards.FindAsync(flashcardId);
                    Category category = await _context.Categories.FindAsync(categoryId);

                    if (category != null && flashcard != null)
                    {
                        category.Flashcards.Remove(flashcard);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public IEnumerable<Flashcard> SearchForFlashcard(string searchString, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
