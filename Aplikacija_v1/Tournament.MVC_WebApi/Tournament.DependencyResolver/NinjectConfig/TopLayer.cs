using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Tournament.Service.Common;
using Tournament.Service;

namespace Tournament.DependencyResolver.NinjectConfig
{
    public class TopLayer : NinjectModule
    {
        public override void Load()
        {
            Bind<IAspNetUserService>().To<AspNetUserService>();
            Bind<ILocationService>().To<LocationService>();
            Bind<IMatchService>().To<MatchService>();
            Bind<IPlayerService>().To<PlayerService>();
            Bind<IRefereeService>().To<RefereeService>();
            Bind<IResultService>().To<ResultService>();
            Bind<ITeamService>().To<TeamService>();
            Bind<ITournamentService>().To<TournamentService>();
            Bind<IAspNetUserLoginService>().To<AspNetUserLoginService>();
            Bind<IGalleryService>().To<GalleryService>();
        }
    }
}
