using Disunity.Core;


namespace Disunity.Preloader {

    public class PreloadMod : Mod {

        public PreloadMod(string infoPath) : base(infoPath) {
            Log = new PreloadLogger(Info.Name);
        }
    }
}