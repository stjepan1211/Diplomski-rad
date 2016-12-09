using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Tournament.DAL.Common;
using Tournament.DAL;

namespace Tournament.DependencyResolver.NinjectConfig
{
    public class BottomLayer : NinjectModule
    {
        public override void Load()
        {
            //Binding database context
            Bind<ITournament_v1Entities>().To<Tournament_v1Entities>();
        }
    }
}
