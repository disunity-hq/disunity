using AutoMapper;

using Disunity.Client.v1.Models;


namespace Disunity.Store.Entities {

    public class TargetVersionProfile : Profile {

        public TargetVersionProfile() {
            // Allegedly AutoMapper can figure this out on it's own. Not sure why it isnt'
            CreateMap<TargetVersion, TargetVersionDto>()
                .ForMember(v => v.Slug,
                    m => m.MapFrom(s => s.Target.Slug))
                .ForMember(
                    v => v.MaxCompatibleVersion,
                    m => m.MapFrom(
                        s => s.DisunityCompatibility.MaxCompatibleVersion.VersionNumber))
                .ForMember(
                    v => v.MinCompatibleVersion,
                    m => m.MapFrom(
                        s => s.DisunityCompatibility.MinCompatibleVersion.VersionNumber));
        }

    }

}