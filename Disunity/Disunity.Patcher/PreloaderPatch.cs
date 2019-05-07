using System.Collections.Generic;
using Mono.Cecil;

namespace Disunity.Interface {
    public abstract class PreloaderPatch {
        public abstract IEnumerable<string> TargetAssemblies { get; }

        public virtual void OnInitialize() { }

        public virtual void OnFinalize() { }

        public abstract void Patch(ref AssemblyDefinition assembly);
    }
}