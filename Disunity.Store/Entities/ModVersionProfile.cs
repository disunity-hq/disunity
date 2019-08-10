using AutoMapper;


namespace Disunity.Store.Entities {

    public class ModVersionProfile : Profile {

        public ModVersionProfile() {
            CreateMap<ModVersion, ModVersionDto>();

        }

    }

}