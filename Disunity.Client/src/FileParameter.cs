using System.CodeDom.Compiler;
using System.IO;


namespace Disunity.Client {

    [GeneratedCode("NSwag", "13.0.4.0 (NJsonSchema v10.0.21.0 (Newtonsoft.Json v11.0.0.0))")]
    public class FileParameter
    {
        public FileParameter(Stream data)
            : this (data, null)
        {
        }

        public FileParameter(Stream data, string fileName)
            : this (data, fileName, null)
        {
        }

        public FileParameter(Stream data, string fileName, string contentType)
        {
            Data = data;
            FileName = fileName;
            ContentType = contentType;
        }

        public Stream Data { get; private set; }

        public string FileName { get; private set; }

        public string ContentType { get; private set; }
    }

}