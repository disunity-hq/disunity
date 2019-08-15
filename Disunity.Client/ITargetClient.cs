using Disunity.Store.Entities;


namespace Disunity.Client {

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.0.4.0 (NJsonSchema v10.0.21.0 (Newtonsoft.Json v11.0.0.0))")]
    public partial interface ITargetClient
    {
        /// <summary>Get a list of all targets currently registered with Disunity.io</summary>
        /// <returns>Returns a JSON array of targets currently registered</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<System.Collections.Generic.List<TargetDto>> GetAllTargetsAsync();
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get a list of all targets currently registered with Disunity.io</summary>
        /// <returns>Returns a JSON array of targets currently registered</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<System.Collections.Generic.List<TargetDto>> GetAllTargetsAsync(System.Threading.CancellationToken cancellationToken);
    
        /// <summary>Lookup a target version from the executable hash</summary>
        /// <param name="hash">The SHA-256 hash of the executable you are searching for</param>
        /// <returns>Returns details on the located target</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<TargetVersionDto> FindAsync(string hash);
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Lookup a target version from the executable hash</summary>
        /// <param name="hash">The SHA-256 hash of the executable you are searching for</param>
        /// <returns>Returns details on the located target</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<TargetVersionDto> FindAsync(string hash, System.Threading.CancellationToken cancellationToken);
    
    }

}