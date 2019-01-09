using System.Collections.Generic;

namespace MarelibuSoft.WebStore.Services
{
	public interface IMetaService
	{
		Dictionary<string, string> GetMetadata();
		void AddMetadata(string key, string value);
	}
}