using AutoMapper;


namespace Disunity.Store.Entities {

    public class OrgProfile : Profile {

        public OrgProfile() {
            CreateMap<Org, OrgDto>();
        }

    }

}