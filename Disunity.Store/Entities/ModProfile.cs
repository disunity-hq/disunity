using AutoMapper;

using Disunity.Client.v1.Models;


namespace Disunity.Store.Entities {

    public class ModProfile : Profile {

        public ModProfile() {
            CreateMap<Mod, ModDto>();
        }

    }

}