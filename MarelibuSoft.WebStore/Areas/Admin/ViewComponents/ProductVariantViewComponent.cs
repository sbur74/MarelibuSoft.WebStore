using MarelibuSoft.WebStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.ViewComponents
{
    public class ProductVariantViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext context;
        public ProductVariantViewComponent(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var variants = await context.VariantTemplates.Include(o => o.VariantOptionTemplates).ToListAsync();
            return View(variants);
        }
    }
}
