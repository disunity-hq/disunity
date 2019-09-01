using System.Collections.Generic;


namespace Disunity.Management.Models {

    public class TargetMeta {

        public int Id { get; set; }

        public string ExecutablePath { get; set; }

        public string ManagedPath { get; set; }

        public string Slug { get; set; }

        public string DisplayName { get; set; }

        public ProfileMeta ActiveProfile { get; set; }

        public List<TargetProfile> Profiles { get; set; }

    }

}