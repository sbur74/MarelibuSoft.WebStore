using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Enums
{
	public enum CmsPageContentTypeEnum
	{
		[Display(Name = "Bild")]
		Image,
		Video,
		[Display(Name = "Dokument")]
		Document
	}
}
