using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Tournament.MVC_WebApi.ViewModels;
using Tournament.Model.Common;
using Tournament.Model;

namespace Tournament.MVC_WebApi.AutoMapperConfig
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            //AspNetUser view <-> AspNetUser domain
            CreateMap<IAspNetUserDomain, AspNetUserView>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<AspNetUserView, AspNetUserDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //AspNetRole view <-> AspNetRole domain
            CreateMap<AspNetRoleView, IAspNetRoleDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<AspNetRoleView, AspNetRoleDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //AspNetUserClaim view <-> AspNetUserClaim domain
            CreateMap<AspNetUserClaimView, IAspNetUserClaimDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<AspNetUserClaimView, AspNetUserClaimDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //AspNetUserLogin view <-> AspNetUserLogin domain
            CreateMap<AspNetUserLoginView, IAspNetUserLoginDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<AspNetUserLoginView, AspNetUserLoginDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //Location view <-> Location domain
            CreateMap<LocationView, ILocationDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<LocationView, LocationDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //Match view <-> Match domain
            CreateMap<MatchView, IMatchDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<MatchView, MatchDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //Player view <-> Player domain
            CreateMap<PlayerView, IPlayerDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<PlayerView, PlayerDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //Referee view <-> Referee domain
            CreateMap<RefereeView, IRefereeDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<RefereeView, RefereeDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //Result view <-> Result domain
            CreateMap<ResultView, IResultDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<ResultView, ResultDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //Team view <-> Team domain
            CreateMap<TeamView, ITeamDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<TeamView, TeamDomain>().PreserveReferences().ReverseMap().PreserveReferences();

            //Tournament view <-> Tournament domain
            CreateMap<TournamentView, ITournamentDomain>().PreserveReferences().ReverseMap().PreserveReferences();
            CreateMap<TournamentView, TournamentDomain>().PreserveReferences().ReverseMap().PreserveReferences();
        }
    }
}