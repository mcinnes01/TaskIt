using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskIt.Data;
using TaskIt.Helpers;
using TaskIt.Models;
using TaskIt.Models.TaskViewModels;

namespace TaskIt.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public TaskController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            object model = await EntityFrameworkQueryableExtensions.ToListAsync<TaskViewModel>(EntityFrameworkQueryableExtensions.Include<TaskViewModel, CategoryViewModel>(_context.Tasks, (TaskViewModel t) => t.Category), default(CancellationToken));
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var task = await _context.Tasks
                               .Include(c => c.Category)
                               .Include(u => u.User)
                               .SingleOrDefaultAsync(m => m.TaskId == id && 
                                                          m.UserName == HttpContext.User.Identity.Name);

            if (task == null)
                return NotFound();

            if (Request.IsAjaxRequest())
                return PartialView("Details", task);

            return View(task);
        }

        public IActionResult Create()
        {
            if (Request.IsAjaxRequest())
                return PartialView("Create");
            
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(new string[] {"TaskId,Body,CategoryId,Created,State,Title,UserName"})] TaskViewModel taskViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                taskViewModel.UserName = user.UserName;
                taskViewModel.User = user;
                
                IQueryable<CategoryViewModel> categories = _context.Categories;
                CategoryViewModel category = await categories.SingleOrDefaultAsync(x => x.CategoryId == taskViewModel.CategoryId);
                taskViewModel.Category = category;

                _context.Add(taskViewModel);
                await _context.SaveChangesAsync();
                RedirectToAction("Index");
            }
            else
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", taskViewModel.CategoryId);
                if (Request.IsAjaxRequest())
                {
                    RedirectToAction("Index", "TaskBoard");
                }
                else
                {
                    View(taskViewModel);
                }
            }
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            
            var task = await _context.Tasks.Include(u => u.User)
                            .Include(c => c.Category)
                            .SingleOrDefaultAsync(m => m.TaskId == id &&
                                                       m.UserName == HttpContext.User.Identity.Name);

            if (task == null)
                return NotFound();

                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", task.CategoryId);

            if (Request.IsAjaxRequest())
                return PartialView("Edit", task);

            return View(task);                        
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, 
            [Bind(new string[] {"TaskId,Body,CategoryId,Created,State,Title,UserName"})] TaskViewModel taskViewModel)
        {
            if (id != taskViewModel.TaskId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var task = await _context.Tasks.Include(c => c.Category)
                                                   .Include(u => u.User)
                                                   .SingleOrDefaultAsync(m => m.TaskId == id &&
                                                                              m.UserName == HttpContext.User.Identity.Name);
                    
                    if (task == null)
                        return NotFound();

                    task.Title = taskViewModel.Title;
                    task.Body = taskViewModel.Body;

                    var category = await _context.Categories.Include(u => u.User)
                                                        .SingleOrDefaultAsync(c => c.CategoryId == taskViewModel.CategoryId &&
                                                                                   c.UserName == HttpContext.User.Identity.Name);

                    task.Category = category;
                    task.State = taskViewModel.State;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskViewModelExists(taskViewModel.TaskId))
                        return NotFound();
                    throw;
                }
                
                return RedirectToAction("Index", "TaskBoard");
            }
            else
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", taskViewModel.CategoryId);
                if (Request.IsAjaxRequest())
                    return PartialView("Edit", taskViewModel);

                return View(taskViewModel);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                NotFound();
            }
            else
            {
                IQueryable<TaskViewModel> tasks = _context.Tasks;
                TaskViewModel taskViewModel = await tasks.SingleOrDefaultAsync(m => m.TaskId == id);
                if (taskViewModel == null)
                {
                    NotFound();
                }
                else if (Request.IsAjaxRequest())
                {
                    PartialView("Delete", taskViewModel);
                }
                else
                {
                    View(taskViewModel);
                }
            }
            return View();
        }

        [ActionName("Delete"), HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            IQueryable<TaskViewModel> tasks = _context.Tasks;
            TaskViewModel taskViewModel = await tasks.SingleOrDefaultAsync(m => m.TaskId == id);
            _context.Tasks.Remove(taskViewModel);
            await _context.SaveChangesAsync();

            if (Request.IsAjaxRequest())
            {
                RedirectToAction("Index", "TaskBoard");
            }
            else
            {
                RedirectToAction("Index");
            }
            return View();
        }

        private bool TaskViewModelExists(int id)
        {
            return _context.Tasks.Any((TaskViewModel e) => e.TaskId == id);
        }
    }
}
