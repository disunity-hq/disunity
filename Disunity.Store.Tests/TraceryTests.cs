using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Disunity.Store.Data.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Tracery;

using Xunit;
using Xunit.Abstractions;


namespace Disunity.Store.Tests {

    public class TraceryFixture {

        public ServiceProvider Container { get; }
        public Func<string, Unparser> UnparserFactory { get; }

        public TraceryFixture() {
            var assemblyPath = typeof(TraceryFixture).GetTypeInfo().Assembly.Location;
            var rootPath = Path.GetDirectoryName(assemblyPath);

            var config = new ConfigurationBuilder()
                         .SetBasePath(rootPath)
                         .AddJsonFile("appsettings.json")
                         .AddJsonFile("appsettings.Development.json", optional: true)
                         .AddEnvironmentVariables()
                         .Build();

            Container = new ServiceCollection()
                        .AddLogging()
                        .AddSingleton<IConfiguration>(config)
                        .AddSingleton<ISlugifier, Slugifier>()
                        .AddSingleton(TraceryUnparser.UnparserFactory)
                        .BuildServiceProvider();

            UnparserFactory = Container.GetRequiredService<Func<string, Unparser>>();
        }

    }

    public class TraceryTests : IClassFixture<TraceryFixture> {

        private ITestOutputHelper _log;
        private readonly TraceryFixture _fixture;

        public TraceryTests(ITestOutputHelper log, TraceryFixture fixture) {
            _log = log;
            _fixture = fixture;
        }

        [Fact]
        public void BasicTest() {
            var unparser = _fixture.UnparserFactory("Entities/target.json");
            // unparser.Seed(0);
            var entries = new List<string>();

            for (var i = 0; i < 20; i++) {
                var result = unparser.Generate("#description#");
                entries.Add(result);
            }

            var message = String.Join("\n", entries);
            throw new Exception(message);
        }

    }

}