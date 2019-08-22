using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using OrgMemberDto = Disunity.Client.v1.Models.OrgMemberDto;


namespace Disunity.Client.v1 {

    [GeneratedCode("NSwag", "13.0.4.0 (NJsonSchema v10.0.21.0 (Newtonsoft.Json v11.0.0.0))")]
    public class OrgMemberClient : IOrgMemberClient
    {
        private string _baseUrl = "";
        public HttpClient HttpClient { get; }
        private Lazy<JsonSerializerSettings> _settings;
    
        public OrgMemberClient(string baseUrl, HttpClient httpClient)
        {
            BaseUrl = baseUrl; 
            HttpClient = httpClient; 
            _settings = new Lazy<JsonSerializerSettings>(() => 
            {
                var settings = new JsonSerializerSettings();
                UpdateJsonSerializerSettings(settings);
                return settings;
            });
        }
    
        public string BaseUrl 
        {
            get { return _baseUrl; }
            set { _baseUrl = value; }
        }
    
        protected JsonSerializerSettings JsonSerializerSettings { get { return _settings.Value; } }

        void UpdateJsonSerializerSettings(JsonSerializerSettings settings) {
            throw new NotImplementedException();
        }

        void PrepareRequest(HttpClient client, HttpRequestMessage request, string url) {
            throw new NotImplementedException();
        }

        void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder) {
            throw new NotImplementedException();
        }

        void ProcessResponse(HttpClient client, HttpResponseMessage response) {
            throw new NotImplementedException();
        }

        /// <summary>Get a list of the memberships for all users in the given org</summary>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        public Task<List<OrgMemberDto>> GetMembersAsync(string orgSlug)
        {
            return GetMembersAsync(orgSlug, CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get a list of the memberships for all users in the given org</summary>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        public async Task<List<OrgMemberDto>> GetMembersAsync(string orgSlug, CancellationToken cancellationToken)
        {
            if (orgSlug == null)
                throw new ArgumentNullException("orgSlug");
    
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/orgs/{orgSlug}/members");
            urlBuilder_.Replace("{orgSlug}", Uri.EscapeDataString(ConvertToString(orgSlug, CultureInfo.InvariantCulture)));
    
            var client_ = HttpClient;

            using (var request_ = new HttpRequestMessage())
            {
                request_.Method = new HttpMethod("GET");
                request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                PrepareRequest(client_, request_, urlBuilder_);
                var url_ = urlBuilder_.ToString();
                request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                PrepareRequest(client_, request_, url_);
    
                var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                try
                {
                    var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }
    
                    ProcessResponse(client_, response_);
    
                    var status_ = ((int)response_.StatusCode).ToString();
                    if (status_ == "200") 
                    {
                        var objectResponse_ = await ReadObjectResponseAsync<List<OrgMemberDto>>(response_, headers_).ConfigureAwait(false);
                        return objectResponse_.Object;
                    }
                    else
                    if (status_ != "200" && status_ != "204")
                    {
                        var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false); 
                        throw new ApiClientException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                    }
            
                    return default;
                }
                finally
                {
                    if (response_ != null)
                        response_.Dispose();
                }
            }

        }
    
        /// <summary>Update a users role within an org</summary>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        public Task UpdateUserRoleAsync(OrgMemberDto membershipDto, string orgSlug)
        {
            return UpdateUserRoleAsync(membershipDto, orgSlug, CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Update a users role within an org</summary>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        public async Task UpdateUserRoleAsync(OrgMemberDto membershipDto, string orgSlug, CancellationToken cancellationToken)
        {
            if (orgSlug == null)
                throw new ArgumentNullException("orgSlug");
    
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/orgs/{orgSlug}/members");
            urlBuilder_.Replace("{orgSlug}", Uri.EscapeDataString(ConvertToString(orgSlug, CultureInfo.InvariantCulture)));
    
            var client_ = HttpClient;

            using (var request_ = new HttpRequestMessage())
            {
                var content_ = new StringContent(JsonConvert.SerializeObject(membershipDto, _settings.Value));
                content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                request_.Content = content_;
                request_.Method = new HttpMethod("PUT");
    
                PrepareRequest(client_, request_, urlBuilder_);
                var url_ = urlBuilder_.ToString();
                request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                PrepareRequest(client_, request_, url_);
    
                var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                try
                {
                    var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }
    
                    ProcessResponse(client_, response_);
    
                    var status_ = ((int)response_.StatusCode).ToString();
                    if (status_ == "200") 
                    { }
                    else
                    if (status_ != "200" && status_ != "204")
                    {
                        var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false); 
                        throw new ApiClientException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                    }
                }
                finally
                {
                    if (response_ != null)
                        response_.Dispose();
                }
            }

        }
    
