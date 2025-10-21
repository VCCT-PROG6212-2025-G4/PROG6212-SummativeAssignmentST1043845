using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMCS_Web_App.Models;
using CMCS_Web_App.Data;

namespace CMCS_Web_App.Controllers
{
    public class CoordinatorController : Controller
    {
        private readonly AppDbContext _context;

        public CoordinatorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() =>
            View(await _context.Coordinators.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var coordinator = await _context.Coordinators.FirstOrDefaultAsync(m => m.CoordinatorId == id);
            if (coordinator == null) return NotFound();
            return View(coordinator);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coordinator coordinator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coordinator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coordinator);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var coordinator = await _context.Coordinators.FindAsync(id);
            if (coordinator == null) return NotFound();
            return View(coordinator);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Coordinator coordinator)
        {
            if (id != coordinator.CoordinatorId) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coordinator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Coordinators.Any(e => e.CoordinatorId == id))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(coordinator);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var coordinator = await _context.Coordinators.FirstOrDefaultAsync(m => m.CoordinatorId == id);
            if (coordinator == null) return NotFound();
            return View(coordinator);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coordinator = await _context.Coordinators.FindAsync(id);
            if (coordinator != null)
            {
                _context.Coordinators.Remove(coordinator);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

