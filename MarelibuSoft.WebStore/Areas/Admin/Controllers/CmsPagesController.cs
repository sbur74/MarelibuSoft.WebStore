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
    public class CmsPagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CmsPagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CmsPages
        public async Task<IActionResult> Index()
        {
            return View(await _context.CmsPages.ToListAsync());
        }

        // GET: Admin/CmsPages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cmsPage = await _context.CmsPages
                .FirstOrDefaultAsync(m => m.Id == id);
			var draft = await _context.CmsPageDrafts.FirstOrDefaultAsync(d => d.DraftOfPageId == id);

			if (cmsPage == null)
            {
                return NotFound();
            }

			CmsPageWorkViewModel viewModel;

			if(draft != null)
			{
				viewModel = new CmsPageWorkViewModel
				{
					Id = draft.Id,
					DraftOfPageId = draft.DraftOfPageId,
					LastChange = draft.LastChange,
					LayoutEnum = draft.LayoutEnum,
					Name = draft.Name,
					PageContent = draft.PageContent,
					PageDesciption = draft.PageDesciption,
					PageEnum = draft.PageEnum,
					SeoDescription = draft.SeoDescription,
					SeoKeywords = draft.SeoKeywords,
					StatusEnum = draft.StatusEnum,
					Titel = draft.Titel
				};
			}
			else
			{
				viewModel = new CmsPageWorkViewModel
				{
					Id = cmsPage.Id,
					DraftOfPageId = 0,
					LastChange = cmsPage.LastChange,
					LayoutEnum = cmsPage.LayoutEnum,
					Name = cmsPage.Name,
					PageContent = cmsPage.PageContent,
					PageDesciption = cmsPage.PageDesciption,
					PageEnum = cmsPage.PageEnum,
					SeoDescription = cmsPage.SeoDescription,
					SeoKeywords = cmsPage.SeoKeywords,
					StatusEnum = cmsPage.StatusEnum,
					Titel = cmsPage.Titel
				};
			}


            return View(viewModel);
        }

        // GET: Admin/CmsPages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CmsPages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Titel,PageHeadline,PageContent,PageEnum,LayoutEnum,StatusEnum")] CmsPage cmsPage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cmsPage);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cmsPage);
        }

        // GET: Admin/CmsPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cmsPage = await _context.CmsPages.FindAsync(id);
	
            if (cmsPage == null)
            {
                return NotFound();
            }

			var draft = await _context.CmsPageDrafts.FirstOrDefaultAsync(c => c.DraftOfPageId == cmsPage.Id);
			CmsPageWorkViewModel viewModel;

			if (draft == null)
			{
				CmsPageDraft cmsPageDraft = new CmsPageDraft { DraftOfPageId = cmsPage.Id, LastChange = DateTime.Now, LayoutEnum = cmsPage.LayoutEnum, Name = cmsPage.Name, PageContent = cmsPage.PageContent, PageDesciption = cmsPage.PageDesciption, PageEnum = cmsPage.PageEnum, SeoDescription = cmsPage.SeoDescription, SeoKeywords = cmsPage.SeoKeywords, StatusEnum = cmsPage.StatusEnum, Titel = cmsPage.Titel };

				_context.CmsPageDrafts.Add(cmsPageDraft);
				await _context.SaveChangesAsync();

				draft = await _context.CmsPageDrafts.FirstOrDefaultAsync(c => c.DraftOfPageId == cmsPage.Id);
			}

			viewModel = new CmsPageWorkViewModel
			{
				Id = draft.Id,
				DraftOfPageId = draft.DraftOfPageId,
				LastChange = draft.LastChange,
				LayoutEnum = draft.LayoutEnum,
				Name = draft.Name,
				PageContent = draft.PageContent,
				PageDesciption = draft.PageDesciption,
				PageEnum = draft.PageEnum,
				SeoDescription = draft.SeoDescription,
				SeoKeywords = draft.SeoKeywords,
				StatusEnum = draft.StatusEnum,
				Titel = draft.Titel
			};

			return View(viewModel);
        }

        // POST: Admin/CmsPages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DraftOfPageId,Name,Titel,PageHeadline,PageContent,PageEnum,LayoutEnum,StatusEnum")] CmsPageWorkViewModel work)
        {
            if (id != work.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					var cms = _context.CmsPages.Find(work.DraftOfPageId);
					var draft = _context.CmsPageDrafts.Find(work.Id);

					if(cms == null)
					{
						return NotFound();
					}

					if (work.StatusEnum == Enums.CmsPageStatusEnum.Published)
					{
						

						if(cms != null)
						{
							cms.LastChange = DateTime.Now;
							cms.LayoutEnum = work.LayoutEnum;
							cms.Name = work.Name;
							cms.PageContent = work.PageContent;
							cms.PageDesciption = work.PageDesciption;
							cms.PageEnum = work.PageEnum;
							cms.SeoDescription = work.SeoDescription;
							cms.SeoKeywords = work.SeoKeywords;
							cms.StatusEnum = work.StatusEnum;
							cms.Titel = work.Titel;
							_context.Update(cms);
						}
					}

					if (draft != null)
					{
						draft.LastChange = DateTime.Now;
						draft.LayoutEnum = work.LayoutEnum;
						draft.Name = work.Name;
						draft.PageContent = work.PageContent;
						draft.PageDesciption = work.PageDesciption;
						draft.PageEnum = work.PageEnum;
						draft.SeoDescription = work.SeoDescription;
						draft.SeoKeywords = work.SeoKeywords;
						draft.StatusEnum = work.StatusEnum;
						draft.Titel = work.Titel;
						_context.Update(draft);
					}
					else
					{
						draft = new CmsPageDraft
						{
							DraftOfPageId = work.DraftOfPageId,
							LastChange = DateTime.Now,
							LayoutEnum = work.LayoutEnum,
							Name = work.Name,
							PageContent = work.PageContent,
							PageDesciption = work.PageDesciption,
							PageEnum = work.PageEnum,
							SeoDescription = work.SeoDescription,
							SeoKeywords = work.SeoKeywords,
							StatusEnum = work.StatusEnum,
							Titel = work.Titel
						};
						_context.Add(draft);
					}
					await _context.SaveChangesAsync();
				}
                catch (DbUpdateConcurrencyException e)
                {
					Console.WriteLine($"CmsPageController.Edit -> Fehler bei Datenbanktransaktion!\n{e}");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(work);
        }

        // GET: Admin/CmsPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cmsPage = await _context.CmsPages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cmsPage == null)
            {
                return NotFound();
            }

            return View(cmsPage);
        }

        // POST: Admin/CmsPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cmsPage = await _context.CmsPages.FindAsync(id);
            _context.CmsPages.Remove(cmsPage);
			var draft = await _context.CmsPageDrafts.FirstOrDefaultAsync(d => d.DraftOfPageId == id);
			_context.CmsPageDrafts.Remove(draft);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CmsPageExists(int id)
        {
            return _context.CmsPages.Any(e => e.Id == id);
        }



    }
}
