using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Disunity.Store.Exceptions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;


namespace Disunity.Store.Errors {

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class ApiError : IActionResult {

        [JsonProperty] public string Name { get; }
        [JsonProperty] public string Context { get; }
        [JsonProperty] public string Message { get; }

        public virtual string Info => "A non-specific error.";

        public HttpStatusCode StatusCode { get; protected set; } = HttpStatusCode.BadRequest;

        protected ApiError(string message, string name = null, string context = null) {
            Name = name ?? GetType().Name;
            Context = context ?? GetContext();
            Message = message;
        }

        protected string GetContext() {
            var stackTrace = new StackTrace(1, false);
            var frames = stackTrace.GetFrames();

            return (from frame in frames
                    select frame.GetMethod()
                    into method
                    where !method.IsConstructor
                    select method.Name).FirstOrDefault();
        }

        public virtual ObjectResult ToObjectResult() {
            var result = new {
                errors = new Dictionary<string, object> {
                    [Name] = new {
                        Info = Info,
                        Items = new[] {this}
                    }
                }
            };

            return new ObjectResult(result) {
                StatusCode = (int) StatusCode
            };
        }

        public async Task ExecuteResultAsync(ActionContext context) {
            var objectResult = ToObjectResult();
            await objectResult.ExecuteResultAsync(context);
        }

        public virtual void OnFormatting(ActionContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }

            context.HttpContext.Response.StatusCode = (int) StatusCode;
        }

        public override string ToString() {
            return $"{Name} @{Context}: {Message}";
        }

        public ApiException ToExec() {
            return new ApiException(this);
        }

    }

}