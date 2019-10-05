using MarelibuSoft.WebStore.Services;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.TagHelpers
{
	public class MetaDataTagHelperComponent : TagHelperComponent
	{
		private readonly IMetaService metaService;
		public MetaDataTagHelperComponent(IMetaService service)
		{
			metaService = service;
		}

		public override int Order => base.Order;

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (string.Equals(context.TagName, "head", StringComparison.OrdinalIgnoreCase))
			{
				foreach (var item in metaService.GetMetadata())
					output.PostContent.AppendHtml(
						$"<meta name=\"{item.Key}\" content=\"{item.Value}\" /> \r\n");
                foreach (var item in metaService.GetMetadataProperty())
                    output.PostContent.AppendHtml(
                        $"<meta property=\"{item.Key}\" content=\"{item.Value}\" /> \r\n");
            }
		}
	}
}
