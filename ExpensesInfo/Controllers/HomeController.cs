using System.Diagnostics;
using ExpensesInfo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpensesInfo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ExpensesInfoDbContext _context;
        public HomeController(ILogger<HomeController> logger, ExpensesInfoDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Expenses(int? typeId)
        {
            ViewBag.Types = await _context.ExpenseTypes.ToListAsync();
            ViewBag.SelectedTypeId = typeId;

            var query = _context.Expenses
            .Include(e => e.ExpenseType)
            .AsQueryable();

            if (typeId.HasValue)
            {
                query = query.Where(e => e.ExpenseTypeId == typeId.Value);
            }

            var allExpenses = await query.ToListAsync();
            ViewBag.TotalExpenses = allExpenses.Sum(e => e.Value);

            return View(allExpenses);
        }
        public async Task<IActionResult> CreateEditExpense(int? id)
        {
            ViewBag.Types = await _context.ExpenseTypes.ToListAsync();
           
            if (id == null)
            {
                return View(new Expense());
            }
            var expense = await _context.Expenses.SingleOrDefaultAsync(x => x.Id == id);
            if (expense == null) return NotFound();
            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEditExpenseForm(Expense model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Types = await _context.ExpenseTypes.ToListAsync();
                return View("CreateEditExpense", model);
            }
            if (model.Id == 0)
            {
                if (model.Date==default)
                {
                    model.Date = DateTime.Today;
                }
                _context.Expenses.Add(model);
            }
            else
            {
                var existing = await _context.Expenses.SingleOrDefaultAsync(x => x.Id == model.Id);
                if (existing == null) return NotFound();
                existing.Value = model.Value;
                existing.Description = model.Description;
                existing.Date = model.Date;
                existing.ExpenseTypeId = model.ExpenseTypeId;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Expenses));
        }
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.SingleOrDefaultAsync(x => x.Id == id);
            if (expense == null) return NotFound();
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Expenses));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
