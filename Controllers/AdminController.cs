using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _7semester_ASP_FirstTask.Models;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace _7semester_ASP_FirstTask.Controllers
{
	public class AdminController : Controller
	{
		private readonly ApplicationContext _context;

		public AdminController(ApplicationContext context)
		{
			_context = context;

		}
		public IActionResult CheckIfAdmin()
		{
			//if ()
			return RedirectToAction(nameof(UserIndex));
		}

		// GET: Admin
		public async Task<IActionResult> UserIndex()
		{
			return View(await _context.Users.ToListAsync());
		}

		// GET: Admin/UserDetails/5
		public async Task<IActionResult> UserDetails(int? id)
		{
			if (id == null || _context.Users == null)
			{
				return NotFound();
			}

			var user = await _context.Users
				.FirstOrDefaultAsync(m => m.Id == id);
			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		// GET: Admin/UserCreate
		public IActionResult UserCreate()
		{
			return View();
		}

		// POST: Admin/UserCreate
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UserCreate([Bind("Id,Name,login,password,Country")] User user)
		{
			if (ModelState.IsValid)
			{
				_context.Add(user);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(UserIndex));
			}
			return View(user);
		}

		// GET: Admin/UserEdit/5
		public async Task<IActionResult> UserEdit(int? id)
		{
			if (id == null || _context.Users == null)
			{
				return NotFound();
			}

			var user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			return View(user);
		}

		// POST: Admin/UserEdit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UserEdit(int id, [Bind("Id,Name,login,password,Country")] User user)
		{
			if (id != user.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(user);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!UserExists(user.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(UserIndex));
			}
			return View(user);
		}

		// GET: Admin/UserDelete/5
		public async Task<IActionResult> UserDelete(int? id)
		{
			if (id == null || _context.Users == null)
			{
				return NotFound();
			}

			var user = await _context.Users
				.FirstOrDefaultAsync(m => m.Id == id);
			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		// POST: Admin/UserDelete/5
		[HttpPost, ActionName("UserDelete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Users == null)
			{
				return Problem("Entity set 'ApplicationContext.Users'  is null.");
			}
			var user = await _context.Users.FindAsync(id);
			if (user != null)
			{
				_context.Users.Remove(user);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(UserIndex));
		}

		private bool UserExists(int id)
		{
			return _context.Users.Any(e => e.Id == id);
		}

		public async Task<IActionResult> Bagrains(string skinTone, int master = 0, int page = 1,
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
	}
}
