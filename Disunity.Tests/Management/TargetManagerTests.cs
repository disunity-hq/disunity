using Disunity.Management;

using Xunit;


namespace Disunity.Tests.Management {

    public class TargetManagerFixutre {

        public ITargetManager TargetManager { get; }

        public TargetManagerFixutre() {
            // TODO put implementation here
            TargetManager = null;
        }

    }
    
    public class TargetManagerTests: IClassFixture<TargetManagerFixutre> {

        private readonly TargetManagerFixutre _fixutre;
        
        public TargetManagerTests(TargetManagerFixutre fixutre) {
            _fixutre = fixutre;
        }

    }

}