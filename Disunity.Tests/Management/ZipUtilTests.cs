using System.IO.Abstractions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Disunity.Management.Util;

using Moq;


namespace Disunity.Tests.Management {

    public class ZipUtilFixture {

        

        public ZipUtil ZipUtil { get; }

        public IFileSystem MockFileSystem { get; }

        public ZipUtilFixture() {
            var mock = new Mock<HttpMessageHandler>();
            mock.Setup(m => m.Dispose());

            ZipUtil = new ZipUtil(MockFileSystem, new HttpClient(mock.Object));
        }


    }
    
    public class ZipUtilTests {

        

    }

}