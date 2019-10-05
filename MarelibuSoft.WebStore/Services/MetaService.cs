using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Services
{
	public class MetaService : IMetaService
	{
		private Dictionary<string,string> _metadata;
        private Dictionary<string, string> _metadataProperty;

		public Dictionary<string,string> Metadata
		{
			get { return _metadata; }
			set { _metadata = value; }
		}

        public Dictionary<string, string> MetadataProperty
        {
            get
            {
                return _metadataProperty;
            }
            set
            {
                _metadataProperty = value;
            }
        }

        public MetaService()
		{
			_metadata = new Dictionary<string, string>();
			_metadata.Add("description", "Verkauf von Eislaufasseoirs, Stoffen und mehr.");
			_metadata.Add("author", "Petra Buron");

            _metadataProperty = new Dictionary<string, string>();
		}

		public MetaService(Dictionary<string,string>meta, Dictionary<string,string>propertyMeta)
		{
			_metadata = meta;
            _metadataProperty = propertyMeta;
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

        public Dictionary<string, string> GetMetadataProperty()
        {
            return _metadataProperty;
        }

        public void AddMetadataProperty(string key, string value)
        {
            if (_metadataProperty.Keys.Contains(key))
            {
                _metadataProperty[key] = value;
            }
            else
            {
                _metadataProperty.Add(key, value);
            }
        }
    }
}
