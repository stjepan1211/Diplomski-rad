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
    public class MatchRepository : IMatchRepository
    {
        protected IGenericRepository GenericRepository { get; set; }

        public MatchRepository(IGenericRepository genericRepository)
        {
            this.GenericRepository = genericRepository;
        }

        //Create new Match
        public async Task<int> Add(IMatchDomain entity)
        {
            try
            {
                return await GenericRepository.Add(Mapper.Map<Match>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Match by Id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                var item = await GenericRepository.Get<Match>(id);

                if (item == null)
                    return 0;

                return await GenericRepository.Delete(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Match by Object
        public async Task<int> Delete(IMatchDomain entity)
        {
            try
            {
                return await GenericRepository.Delete(Mapper.Map<Match>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Match by Id
        public async Task<IMatchDomain> Get(Guid id)
        {
            try
            {
                var response = Mapper.Map<IMatchDomain>(await GenericRepository.Get<Match>(id));
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get all Mathces
        public async Task<IEnumerable<IMatchDomain>> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<IMatchDomain>>(await GenericRepository.GetAll<Match>());
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ////Get mathces by tournament
        //public async Task<IEnumerable<IMatchDomain>> GetMatchesByTournament(Guid tournamentId)
        //{
        //    try
        //    {
        //        var response = Mapper.Map<IEnumerable<IMatchDomain>>(await GenericRepository.GetQueryable<Match>()
        //            .Where(m => m.TournametId == tournamentId).ToListAsync());
        //        return response;
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //Get mathces by tournament
        public async Task<IEnumerable<IMatchDomain>> GetMatchesByTournament(Guid tournamentId)
        {
            try
            {
                var x = await GenericRepository.GetQueryable<Match>()
                    .Where(m => m.TournametId == tournamentId).Include(m => m.Referee).Include(m => m.Team).Include(m => m.Team1).ToListAsync();
                var response = Mapper.Map<IEnumerable<IMatchDomain>>(x);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Update Match
        public async Task<int> Update(IMatchDomain entity)
        {
            try
            {
                return await GenericRepository.Update(Mapper.Map<Match>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
