namespace Disunity.Client
{

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.0.4.0 (NJsonSchema v10.0.21.0 (Newtonsoft.Json v11.0.0.0))")]
    public partial class ApiClientException : System.Exception
    {
        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

        public ApiClientException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException) 
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + response.Substring(0, response.Length >= 512 ? 512 : response.Length), innerException)
        {
            StatusCode = statusCode;
            Response = response; 
            Headers = headers;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.0.4.0 (NJsonSchema v10.0.21.0 (Newtonsoft.Json v11.0.0.0))")]
    public partial class ApiClientException<TResult> : ApiClientException
    {
        public TResult Result { get; private set; }

        public ApiClientException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception innerException) 
            : base(message, statusCode, response, headers, innerException)
        {
            Result = result;
        }
    }

}
