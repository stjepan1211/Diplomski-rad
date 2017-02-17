using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Repository.Common.IRepositories;
using AutoMapper;
using Tournament.Repository.Common.IGenericRepository;
using Tournament.Model.Common;
using Tournament.DAL;
using System.Data.Entity;

namespace Tournament.Repository.Repositories
{
    public class ResultRepository : IResultRepository
    {
        protected IGenericRepository GenericRepository { get; set; }

        public ResultRepository(IGenericRepository genericRepository)
        {
            this.GenericRepository = genericRepository;
        }

        //Create new Result
        public async Task<int> Add(IResultDomain entity)
        {
            try
            {
                return await GenericRepository.Add(Mapper.Map<Result>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Result by Id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                var item = await GenericRepository.Get<Result>(id);

                if (item == null)
                    return 0;

                return await GenericRepository.Delete(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Result by Object
        public async Task<int> Delete(IResultDomain entity)
        {
            try
            {
                return await GenericRepository.Delete(Mapper.Map<Result>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Result by Id
        public async Task<IResultDomain> Get(Guid id)
        {
            try
            {
                var response = Mapper.Map<IResultDomain>(await GenericRepository.Get<Result>(id));
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get all Results
        public async Task<IEnumerable<IResultDomain>> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<IResultDomain>>(await GenericRepository.GetAll<Result>());
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Results By Match Id
        public async Task<IEnumerable<IResultDomain>> GetResultByMatch(Guid matchId)
        {
            try
            {
                var response = Mapper.Map<IEnumerable<IResultDomain>>(await GenericRepository.GetQueryable<Result>()
                    .Where(r => r.MatchId == matchId).ToListAsync());
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Update Result
        public async Task<int> Update(IResultDomain entity)
        {
            try
            {
                return await GenericRepository.Update(Mapper.Map<Result>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
