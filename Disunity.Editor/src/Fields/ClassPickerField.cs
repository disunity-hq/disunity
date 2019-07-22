using System;
using System.Collections.Generic;
using System.Linq;
using Disunity.Editor.Pickers;
using Disunity.Editor.Windows;
using UnityEngine;


namespace Disunity.Editor.Fields {

    public abstract class ClassPickerField : BasePickerField {

        protected ClassPickerField(ExporterWindow window, BasePicker picker = null, GUIStyle style = null) : base(window, picker, style) { }

        protected abstract string[] CandidateAssemblyPaths();

        public override BasePicker DefaultPicker() {
            return new FilteredPicker();
        }

        protected Dictionary<string, Type> GetTypesFromAssemblies(string[] options) {
            var types = new Dictionary<string, Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                if (!options.Contains(assembly.GetName().Name)) {
                    continue;
                }

                foreach (var type in assembly.ExportedTypes) {
                    types[$"{assembly.GetName().Name}.{type.FullName}"] = type;
                }
            }

            return types;
        }

        protected List<ListEntry> GetEntries(string[] names) {
            var entries = names.Select(o => new ListEntry() { Value = o, Enabled = true});
            return new List<ListEntry>(entries);
        }

        protected override List<ListEntry> GenerateOptions() {
            var paths = CandidateAssemblyPaths();
            var names = paths.Select(o => AsmDef.FromAssetPath(o).name).ToArray();
            var types = GetTypesFromAssemblies(names);
            var entries = GetEntries(types.Keys.ToArray());
            return new List<ListEntry>(entries);
        }
    }
}