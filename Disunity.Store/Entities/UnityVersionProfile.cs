using AutoMapper;


namespace Disunity.Store.Entities {

    public class UnityVersionProfile : Profile {

        public UnityVersionProfile() {
            CreateMap<UnityVersion, string>().ConvertUsing(u => u.VersionNumber);
        }

    }

}