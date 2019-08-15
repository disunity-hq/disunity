using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TargetDto = Disunity.Client.v1.Models.TargetDto;
using TargetVersionDto = Disunity.Client.v1.Models.TargetVersionDto;


namespace Disunity.Client.v1 {

    [GeneratedCode("NSwag", "13.0.4.0 (NJsonSchema v10.0.21.0 (Newtonsoft.Json v11.0.0.0))")]
    public interface ITargetClient
    {
        /// <summary>Get a list of all targets currently registered with Disunity.io</summary>
        /// <returns>Returns a JSON array of targets currently registered</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task<List<TargetDto>> GetAllTargetsAsync();
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get a list of all targets currently registered with Disunity.io</summary>
        /// <returns>Returns a JSON array of targets currently registered</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task<List<TargetDto>> GetAllTargetsAsync(CancellationToken cancellationToken);
    
        /// <summary>Lookup a target version from the executable hash</summary>
        /// <param name="hash">The SHA-256 hash of the executable you are searching for</param>
        /// <returns>Returns details on the located target</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task<TargetVersionDto> FindTargetByHashAsync(string hash);
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Lookup a target version from the executable hash</summary>
        /// <param name="hash">The SHA-256 hash of the executable you are searching for</param>
        /// <returns>Returns details on the located target</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task<TargetVersionDto> FindTargetByHashAsync(string hash, CancellationToken cancellationToken);
    
    }

}