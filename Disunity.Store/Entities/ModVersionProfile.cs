using AutoMapper;

using Disunity.Client.v1.Models;


namespace Disunity.Store.Entities {

    public class ModVersionProfile : Profile {

        public ModVersionProfile() {
            CreateMap<ModVersion, ModVersionDto>();

        }

    }

}