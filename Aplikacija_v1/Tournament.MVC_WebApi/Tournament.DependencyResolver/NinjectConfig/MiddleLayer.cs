using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Tournament.Repository.Common;
using Tournament.Repository;
using Tournament.Repository.Common.IGenericRepository;
using Tournament.Repository.GenericRepository;
using Tournament.Repository.Common.IRepositories;
using Tournament.Repository.Repositories;

namespace Tournament.DependencyResolver.NinjectConfig
{
    public class MiddleLayer : NinjectModule
    {
        public override void Load()
        {
            //Binding repositories
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IUnitOfWorkFactory>().To<UnitOfWorkFactory>();

            Bind<IGenericRepository>().To<GenericRepository>();

            Bind<IAspNetUserRepository>().To<AspNetUserRepository>();
            Bind<IAspNetUserLoginRepository>().To<AspNetUserLoginRepository>();
            Bind<ILocationRepository>().To<LocationRepository>();
            Bind<IMatchRepository>().To<MatchRepository>();
            Bind<IPlayerRepository>().To<PlayerRepository>();
            Bind<IRefereeRepository>().To<RefereeRepository>();
            Bind<IResultRepository>().To<ResultRepository>();
            Bind<ITeamRepository>().To<TeamRepository>();
            Bind<ITournamentRepository>().To<TournamentRepository>();
            Bind<IGalleryRepository>().To<GalleryRepository>();
        }
    }
}
