using System.Collections.Generic;

namespace MarelibuSoft.WebStore.Services
{
	public interface IMetaService
	{
		Dictionary<string, string> GetMetadata();
        Dictionary<string, string> GetMetadataProperty();
		void AddMetadata(string key, string value);
        void AddMetadataProperty(string key, string value);
	}
}