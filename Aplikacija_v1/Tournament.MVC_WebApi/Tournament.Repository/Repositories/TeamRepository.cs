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

        //Get Team with most points
        public async Task<IEnumerable<ITeamDomain>> MostPoints()
        {
            try
            {
                IEnumerable<ITeamDomain> teams = Mapper.Map<IEnumerable<ITeamDomain>>
                   (await GenericRepository.GetQueryable<Team>().OrderByDescending(t => t.Points).ToListAsync());
                IEnumerable<ITeamDomain> teamWithMostPoints = teams.Take(1);

                List<ITeamDomain> response = new List<ITeamDomain>();
                //response.Add(teamWithMostPoints.First());

                foreach (var team in teams)
                {
                    if (team.Points == teamWithMostPoints.First().Points)
                    {
                        response.Add(team);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         //Get Team With most wins
        public async Task<IEnumerable<ITeamDomain>> MostWins()
        {
            try
            {
                IEnumerable<ITeamDomain> teams = Mapper.Map<IEnumerable<ITeamDomain>>
                   (await GenericRepository.GetQueryable<Team>().OrderByDescending(t => t.Won)
                   .ToListAsync());
                IEnumerable<ITeamDomain> teamWithMostWins = teams.Take(1);

                List<ITeamDomain> response = new List<ITeamDomain>();
                //response.Add(teamWithMostWins.First());

                foreach (var team in teams)
                {
                    if (team.Won == teamWithMostWins.First().Won)
                    {
                        response.Add(team);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Team With most goals
        public async Task<IEnumerable<ITeamDomain>> MostGoals()
        {
            try
            {
                IEnumerable<ITeamDomain> teams = Mapper.Map<IEnumerable<ITeamDomain>>
                    (await GenericRepository.GetQueryable<Team>().OrderByDescending(t => t.GoalsScored).ToListAsync());

                IEnumerable<ITeamDomain> teamWithMostGoals = teams.Take(1);



                List<ITeamDomain> response = new List<ITeamDomain>();
                //response.Add(teamWithMostGoals.First());

                foreach (var team in teams)
                {
                    if (team.GoalsScored == teamWithMostGoals.First().GoalsScored)
                    {
                        response.Add(team);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Team Where Type = League and Winner
        public async Task<ITeamDomain> GetLeagueTournamentWinner(Guid tournamentId)
        {
            try
            {
                IEnumerable<ITeamDomain> getAllTeams = Mapper.Map<IEnumerable<ITeamDomain>>
                (await GenericRepository.GetQueryable<Team>().Where(t => t.TournamentId == tournamentId).Include(t => t.Tournament).OrderByDescending(t => t.Points)
                .ToListAsync());
                if (getAllTeams.Count() != 0)
                {
                    var response = getAllTeams.First();
                    return response;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<ITeamDomain>> GetLeagueTournamentFirstTwoTeams(Guid tournamentId)
        {
            try
            {
                IEnumerable<ITeamDomain> getAllTeams = Mapper.Map<IEnumerable<ITeamDomain>>
                (await GenericRepository.GetQueryable<Team>().Where(t => t.TournamentId == tournamentId).Include(t => t.Tournament).OrderByDescending(t => t.Points)
                .ToListAsync());
                if (getAllTeams.Count() != 0)
                {
                    List<ITeamDomain> response = new List<ITeamDomain>();
                    response.Add(getAllTeams.First());
                    response.Add(getAllTeams.ElementAt(1));
                    return response;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
