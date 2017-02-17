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
    public class ResultService : IResultService
    {
        protected IResultRepository ResultRepository { get; set; }

        public ResultService(IResultRepository rep)
        {
            this.ResultRepository = rep;
        }

        //Add Result
        public async Task<int> Add(IResultDomain entry)
        {
            try
            {
                return await ResultRepository.Add(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Result by id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                return await ResultRepository.Delete(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Result by object
        public async Task<int> Delete(IResultDomain entry)
        {
            try
            {
                return await ResultRepository.Delete(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get Result by Id
        public async Task<IResultDomain> Read(Guid id)
        {
            try
            {
                var response = await ResultRepository.Get(id);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get All Results
        public async Task<IEnumerable<IResultDomain>> ReadAll()
        {
            try
            {
                var response = await ResultRepository.GetAll();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Get Results By Match Id 
        public async Task<IEnumerable<IResultDomain>> ReadResultByMatchId(Guid matchId)
        {
            try
            {
                var response = await ResultRepository.GetResultByMatch(matchId);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Update Result 
        public async Task<int> Update(IResultDomain entry)
        {
            try
            {
                return await ResultRepository.Update(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
