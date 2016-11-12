using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TaskIt.Data;
using TaskIt.Helpers;
using TaskIt.Models;
using TaskIt.Models.TaskViewModels;

namespace TaskIt.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            object model = await EntityFrameworkQueryableExtensions.ToListAsync<CategoryViewModel>(_context.Categories, default(CancellationToken));
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories.Include(t => t.Tasks)
                                              .SingleOrDefaultAsync(m => m.CategoryId == id &&
                                                                         m.UserName == HttpContext.User.Identity.Name);
            
            if (category == null)
                return NotFound();

            if (Request.IsAjaxRequest())
                PartialView("Details", category);

            return View(category);
        }

        public IActionResult Create()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("Create");
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(new string[] {"CategoryId","Colour","Title","UserName"})] CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                category.User = user;
                category.UserName = user.UserName;
                _context.Add(category);
                await _context.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                    return RedirectToAction("Index", "TaskBoard");

                return RedirectToAction("Index");
            }

            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories.Include(u => u.User)
                                                    .SingleOrDefaultAsync(m => m.CategoryId == id &&
                                                                               m.UserName == HttpContext.User.Identity.Name);
            if (category == null)
                return NotFound();

            if (Request.IsAjaxRequest())
                return PartialView("Edit", category);

                return View(category);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(new string[]
        {"CategoryId","Title","Colour","Tasks","UserName","User"})] CategoryViewModel category)
        {
            if (id != category.CategoryId)
                return NotFound();
            
            if (ModelState.IsValid)
            {
                try
                {
                    var original = await _context.Categories
                                                 .Include(u => u.User)
                                                 .SingleOrDefaultAsync(m => m.CategoryId == id &&
                                                                            m.UserName == HttpContext.User.Identity.Name);
                    if (original != null)
                    {
                        original.Title = category.Title;
                        original.Colour = category.Colour;
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                        return NotFound();
                        
                    throw;
                }

                if (Request.IsAjaxRequest())
                    return RedirectToAction("Index", "TaskBoard");

                return RedirectToAction("Index");
            }

            if (Request.IsAjaxRequest())
                return PartialView("Edit", category);

            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories
                                         .SingleOrDefaultAsync(m => m.CategoryId == id &&
                                                                    m.UserName == HttpContext.User.Identity.Name);

            if (category == null)
                return NotFound();

            if (Request.IsAjaxRequest())
                return PartialView("Delete", category);

            return View(category);
        }

        [ActionName("Delete"), HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories
                                         .SingleOrDefaultAsync(m => m.CategoryId == id &&
                                                                    m.UserName == HttpContext.User.Identity.Name);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            
            if (Request.IsAjaxRequest())
                return RedirectToAction("Index", "TaskBoard");

            return RedirectToAction("Index");
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any((CategoryViewModel e) => e.CategoryId == id);
        }
    }
}
