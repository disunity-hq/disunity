using System;
using System.IO;
using System.Linq;

using BindingAttributes;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Tracery;


namespace Disunity.Store.Data.Services
{

    public class TraceryUnparser
    {

        [Factory]
        public static Func<string, Unparser> UnparserFactory(IServiceProvider di) {
            var logger = di.GetRequiredService<ILogger<TraceryUnparser>>();
            var config = di.GetRequiredService<IConfiguration>();
            var slugifier = di.GetRequiredService<ISlugifier>();
            var grammarPath = config.GetValue<string>("Database:TraceryGrammarPath");

            string Capitalize(string s) {
                if (string.IsNullOrEmpty(s)) {
                    return s;
                }

                var result = char.ToUpper(s[0]).ToString();

                if (s.Length > 1) {
                    result += s.Substring(1);
                }

                return result;
            }

            string TitleCase(string s) {
                var parts = s.Split(null)
                             .Where(p => p.Any())
                             .Select(p => char.ToUpper(p[0]) + p.Substring(1));

                return String.Join(" ", parts);
            }

            return filename => {
                var fullPath = Path.Combine(grammarPath, filename);
                var grammar = Grammar.FromFile(fullPath);
                var unparser = new Unparser(grammar);

                unparser.Modifiers["capitalize"] = Capitalize;
                unparser.Modifiers["title"] = TitleCase;
                unparser.Modifiers["slug"] = slugifier.Slugify;

                return unparser;
            };
        }
    }
}