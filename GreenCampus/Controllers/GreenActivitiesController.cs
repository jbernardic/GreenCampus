using GreenCampus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenCampus.Controllers
{
    public class GreenActivitiesController : Controller
    {
        private readonly DatabaseContext _context;

        public GreenActivitiesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: GreenActivities
        public async Task<IActionResult> Index()
        {
            var activities = await _context.GreenActivities.ToListAsync();
            return View(activities);
        }
        private async Task<IActionResult> ActivityViewOrNotFoundAsync(int? id, string viewName)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.GreenActivities
                .FirstOrDefaultAsync(a => a.GreenActivityId == id);

            if (activity == null)
            {
                return NotFound();
            }

            return View(viewName, activity);
        }


        // GET: GreenActivities/Details/5
        public Task<IActionResult> Details(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Task.FromResult<IActionResult>(BadRequest(ModelState));
            }

            return ActivityViewOrNotFoundAsync(id, "Details");
        }



        // GET: GreenActivities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GreenActivities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] GreenActivity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        // GET: GreenActivities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.GreenActivities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }


        // POST: GreenActivities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GreenActivityId,Name,Description")] GreenActivity activity)
        {
            if (id != activity.GreenActivityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.GreenActivityId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        // GET: GreenActivities/Delete/5
        public Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Task.FromResult<IActionResult>(BadRequest(ModelState));
            }

            return ActivityViewOrNotFoundAsync(id, "Delete");
        }



        // POST: GreenActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activity = await _context.GreenActivities.FindAsync(id);
            if (activity != null)
            {
                _context.GreenActivities.Remove(activity);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool ActivityExists(int id)
        {
            return _context.GreenActivities.Any(e => e.GreenActivityId == id);
        }
    }
}