        /// <summary>Adds a user to an existing organization</summary>
        /// <param name="membershipDto">The username of the user to add and the role with which to grant them</param>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        public Task AddMembersAsync(OrgMemberDto membershipDto, string orgSlug)
        {
            return AddMembersAsync(membershipDto, orgSlug, CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Adds a user to an existing organization</summary>
        /// <param name="membershipDto">The username of the user to add and the role with which to grant them</param>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        public async Task AddMembersAsync(OrgMemberDto membershipDto, string orgSlug, CancellationToken cancellationToken)
        {
            if (orgSlug == null)
                throw new ArgumentNullException("orgSlug");
    
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/orgs/{orgSlug}/members");
            urlBuilder_.Replace("{orgSlug}", Uri.EscapeDataString(ConvertToString(orgSlug, CultureInfo.InvariantCulture)));
    
            var client_ = HttpClient;

            using (var request_ = new HttpRequestMessage())
            {
                var content_ = new StringContent(JsonConvert.SerializeObject(membershipDto, _settings.Value));
                content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                request_.Content = content_;
                request_.Method = new HttpMethod("POST");
    
                PrepareRequest(client_, request_, urlBuilder_);
                var url_ = urlBuilder_.ToString();
                request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                PrepareRequest(client_, request_, url_);
    
                var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                try
                {
                    var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }
    
                    ProcessResponse(client_, response_);
    
                    var status_ = ((int)response_.StatusCode).ToString();
                    if (status_ == "200") 
                    { }
                    else
                    if (status_ == "204") 
                    { }
                    else
                    if (status_ == "400") 
                    {
                        string responseText_ = ( response_.Content == null ) ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new ApiClientException("Returns information about why the request was malformed", (int)response_.StatusCode, responseText_, headers_, null);
                    }
                    else
                    if (status_ != "200" && status_ != "204")
                    {
                        var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false); 
                        throw new ApiClientException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                    }
                }
                finally
                {
                    if (response_ != null)
                        response_.Dispose();
                }
            }

        }
    
        /// <summary>Remove a user from an organization</summary>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        public Task DeleteMembersAsync(string username, string orgSlug)
        {
            return DeleteMembersAsync(username, orgSlug, CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Remove a user from an organization</summary>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        public async Task DeleteMembersAsync(string username, string orgSlug, CancellationToken cancellationToken)
        {
            if (username == null)
                throw new ArgumentNullException("username");
    
            if (orgSlug == null)
                throw new ArgumentNullException("orgSlug");
    
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/orgs/{orgSlug}/members/{username}");
            urlBuilder_.Replace("{username}", Uri.EscapeDataString(ConvertToString(username, CultureInfo.InvariantCulture)));
            urlBuilder_.Replace("{orgSlug}", Uri.EscapeDataString(ConvertToString(orgSlug, CultureInfo.InvariantCulture)));
    
            var client_ = HttpClient;

            using (var request_ = new HttpRequestMessage())
            {
                request_.Method = new HttpMethod("DELETE");
    
                PrepareRequest(client_, request_, urlBuilder_);
                var url_ = urlBuilder_.ToString();
                request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                PrepareRequest(client_, request_, url_);
    
                var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                try
                {
                    var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }
    
                    ProcessResponse(client_, response_);
    
                    var status_ = ((int)response_.StatusCode).ToString();
                    if (status_ == "200") 
                    { }
                    else
                    if (status_ != "200" && status_ != "204")
                    {
                        var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false); 
                        throw new ApiClientException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                    }
                }
                finally
                {
                    if (response_ != null)
                        response_.Dispose();
                }
            }

        }
    
        protected struct ObjectResponseResult<T>
        {
            public ObjectResponseResult(T responseObject, string responseText)
            {
                Object = responseObject;
                Text = responseText;
            }
    
            public T Object { get; }
    
            public string Text { get; }
        }
    
        public bool ReadResponseAsString { get; set; }
        
        protected virtual async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers)
        {
            if (response == null || response.Content == null)
            {
                return new ObjectResponseResult<T>(default, string.Empty);
            }
        
            if (ReadResponseAsString)
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    var typedBody = JsonConvert.DeserializeObject<T>(responseText, JsonSerializerSettings);
                    return new ObjectResponseResult<T>(typedBody, responseText);
                }
                catch (JsonException exception)
                {
                    var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
                    throw new ApiClientException(message, (int)response.StatusCode, responseText, headers, exception);
                }
            }

            try
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                using (var streamReader = new StreamReader(responseStream))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    var serializer = JsonSerializer.Create(JsonSerializerSettings);
                    var typedBody = serializer.Deserialize<T>(jsonTextReader);
                    return new ObjectResponseResult<T>(typedBody, string.Empty);
                }
            }
            catch (JsonException exception)
            {
                var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                throw new ApiClientException(message, (int)response.StatusCode, string.Empty, headers, exception);
            }
        }
    
        private string ConvertToString(object value, CultureInfo cultureInfo)
        {
            if (value is Enum)
            {
                string name = Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    var field = IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                    if (field != null)
                    {
                        var attribute = CustomAttributeExtensions.GetCustomAttribute(field, typeof(EnumMemberAttribute)) 
                            as EnumMemberAttribute;
                        if (attribute != null)
                        {
                            return attribute.Value != null ? attribute.Value : name;
                        }
                    }
                }
            }
            else if (value is bool) {
                return Convert.ToString(value, cultureInfo).ToLowerInvariant();
            }
            else if (value is byte[])
            {
                return Convert.ToBase64String((byte[]) value);
            }
            else if (value != null && value.GetType().IsArray)
            {
                var array = Enumerable.OfType<object>((Array) value);
                return string.Join(",", Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
            }
        
            return Convert.ToString(value, cultureInfo);
        }
    }

}