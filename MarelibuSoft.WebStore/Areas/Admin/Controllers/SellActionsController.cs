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

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SellActionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SellActionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/SellActions
        public async Task<IActionResult> Index()
        {
            return View(await _context.SellActions.ToListAsync());
        }

        // GET: Admin/SellActions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellAction = await _context.SellActions
                .FirstOrDefaultAsync(m => m.SellActionID == id);
            if (sellAction == null)
            {
                return NotFound();
            }
            var vm = await GetViewModelAsync(sellAction, true);

            return View(vm);
        }

        // GET: Admin/SellActions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/SellActions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SellActionID,ActionName,StartDate,EndDate,Percent,IsActive")] SellAction sellAction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sellAction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sellAction);
        }

        // GET: Admin/SellActions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellAction = await _context.SellActions.FindAsync(id);
            if (sellAction == null)
            {
                return NotFound();
            }

            var sellactionvm = await GetViewModelAsync(sellAction, false);

            return View(sellactionvm);
        }

        // POST: Admin/SellActions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SellActionID,ActionName,StartDate,EndDate,Percent,IsActive")] SellAction sellAction)
        {
            if (id != sellAction.SellActionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sellAction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellActionExists(sellAction.SellActionID))
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
            return View(sellAction);
        }

        // GET: Admin/SellActions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellAction = await _context.SellActions
                .FirstOrDefaultAsync(m => m.SellActionID == id);
            if (sellAction == null)
            {
                return NotFound();
            }

            return View(sellAction);
        }

        // POST: Admin/SellActions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sellAction = await _context.SellActions.FindAsync(id);
            _context.SellActions.Remove(sellAction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<SellActionViewModel> GetViewModelAsync(SellAction sellAction, bool detailview)
        {
            
            var sellactionitems = await _context.SellActionItems.Where(i => i.SellActionID == sellAction.SellActionID).ToListAsync();
            var selectabel = new List<SelectProductViewModel>();
            var selecteditems = new List<SellActionItemViewModel>();

            if (!detailview)
            {
                var products = _context.Products;
                foreach (var item in products)
                {
                    var img = await _context.ProductImages.FirstOrDefaultAsync(i => i.ProductID == item.ProductID && i.IsMainImage);
                    string imgurl = "noImage.svg";
                    if(img != null)
                    {
                        imgurl = img.ImageUrl;
                    }
                    var selectitem = new SelectProductViewModel { ID = item.ProductID, No = item.ProductNumber, Name = item.Name, Img = imgurl };

                    selectabel.Add(selectitem);
                } 
            }

            foreach (var item in sellactionitems)
            {
                var product = await _context.Products.FirstAsync(p => p.ProductID == item.FkProductID);
                var img = await _context.ProductImages.FirstOrDefaultAsync(i => i.ProductID == item.FkProductID && i.IsMainImage);

                string imgurl = "noImage.svg";
                if (img != null)
                {
                    imgurl = img.ImageUrl;
                }

                var vm = new SellActionItemViewModel { SellActionID = item.SellActionID, SellActionItemID = item.SellActionItemID, FkProductID = item.FkProductID, Img = imgurl, Name = product.Name, No = product.ProductNumber };
                selecteditems.Add(vm);
            }
            
            return new SellActionViewModel { ActionName = sellAction.ActionName, EndDate = sellAction.EndDate, StartDate = sellAction.StartDate, IsActive = sellAction.IsActive, Percent = sellAction.Percent, SellActionID = sellAction.SellActionID, SelectedItems = selecteditems, SelectProducts = selectabel };
        }

        private bool SellActionExists(int id)
        {
            return _context.SellActions.Any(e => e.SellActionID == id);
        }
    }
}
