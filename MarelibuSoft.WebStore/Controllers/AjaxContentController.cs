using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarelibuSoft.WebStore.Controllers
{
    public class AjaxContentController : Controller
    {
        // GET: /<controller>/
        public IActionResult ShoppingCart()
        {
			return ViewComponent("ShoppingCart");
        }
    }
}
