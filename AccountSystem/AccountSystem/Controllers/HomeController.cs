using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AccountSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AccountSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Records
        public async Task<IActionResult> Index(DateTime? dateTime, int? project, int page = 1, SortState sortState = SortState.RecordDateAsc )
        {
            IQueryable<Record> records = _context.Records.Include(r => r.Project);

            // Filtration
            if (dateTime != null)
            {
                records = records.Where(p => p.RecordDate == dateTime);
            }
            if (project != null && project != 0)
            {
                records = records.Where(p => p.ProjectId == project);
            }

            //  Sort
            switch (sortState)
            {
                case SortState.ProjectAsc:
                    records = records.OrderBy(s => s.Project.Name);
                    break;
                case SortState.RecordDateDesc:
                    records = records.OrderByDescending(s => s.RecordDate);
                    break;
                case SortState.ProjectDesc:
                    records = records.OrderByDescending(s => s.Project.Name);
                    break;
                default:
                    records = records.OrderBy(s => s.RecordDate);
                    break;
            }

            //  Pagination 
            int pageSize = 5;
            var count = records.Count();
            var items = await records.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            //  Main model
            IndexViewModel viewModel = new IndexViewModel
            {               
                Records = items,
                SortViewModel = new SortViewModel(sortState),
                FilterViewModel = new FilterViewModel(_context.Projects.ToList(), project, dateTime),
                PageViewModel = new PageViewModel(count,page,pageSize)
            };

            return View(viewModel);
        }

        // GET: Records/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Records
                .Include(r => r.Project)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        // GET: Records/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            return View();
        }

        // POST: Records/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RecordDate,Text,ProjectId")] Record record)
        {
            if (ModelState.IsValid)
            {
                _context.Add(record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", record.ProjectId);
            return View(record);
        }

        // GET: Records/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Records.SingleOrDefaultAsync(m => m.Id == id);
            if (record == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", record.ProjectId);
            return View(record);
        }

        // POST: Records/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RecordDate,Text,ProjectId")] Record record)
        {
            if (id != record.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(record);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordExists(record.Id))
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
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", record.ProjectId);
            return View(record);
        }

        // GET: Records/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Records
                .Include(r => r.Project)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        // POST: Records/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var record = await _context.Records.SingleOrDefaultAsync(m => m.Id == id);
            _context.Records.Remove(record);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordExists(int id)
        {
            return _context.Records.Any(e => e.Id == id);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contacts";

            return View();
        }
    }
}
