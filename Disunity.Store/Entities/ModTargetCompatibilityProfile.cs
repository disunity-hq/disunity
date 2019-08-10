using AutoMapper;


namespace Disunity.Store.Entities {

    public class ModTargetCompatibilityProfile : Profile {

        public ModTargetCompatibilityProfile() {
            CreateMap<ModTargetCompatibility, ModTargetCompatibilityDto>();

        }

    }

}