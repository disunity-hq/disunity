using AutoMapper;

using Disunity.Client.v1.Models;


namespace Disunity.Store.Entities {

    public class ModTargetCompatibilityProfile : Profile {

        public ModTargetCompatibilityProfile() {
            CreateMap<ModTargetCompatibility, ModTargetCompatibilityDto>();

        }

    }

}