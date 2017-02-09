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

namespace Tournament.Repository.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        protected IGenericRepository GenericRepository { get; set; }

        public TournamentRepository(IGenericRepository genericRepository)
        {
            this.GenericRepository = genericRepository;
        }

        //Create new Tournament
        public async Task<int> Add(ITournamentDomain entity)
        {
            try
            {
                return await GenericRepository.Add(Mapper.Map<Tournament.DAL.Tournament>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Tournament by Id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                var item = await GenericRepository.Get<Tournament.DAL.Tournament>(id);

                if (item == null)
                    return 0;

                return await GenericRepository.Delete(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Tournament by Object
        public async Task<int> Delete(ITournamentDomain entity)
        {
            try
            {
                return await GenericRepository.Delete(Mapper.Map<Tournament.DAL.Tournament>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Tournament by Id
        public async Task<ITournamentDomain> Get(Guid id)
        {
            try
            {
                var response = Mapper.Map<ITournamentDomain>(await GenericRepository.Get<Tournament.DAL.Tournament>(id));
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get all Tournaments
        public async Task<IEnumerable<ITournamentDomain>> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<ITournamentDomain>>(await GenericRepository.GetAll<Tournament.DAL.Tournament>());
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Get tournaments by username
        public async Task<IEnumerable<ITournamentDomain>> GetByUsername(string username)
        {
            try
            {
                var response = Mapper.Map<IEnumerable<ITournamentDomain>>(GenericRepository.GetQueryable<DAL.Tournament>()
                    .Where(t => t.AspNetUser.UserName == username));
                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //Update Result
        public async Task<int> Update(ITournamentDomain entity)
        {
            try
            {
                return await GenericRepository.Update(Mapper.Map<Tournament.DAL.Tournament>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
