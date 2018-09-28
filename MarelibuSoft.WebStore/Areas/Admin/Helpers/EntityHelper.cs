using MarelibuSoft.WebStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Helpers
{
    public class EntityHelper
    {
		private readonly ApplicationDbContext _context;

		public EntityHelper(ApplicationDbContext context)
		{
			_context = context;
		}

		public string GetNameByID<T>(int id)
		{
			string name = string.Empty;

			if (id > 0)
			{
				
			}
			return name;
		}
    }
}
