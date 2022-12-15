using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _7semester_ASP_FirstTask.Models;

namespace _7semester_ASP_FirstTask.Controllers
{
	public class AdminBagrainsController : Controller
	{
		private readonly ApplicationContext _context;

		public AdminBagrainsController(ApplicationContext context)
		{
			_context = context;
		}

		// GET: AdminBagrains
		public async Task<IActionResult> Index(string skinTone, int master = 0, int page = 1,
			SortState sortOrder = SortState.SkinToneAsc)
		{
			int pageSize = 3;

			//фильтрация
			IQueryable<Slave> slaves = _context.Slaves.Include(x => x.Master);

			if (master != 0)
			{
				slaves = slaves.Where(p => p.MasterId == master);
			}
			if (!string.IsNullOrEmpty(skinTone))
			{
				slaves = slaves.Where(p => p.SkinTone == skinTone);
			}

			// сортировка
			switch (sortOrder)
			{
				case SortState.SkinToneDesc:
					slaves = slaves.OrderByDescending(s => s.SkinTone);
					break;
				case SortState.AgeAsc:
					slaves = slaves.OrderBy(s => s.Age);
					break;
				case SortState.AgeDesc:
					slaves = slaves.OrderByDescending(s => s.Age);
					break;
				case SortState.MasterAsc:
					slaves = slaves.OrderBy(s => s.Master!.Name);
					break;
				case SortState.MasterDesc:
					slaves = slaves.OrderByDescending(s => s.Master!.Name);
					break;
				default:
					slaves = slaves.OrderBy(s => s.SkinTone);
					break;
			}

			// пагинация
			var count = await slaves.CountAsync();
			var items = await slaves.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

			// формируем модель представления
			BagrainsViewModel viewModel = new BagrainsViewModel(
			items,
				new PageViewModel(count, page, pageSize),
				new FilterViewModel(await _context.Masters.ToListAsync(), master, skinTone),
				new SortViewModel(sortOrder)
			);
			return View(viewModel);
		}

		// GET: AdminBagrains/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Slaves == null)
			{
				return NotFound();
			}

			var slave = await _context.Slaves
				.Include(s => s.Master)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (slave == null)
			{
				return NotFound();
			}

			return View(slave);
		}

		// GET: AdminBagrains/Create
		public IActionResult Create()
		{
			ViewData["MasterId"] = new SelectList(_context.Masters, "Id", "Name");
			return View();
		}

		// POST: AdminBagrains/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Price,Age,SkinTone,MasterId")] Slave slave)
		{
			if (ModelState.IsValid)
			{
				_context.Add(slave);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["MasterId"] = new SelectList(_context.Masters, "Id", "Id", slave.MasterId);
			return View(slave);
		}

		// GET: AdminBagrains/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Slaves == null)
			{
				return NotFound();
			}

			var slave = await _context.Slaves.FindAsync(id);
			if (slave == null)
			{
				return NotFound();
			}
			ViewData["MasterId"] = new SelectList(_context.Masters, "Id", "Name", slave.MasterId);
			return View(slave);
		}

		// POST: AdminBagrains/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Price,Age,SkinTone,MasterId")] Slave slave)
		{
			if (id != slave.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(slave);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!SlaveExists(slave.Id))
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
			ViewData["MasterId"] = new SelectList(_context.Masters, "Id", "Id", slave.MasterId);
			return View(slave);
		}

		// GET: AdminBagrains/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Slaves == null)
			{
				return NotFound();
			}

			var slave = await _context.Slaves
				.Include(s => s.Master)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (slave == null)
			{
				return NotFound();
			}

			return View(slave);
		}

		// POST: AdminBagrains/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Slaves == null)
			{
				return Problem("Entity set 'ApplicationContext.Slaves'  is null.");
			}
			var slave = await _context.Slaves.FindAsync(id);
			if (slave != null)
			{
				_context.Slaves.Remove(slave);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool SlaveExists(int id)
		{
			return _context.Slaves.Any(e => e.Id == id);
		}
	}
}
