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
    public class TeamRepository : ITeamRepository
    {
        protected IGenericRepository GenericRepository { get; set; }

        public TeamRepository(IGenericRepository genericRepository)
        {
            this.GenericRepository = genericRepository;
        }

        //Create new Team
        public async Task<int> Add(ITeamDomain entity)
        {
            try
            {
                return await GenericRepository.Add(Mapper.Map<Team>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Team by Id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                var item = await GenericRepository.Get<Team>(id);

                if (item == null)
                    return 0;

                return await GenericRepository.Delete(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Team by Object
        public async Task<int> Delete(ITeamDomain entity)
        {
            try
            {
                return await GenericRepository.Delete(Mapper.Map<Team>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Team by Id
        public async Task<ITeamDomain> Get(Guid id)
        {
            try
            {
                var response = Mapper.Map<ITeamDomain>(await GenericRepository.Get<Team>(id));
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get all Teams
        public async Task<IEnumerable<ITeamDomain>> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<ITeamDomain>>(await GenericRepository.GetAll<Team>());
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Update Team
        public async Task<int> Update(ITeamDomain entity)
        {
            try
            {
                return await GenericRepository.Update(Mapper.Map<Team>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Team Where TournamentId
        public async Task<IEnumerable<ITeamDomain>> GetAllWhereTournamentId(Guid tournamentId)
        {
            try
            {
                var response = Mapper.Map<IEnumerable<ITeamDomain>>
                    (await GenericRepository.GetQueryable<Team>().Where(t => t.TournamentId == tournamentId).OrderByDescending(t => t.Points)
                    .ToListAsync());
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
