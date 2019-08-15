namespace Disunity.Client {

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.0.4.0 (NJsonSchema v10.0.21.0 (Newtonsoft.Json v11.0.0.0))")]
    public partial interface IUploadClient
    {
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        System.Threading.Tasks.Task UploadAsync(FileParameter archiveUpload);
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        System.Threading.Tasks.Task UploadAsync(FileParameter archiveUpload, System.Threading.CancellationToken cancellationToken);
    
    }

}