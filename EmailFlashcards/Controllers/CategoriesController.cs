using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmailFlashcards.Data;
using EmailFlashcards.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using System.Data;
using System.Xml.Linq;

namespace EmailFlashcards.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CategoriesController(ApplicationDbContext context,
                                    UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Categories
        [Authorize]
        public IActionResult Index(string SuccessMessage = null, string DeleteAction = null)
        {
            ViewData["DeleteAction"] = DeleteAction;
            ViewData["SuccessMessage"] = SuccessMessage;
            string userId = _userManager.GetUserId(User);
            var categories = _context.Categories.Where(categories => categories.UserId == userId)
                                                .ToList();
            return View(categories);
        }

        // GET: Categories/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("CategoryId,FlashcardCategoryName")] Category category)
        {
            List<string?> NamesOfCategories = await _context.Categories.Select(categories => categories.FlashcardCategoryName).ToListAsync();

            foreach (var name in NamesOfCategories)
            {
                if (category.FlashcardCategoryName == name)
                {
                    ViewData["DuplicityMessage"] = "DuplicityMessage";
                    return View();
                }
            }

            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {

                category.UserId = _userManager.GetUserId(User);

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { SuccessMessage = "succes" });
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", category.UserId);
            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            string userId = _userManager.GetUserId(User);
            var category = _context.Categories.Where(category => category.UserId == userId && category.CategoryId == id)
                                        .FirstOrDefault();


            if (category == null)
            {
                return NotFound();
            }


            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,UserId,FlashcardCategoryName")] Category category)
        {

            List<string?> NamesOfCategories = await _context.Categories.Select(categories => categories.FlashcardCategoryName).ToListAsync();

            foreach (var name in NamesOfCategories)
            {
                if (category.FlashcardCategoryName == name)
                {
                    ViewData["DuplicityMessage"] = "DuplicityMessage";
                    return View();
                }
            }
           

            ModelState.Remove("UserId");
            if (ModelState.IsValid) {
               
                    category.UserId = _userManager.GetUserId(User);

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { SuccessMessage = "succes" });
            }

            if (id != category.CategoryId)
            {
                return NotFound();
            }
 
            return View(category);


        }

        // GET: Categories/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            string userId = _userManager.GetUserId(User);
            var category = await _context.Categories
                                         .FirstOrDefaultAsync(categories => categories.CategoryId == id && categories.UserId == userId);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            string userId = _userManager.GetUserId(User);
            var category = await _context.Categories.FirstOrDefaultAsync(categories => categories.CategoryId == id && categories.UserId == userId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), new { DeleteAction = "Delete" });
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
