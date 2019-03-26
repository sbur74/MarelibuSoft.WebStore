using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using MarelibuSoft.WebStore.Common.ViewModels;
using MarelibuSoft.WebStore.Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Administrator, PowerUser")]
	public class CategoryAssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly ILogger _logger;
		private readonly ILoggerFactory factory;

        public CategoryAssignmentsController(ApplicationDbContext context, ILogger<CategoryAssignmentsController>logger, ILoggerFactory loggerFactory)
        {
            _context = context;
			_logger = logger;
			factory = loggerFactory;

        }

        // GET: Admin/CategoryAssignments
        public async Task<IActionResult> Index()
        {
            var categoryAssignments = await _context.CategoryAssignments.Include(c => c.Product).OrderByDescending(ca => ca.ProductID ).ToListAsync();
			var vms = new List<CategoryAssignmentViewModel>();
			foreach (var item in categoryAssignments)
			{
				try
				{
					var vm = new CategoryAssignmentViewModel
					{
						ID = item.ID,
						ProductName = item.Product.Name,
						ProductNo = item.Product.ProductNumber,
						ProductImage = new ProductImageHelper(_context,factory).GetMainImageUrl(item.ProductID),
						Category = new CategoryHelper(_context).GetNameByID(item.CategoryID),
						CategoryDetail = new CategoryDetailHelper(_context).GetNameByID(item.CategoryDetailID),
						CategorySub = new CategorySubHelper(_context).GetNameByID(item.CategorySubID)
					};
					vms.Add(vm);
				}
				catch (Exception e)
				{
					_logger.LogError(e, "CategoryAssignmentsController.GetIndex--> Fehler beim erstellen");
				}
				
			}
            return View(vms);
        }

        // GET: Admin/CategoryAssignments/Artile/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryAssignment = await _context.CategoryAssignments
                .Include(c => c.Product)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (categoryAssignment == null)
            {
                return NotFound();
            }

            return View(categoryAssignment);
        }

        // GET: Admin/CategoryAssignments/Create
        public IActionResult Create()
        {
			List<SelectItemViewModel> catvms = new CategoryHelper(_context).GetVmList();
			List<SelectItemViewModel> catsubvms = new CategorySubHelper(_context).GetVmList();
			List<SelectItemViewModel> catdeatailvms = new CategoryDetailHelper(_context).GetVmList();

			ViewData["CategoryID"] = new SelectList(catvms, "ID", "Name");
			ViewData["CategorySubID"] = new SelectList(catsubvms, "ID", "Name");
			ViewData["CategoryDetailID"] = new SelectList(catdeatailvms, "ID", "Name");

			ViewData["ProductID"] = new SelectList(_context.Products.OrderByDescending(p => p.ProductNumber), "ProductID", "Name");
            return View();
        }

        // POST: Admin/CategoryAssignments/Create.
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ProductID,CategoryID,CategorySubID,CategoryDetailID")] CategoryAssignment categoryAssignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
			List<SelectItemViewModel> catvms = new CategoryHelper(_context).GetVmList();
			List<SelectItemViewModel> catsubvms = new CategorySubHelper(_context).GetVmList();
			List<SelectItemViewModel> catdeatailvms = new CategoryDetailHelper(_context).GetVmList();

			ViewData["CategoryID"] = new SelectList(catvms, "ID", "Name");
			ViewData["CategorySubID"] = new SelectList(catsubvms, "ID", "Name");
			ViewData["CategoryDetailID"] = new SelectList(catdeatailvms, "ID", "Name");

			ViewData["CheckList"] = new MultiSelectList(catvms, "ID", "Name");

			ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", categoryAssignment.ProductID);
            return View(categoryAssignment);
        }

		
		public IActionResult CreateMulti()
		{
			List<SelectProductViewModel> spvms = new ProductHelper(_context, factory).GetSelectVmList();

			ViewData["ProductID"] = new SelectList( spvms, "ID", "Name");
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateMulti([Bind("ID,ProductID,Categories,CategorySubs,CategoryDetails")] CategoryAssignmentMultiViewModel model)
		{
			if (ModelState.IsValid)
			{
				//todo
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			List<SelectProductViewModel> spvms = new ProductHelper(_context, factory).GetSelectVmList();

			ViewData["ProductID"] = spvms;
			return View(model);
		}


		// GET: Admin/CategoryAssignments/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryAssignment = await _context.CategoryAssignments.SingleOrDefaultAsync(m => m.ID == id);
            if (categoryAssignment == null)
            {
                return NotFound();
            }
			List<SelectItemViewModel> catvms = new CategoryHelper(_context).GetVmList();
			List<SelectItemViewModel> catsubvms = new CategorySubHelper(_context).GetVmList();
			List<SelectItemViewModel> catdeatailvms = new CategoryDetailHelper(_context).GetVmList();

			ViewData["CategoryID"] = new SelectList(catvms, "ID", "Name");
			ViewData["CategorySubID"] = new SelectList(catsubvms, "ID", "Name");
			ViewData["CategoryDetailID"] = new SelectList(catdeatailvms, "ID", "Name");

			ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", categoryAssignment.ProductID);
            return View(categoryAssignment);
        }

        // POST: Admin/CategoryAssignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ProductID,CategoryID,CategorySubID,CategoryDetailID")] CategoryAssignment categoryAssignment)
        {
            if (id != categoryAssignment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryAssignmentExists(categoryAssignment.ID))
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

			List<SelectItemViewModel> catvms = new CategoryHelper(_context).GetVmList();
			List<SelectItemViewModel> catsubvms = new CategorySubHelper(_context).GetVmList();
			List<SelectItemViewModel> catdeatailvms = new CategoryDetailHelper(_context).GetVmList();

			ViewData["CategoryID"] = new SelectList(catvms, "ID", "Name");
			ViewData["CategorySubID"] = new SelectList(catsubvms, "ID", "Name");
			ViewData["CategoryDetailID"] = new SelectList(catdeatailvms, "ID", "Name");

			ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", categoryAssignment.ProductID);
            return View(categoryAssignment);
        }

        // GET: Admin/CategoryAssignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryAssignment = await _context.CategoryAssignments
                .Include(c => c.Product)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (categoryAssignment == null)
            {
                return NotFound();
            }

            return View(categoryAssignment);
        }

        // POST: Admin/CategoryAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryAssignment = await _context.CategoryAssignments.SingleOrDefaultAsync(m => m.ID == id);
            _context.CategoryAssignments.Remove(categoryAssignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryAssignmentExists(int id)
        {
            return _context.CategoryAssignments.Any(e => e.ID == id);
        }
    }
}
