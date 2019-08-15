using AutoMapper;

using Disunity.Client.v1.Models;


namespace Disunity.Store.Entities {

    public class OrgProfile : Profile {

        public OrgProfile() {
            CreateMap<Org, OrgDto>();
        }

    }

}