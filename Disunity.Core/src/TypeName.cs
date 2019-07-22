using System;


namespace Disunity.Core {

    /// <summary>
    ///     Represents a Type's name.
    /// </summary>
    [Serializable]
    public class TypeName {

        /// <summary>
        ///     The Type's name.
        /// </summary>
        public string Name = "";

        /// <summary>
        ///     The Type's namespace.
        /// </summary>
        public string NameSpace = "";

        /// <summary>
        ///     Initialize a new TypeName.
        /// </summary>
        /// <param name="nameSpace">The Type's namespace.</param>
        /// <param name="name">The Type's name.</param>
        public TypeName(string nameSpace, string name) {
            NameSpace = nameSpace;
            Name = name;
        }

        public TypeName() { }

    }

}