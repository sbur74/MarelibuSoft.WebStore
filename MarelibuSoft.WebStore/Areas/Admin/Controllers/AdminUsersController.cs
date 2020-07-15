using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarelibuSoft.WebStore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
    public class AdminUsersController : Controller
    {
		private readonly ApplicationDbContext context;
		private readonly ILoggerFactory factory;
		private readonly ILogger logger;

		public AdminUsersController(ApplicationDbContext dbContext
            //, ILoggerFactory loggerFactory
            )
		{
			context = dbContext;
			//factory = loggerFactory;
			//logger = factory.CreateLogger<AdminUsersController>();
		}

        // GET: AdminUserManager
        public ActionResult Index()
        {
			var users =  context.Users.ToList();
            return View(users);
        }

        // GET: AdminUserManager/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminUserManager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminUserManager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminUserManager/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminUserManager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminUserManager/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminUserManager/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}