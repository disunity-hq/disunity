using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ModDto = Disunity.Client.v1.Models.ModDto;


namespace Disunity.Client.v1 {

    [GeneratedCode("NSwag", "13.0.4.0 (NJsonSchema v10.0.21.0 (Newtonsoft.Json v11.0.0.0))")]
    public interface IModListClient: IClientBase
    {
        /// <summary>Get a list of all mods registered with disunity.io</summary>
        /// <param name="page">The current page of information to display, begins at 1.</param>
        /// <param name="pageSize">The page size to use when calculating pagination=</param>
        /// <returns>Return a JSON array of all mods registered with disunity.io</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task<List<ModDto>> GetModsAsync(int? page, int? pageSize);
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get a list of all mods registered with disunity.io</summary>
        /// <param name="page">The current page of information to display, begins at 1.</param>
        /// <param name="pageSize">The page size to use when calculating pagination=</param>
        /// <returns>Return a JSON array of all mods registered with disunity.io</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task<List<ModDto>> GetModsAsync(int? page, int? pageSize, CancellationToken cancellationToken);
    
        /// <summary>Get a list of all mods compatible with the given target id</summary>
        /// <param name="targetId">The target id to search for compatible mods</param>
        /// <param name="page">The current page of information to display, begins at 1.</param>
        /// <param name="pageSize">The page size to use when calculating pagination=</param>
        /// <returns>Return a JSON array of all found mods compatible with the given target id</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task<List<ModDto>> GetModsAsync(int targetId, int? page, int? pageSize);
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get a list of all mods compatible with the given target id</summary>
        /// <param name="targetId">The target id to search for compatible mods</param>
        /// <param name="page">The current page of information to display, begins at 1.</param>
        /// <param name="pageSize">The page size to use when calculating pagination=</param>
        /// <returns>Return a JSON array of all found mods compatible with the given target id</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task<List<ModDto>> GetModsAsync(int targetId, int? page, int? pageSize, CancellationToken cancellationToken);
    
    }

}