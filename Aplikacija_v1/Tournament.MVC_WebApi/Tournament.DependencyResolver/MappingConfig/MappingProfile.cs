using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Tournament.DAL;
using Tournament.Model.Common;
using Tournament.Model;

namespace Tournament.DependencyResolver.MappingConfig
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            //AspNetUser database <-> AspNetUser domain
            CreateMap<AspNetUser, IAspNetUserDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<AspNetUser, AspNetUserDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //AspNetRole database <-> AspNetRole domain
            CreateMap<AspNetRole, IAspNetRoleDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<AspNetRole, AspNetRoleDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //AspNetUserClaim database <-> AspNetUserClaim domain
            CreateMap<AspNetUserClaim, IAspNetUserClaimDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<AspNetUserClaim, AspNetUserClaimDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //AspNetUserLogin database <-> AspNetUserLogin domain
            CreateMap<AspNetUserLogin, IAspNetUserLoginDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<AspNetUserLogin, AspNetUserLoginDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //Location database <-> Location domain
            CreateMap<Location, ILocationDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<Location, LocationDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //Match database <-> Match domain
            CreateMap<Match, IMatchDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<Match, MatchDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //Player database <-> Player domain
            CreateMap<Player, IPlayerDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<Player, PlayerDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //Referee database <-> Referee domain
            CreateMap<Referee, IRefereeDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<Referee, RefereeDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //Result database <-> Result domain
            CreateMap<Result, IResultDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<Result, ResultDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //Team database <-> Team domain
            CreateMap<Team, ITeamDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<Team, TeamDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //Tournament database <-> Tournament domain
            CreateMap<Tournament.DAL.Tournament, ITournamentDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<Tournament.DAL.Tournament, TournamentDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //Gallery database <-> Gallery domain
            CreateMap<Gallery, IGalleryDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<Gallery, GalleryDomain>().PreserveReferences().ReverseMap().PreserveReferences();
        }
    }
}
