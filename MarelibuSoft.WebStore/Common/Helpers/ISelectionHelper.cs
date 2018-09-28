using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarelibuSoft.WebStore.Common.ViewModels;

namespace MarelibuSoft.WebStore.Common.Helpers
{
    interface ISelectionHelper
    {
		List<SelectItemViewModel> GetVmList();
		string GetNameByID(int id);
    }
}
