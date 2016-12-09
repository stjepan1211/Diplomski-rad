using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Repository.Common;

namespace Tournament.Repository
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IUnitOfWork UnitOfWork;

        public UnitOfWorkFactory(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return this.UnitOfWork;
        }
    }
}
