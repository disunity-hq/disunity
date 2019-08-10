using System.Collections.Generic;
using System.IO;

using BindingAttributes;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

using Tracery;


namespace Disunity.Store.Data.Services {

    public interface IThesaurus {

        string For(string word);

    }

    [AsSingleton(typeof(IThesaurus))]
    public class Thesaurus : IThesaurus {

        private readonly Dictionary<string, List<string>> _index;

        public Thesaurus(IConfiguration config) {
            _index = new Dictionary<string, List<string>>();
            var thesaurusDataPath = config.GetValue<string>("Database:ThesaurusDataPath");
            var letters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

            foreach (var letter in letters) {
                var filename = $"thesaurus-{letter}.json";
                var path = Path.Combine(thesaurusDataPath, filename);
                var text = File.ReadAllText(path);
                var data = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(text);

                foreach (var pair in data) {
                    _index[pair.Key] = pair.Value;
                }
            }
        }

        public string For(string word) {
            return _index.ContainsKey(word) ? _index[word].PickRandom() : word;
        }

    }

}