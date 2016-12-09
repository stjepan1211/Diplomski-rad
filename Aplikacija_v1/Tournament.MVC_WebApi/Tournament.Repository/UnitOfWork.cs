using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.DAL.Common;
using Tournament.Repository.Common;

namespace Tournament.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        protected ITournament_v1Entities Context;

        public UnitOfWork(ITournament_v1Entities context)
        {
            this.Context = context;
        }

        public async Task<int> Commit()
        {
            return await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
