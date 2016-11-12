using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TaskIt.Data;
using TaskIt.Models;
using TaskIt.Models.TaskViewModels;

namespace TaskIt.Controllers
{
    public class TaskBoardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskBoardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            IQueryable<CategoryViewModel> userData = from c in _context.Categories
                                                                       .Include(t => t.Tasks)
                                                                       .Include(u => u.User)
                                                     where c.UserName == this.HttpContext.User.Identity.Name
                                                     select c;
          return View(userData);
        }
    }
}
