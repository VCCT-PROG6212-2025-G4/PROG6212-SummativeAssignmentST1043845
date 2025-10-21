using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMCS_Web_App.Models;
using CMCS_Web_App.Data;

namespace CMCS_Web_App.Controllers
{
    public class ManagerController : Controller
    {
        private readonly AppDbContext _context;

        public ManagerController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() =>
            View(await _context.Managers.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var manager = await _context.Managers.FirstOrDefaultAsync(m => m.ManagerId == id);
            if (manager == null) return NotFound();
            return View(manager);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Manager manager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manager);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var manager = await _context.Managers.FindAsync(id);
            if (manager == null) return NotFound();
            return View(manager);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Manager manager)
        {
            if (id != manager.ManagerId) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Managers.Any(e => e.ManagerId == id))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(manager);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var manager = await _context.Managers.FirstOrDefaultAsync(m => m.ManagerId == id);
            if (manager == null) return NotFound();
            return View(manager);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manager = await _context.Managers.FindAsync(id);
            if (manager != null)
            {
                _context.Managers.Remove(manager);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

