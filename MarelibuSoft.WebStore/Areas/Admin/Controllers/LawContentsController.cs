using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using MarelibuSoft.WebStore.Enums;

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LawContentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LawContentsController(ApplicationDbContext context)
        {
            _context = context;
        }

		// GET: Admin/LawContents
		public async Task<IActionResult> Index()
		{
			List<LawContent> items = new List<LawContent>();
			items = await _context.LawContents.ToListAsync();

			if (items.Count == 0)
			{
				var agb = new LawContent { ID = 1, HtmlContent = "<strong>AGB hier anpassen</strong>", SiteType = (int)LawContentEnum.TAC, Titel = "AGB" };
				var wbl = new LawContent { ID = 2, HtmlContent = "<strong>Wiederrufsbelerung hier anpassen</strong>", SiteType = (int)LawContentEnum.CAL, Titel = "Wiederrufsbelerung" };
				var dsgvo = new LawContent { ID = 3, HtmlContent = "<strong>Datenschuterklärung hier anpassen</strong>", SiteType = (int)LawContentEnum.PPO, Titel = "Datenschuterklärung" };
				var impressum = new LawContent { ID = 4, HtmlContent = "<strong>Impressum hier anpassen</strong>", SiteType = (int)LawContentEnum.IMP, Titel = "Impressum" };

				_context.Add(agb);
				_context.Add(wbl);
				_context.Add(dsgvo);
				_context.Add(impressum);
				await _context.SaveChangesAsync();

				items = await _context.LawContents.ToListAsync();
			}


			return View(items);
		}

		// GET: Admin/LawContents/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lawContent = await _context.LawContents
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lawContent == null)
            {
                return NotFound();
            }

            return View(lawContent);
        }

        // GET: Admin/LawContents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/LawContents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SiteType,Titel,HtmlContent")] LawContent lawContent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lawContent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lawContent);
        }

        // GET: Admin/LawContents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lawContent = await _context.LawContents.FindAsync(id);
            if (lawContent == null)
            {
                return NotFound();
            }
            return View(lawContent);
        }

        // POST: Admin/LawContents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SiteType,Titel,HtmlContent")] LawContent lawContent)
        {
            if (id != lawContent.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lawContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LawContentExists(lawContent.ID))
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
            return View(lawContent);
        }

        // GET: Admin/LawContents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lawContent = await _context.LawContents
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lawContent == null)
            {
                return NotFound();
            }

            return View(lawContent);
        }

        // POST: Admin/LawContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lawContent = await _context.LawContents.FindAsync(id);
            _context.LawContents.Remove(lawContent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LawContentExists(int id)
        {
            return _context.LawContents.Any(e => e.ID == id);
        }
    }
}
