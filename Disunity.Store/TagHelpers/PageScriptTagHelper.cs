using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.TagHelpers {

    public class PageScriptTagHelper : TagHelper {

        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<PageScriptTagHelper> _logger;

        /// <summary>
        /// The path to the script to load.
        /// </summary>
        /// <example>
        /// "mod/upload" will be mapped to the js file "/js/mod/upload.js"
        /// </example>
        public string Path { get; set; }

        /// <summary>
        /// Whether or not the attempt to run the `InitPageScript()` function after injecting script
        /// </summary>
        public bool InitScript { get; set; } = true;

        public object[] Params { get; set; } = new object[0];

        public PageScriptTagHelper(IHostingEnvironment env, IHttpContextAccessor context,
                                   ILogger<PageScriptTagHelper> logger) {
            _env = env;
            _context = context;
            _logger = logger;
        }

        private string GetSrcForPath(string path) {
            if (_env.IsDevelopment()) {
                return $"/js/{path}.js";
            } else {
                return $"/js/{path}.min.js";
            }
        }

        private string GetPathForPage() {
            var page = _context.HttpContext.GetRouteData().Values["page"] as string;
            return page?.Substring(1, page.Length - 1).ToLower();
        }

        private string GetAddendum(string route) {
            var paramLiterals = new List<string>();

            foreach (var param in Params) {
                switch (param) {
                    case null:
                        paramLiterals.Add("null");
                        break;

                    case string stringParam:
                        paramLiterals.Add($"'{stringParam}'");
                        break;

                    default:
                        paramLiterals.Add(param.ToString().ToLowerInvariant());
                        break;
                }
            }

            var template = $@"<script>

            try {{
                InitPageScript({string.Join(",", paramLiterals)});
            }}
            catch (error) {{
                console.error('Page script failed to initialize for {route}');
                console.error(error);
            }}

            </script>";

            return template;
        }

        private string GetContent(string path) {
            var src = GetSrcForPath(path);
            return $"<script src=\"{src}\"></script>";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            var path = Path ?? GetPathForPage();

            string content;

            if (InitScript) {
                var addendum = GetAddendum(path);
                content = GetContent(path) + addendum;
            } else {
                content = GetContent(path);
            }

            output.TagName = null;
            output.Content.SetHtmlContent(content);
        }

    }

}