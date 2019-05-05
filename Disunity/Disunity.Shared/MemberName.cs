using System;


namespace Disunity.Shared {

    /// <summary>
    ///     Represents a member's name.
    /// </summary>
    [Serializable]
    public class MemberName {

        /// <summary>
        ///     The member's name.
        /// </summary>
        public string Name = "";

        /// <summary>
        ///     The Type to which the member belongs.
        /// </summary>
        public TypeName Type = new TypeName();

        /// <summary>
        ///     Initialize a new MemberName.
        /// </summary>
        /// <param name="type">The Type to which the member belongs.</param>
        /// <param name="name">The member's name.</param>
        public MemberName(TypeName type, string name) {
            Type = type;
            Name = name;
        }

        public MemberName() {
        }

    }

}