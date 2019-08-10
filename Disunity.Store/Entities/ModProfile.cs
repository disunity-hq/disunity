using AutoMapper;


namespace Disunity.Store.Entities {

    public class ModProfile : Profile {

        public ModProfile() {
            CreateMap<Mod, ModDto>();
        }

    }

}