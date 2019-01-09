using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Services
{
	public class MetaService : IMetaService
	{
		private Dictionary<string,string> _metadata;

		public Dictionary<string,string> Metadata
		{
			get { return _metadata; }
			set { _metadata = value; }
		}

		public MetaService()
		{
			_metadata = new Dictionary<string, string>();
			_metadata.Add("description", "Verkauf von Eislaufasseoirs, Stoffen und mehr.");
			_metadata.Add("author", "Petra Buron");
		}

		public MetaService(Dictionary<string,string>meta)
		{
			_metadata = meta;
		}


		public Dictionary<string, string> GetMetadata()
		{
			return _metadata;
		}

		public void AddMetadata(string key, string value)
		{
			if (_metadata.Keys.Contains(key))
			{
				_metadata[key] = value;
			}
			else
			{
				_metadata.Add(key, value);
			}
		}
	}
}
