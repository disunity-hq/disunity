using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Disunity.Store.Entities;


namespace Disunity.Client {

    [GeneratedCode("NSwag", "13.0.4.0 (NJsonSchema v10.0.21.0 (Newtonsoft.Json v11.0.0.0))")]
    public interface IDisunityClient
    {
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task<List<DisunityVersionDto>> GetDisunityVersionsAsync();
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task<List<DisunityVersionDto>> GetDisunityVersionsAsync(CancellationToken cancellationToken);
    
    }

}