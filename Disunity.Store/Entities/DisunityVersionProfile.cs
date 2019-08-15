using AutoMapper;

using Disunity.Client.v1.Models;


namespace Disunity.Store.Entities {

    public class DisunityVersionProfile : Profile {

        public DisunityVersionProfile() {
            CreateMap<DisunityVersion, DisunityVersionDto>()
                .ForMember(d => d.MaxUnityVersion,
                           m => m.MapFrom(v => v.CompatibleUnityVersion.MaxCompatibleVersion))
                .ForMember(
                    d => d.MinUnityVersion,
                    m => m.MapFrom(v => v.CompatibleUnityVersion.MinCompatibleVersion));
        }

    }

}