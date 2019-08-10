using Disunity.Store.Util;

using Xunit;


namespace Disunity.Store.Tests {

    public class HelloWorldTests {

        [Fact]
        public void Greet_Arg_Works() {
            var message = HelloWorld.Greet("Charlie");
            Assert.IsType<string>(message);
            Assert.Equal("Hello Charlie!", message);
        }

        [Fact]
        public void Greet_Default_Works() {
            var message = HelloWorld.Greet();
            Assert.IsType<string>(message);
            Assert.Equal("Hello World!", message);
        }

    }

}