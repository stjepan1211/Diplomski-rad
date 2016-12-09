using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;
using Tournament.Repository.Common.IRepositories;
using Tournament.Service.Common;

namespace Tournament.Service
{
    public class RefereeService : IRefereeService
    {
        protected IRefereeRepository RefereeRepository { get; set; }

        public RefereeService(IRefereeRepository rep)
        {
            this.RefereeRepository = rep;
        }

        //Add Referee
        public async Task<int> Add(IRefereeDomain entry)
        {
            try
            {
                return await RefereeRepository.Add(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Referee by id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                return await RefereeRepository.Delete(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Referee by object
        public async Task<int> Delete(IRefereeDomain entry)
        {
            try
            {
                return await RefereeRepository.Delete(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get Referee by Id
        public async Task<IRefereeDomain> Read(Guid id)
        {
            try
            {
                var response = await RefereeRepository.Get(id);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get All Referees
        public async Task<IEnumerable<IRefereeDomain>> ReadAll()
        {
            try
            {
                var response = await RefereeRepository.GetAll();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Update Player 
        public async Task<int> Update(IRefereeDomain entry)
        {
            try
            {
                return await RefereeRepository.Update(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
