using Microsoft.AspNetCore.Mvc;

namespace EmailFlashcards.Models
{
    public class FlashcardCsvResult : FileResult
    {
        private readonly IEnumerable<Flashcard> _flashcardList;
        public FlashcardCsvResult(IEnumerable<Flashcard> flashcardList, string fileDownloadName) : base("text/csv")
        {
            _flashcardList = flashcardList;
            FileDownloadName = fileDownloadName;
        }

        public async override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            context.HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + FileDownloadName });

            using (var streamWriter = new StreamWriter(response.Body))
            {
                await streamWriter.WriteLineAsync(
                  $"FlashcardTitle, FlashcardText, Created"
                );
                foreach (var p in _flashcardList)
                {
                    await streamWriter.WriteLineAsync(
                      $"{p.FlashcardTitle}, {p.FlashcardText}, {p.FlashcardCreatedDate}"
                    );
                    await streamWriter.FlushAsync();
                }
                await streamWriter.FlushAsync();
            }
        }

    }
}
