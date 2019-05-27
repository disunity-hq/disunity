using Disunity.Preloader;
using Mono.Cecil;
using ILogger = Disunity.Core.ILogger;


public class ExamplePreloader
{

    private readonly PreloadMod _mod;
    private readonly ILogger _log;

    public ExamplePreloader(PreloadMod mod)
    {
        _mod = mod;
        _log = mod.Log;

        Preloader.RegisterPatcher("Assembly-CSharp", PatchASC);
    }

    void PatchASC(AssemblyDefinition assembly) {
        _log.LogInfo("Hello from ExamplePreloader!");
    }
}