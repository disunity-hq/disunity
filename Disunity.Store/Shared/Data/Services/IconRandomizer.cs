using System.Collections.Generic;
using System.IO;
using System.Linq;

using BindingAttributes;

using Microsoft.Extensions.Configuration;

using Tracery;


namespace Disunity.Store.Data.Services {

    [AsSingleton]
    public class IconRandomizer {

        private readonly IConfiguration _config;
        private string _iconPath;
        private string _iconUrl;
        private IEnumerable<string> _iconPaths;
        private List<string> _iconUrls;

        public IconRandomizer(IConfiguration config) {
            _config = config;

            var iconMount = config.GetValue<string>("Database:RandomIconMount");
            _iconUrl = config.GetValue<string>("Database:RandomIconUrl");
            _iconPath = Path.Combine(iconMount, _iconUrl.Substring(1));

            var files = Directory.GetFiles(_iconPath);
            _iconPaths = files.Select(p => Path.Combine(_iconPath, p));
            _iconUrls = files.Select(p => Path.Combine(_iconUrl, p.Replace(iconMount, ""))).ToList();

        }

        public string GetIconUrl() {
            return _iconUrls.PickRandom();
        }

    }

}