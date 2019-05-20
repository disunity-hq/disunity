using System;
using System.Collections.Generic;
using System.Linq;
using Disunity.Editor.Pickers;
using UnityEditorInternal;
using Object = UnityEngine.Object;


namespace Disunity.Editor.Fields {

    public class ClassPickerField : BasePickerField<GenericEntry, FilteredPicker> {

        protected Dictionary<string, Type> GetTypesFromAssemblies(Object[] options) {
            var types = new Dictionary<string, Type>();
            var assemblies = options.Select(o => ( (AssemblyDefinitionAsset)o ).name);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                if (!assemblies.Contains(assembly.GetName().Name)) {
                    continue;
                }

                foreach (var type in assembly.ExportedTypes) {
                    types[$"{assembly.GetName().Name}.{type.FullName}"] = type;
                }
            }

            return types;
        }

        protected List<IEntry> GetEntries(string[] names) {
            var entries = names.Select(o => new GenericEntry() { value = o });
            return new List<IEntry>(entries);
        }

        public void OnGUI(string currentClass, string currentAssembly, Object[] assemblies, Action<string, string> handler) {
            var types = new Dictionary<string, Type>();

            List<IEntry> Generator() {
                types = GetTypesFromAssemblies(assemblies);
                return GetEntries(types.Keys.ToArray());
            }

            void Handler(BasePickerField<GenericEntry, FilteredPicker> field) {
                currentClass = field.Selection.AsString;
                var behaviour = types[currentClass];
                handler(behaviour.FullName, behaviour.Assembly.GetName().Name);
            }

            base.OnGUI(Generator, Handler);
        }

    }

}