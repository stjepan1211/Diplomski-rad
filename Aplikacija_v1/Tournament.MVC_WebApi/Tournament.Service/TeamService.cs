﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;
using Tournament.Repository.Common.IRepositories;
using Tournament.Service.Common;

namespace Tournament.Service
{
    public class TeamService : ITeamService
    {
        protected ITeamRepository TeamRepository { get; set; }

        public TeamService(ITeamRepository rep)
        {
            this.TeamRepository = rep;
        }

        //Add Team
        public async Task<int> Add(ITeamDomain entry)
        {
            try
            {
                return await TeamRepository.Add(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Team by id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                return await TeamRepository.Delete(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Team by object
        public async Task<int> Delete(ITeamDomain entry)
        {
            try
            {
                return await TeamRepository.Delete(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get Team by Id
        public async Task<ITeamDomain> Read(Guid id)
        {
            try
            {
                var response = await TeamRepository.Get(id);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get Team with most points
        public async Task<IEnumerable<ITeamDomain>> ReadMostPoints()
        {
            try
            {
                var response = await TeamRepository.MostPoints();
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Get Team with most wins
        public async Task<IEnumerable<ITeamDomain>> ReadMostWins()
        {
            try
            {
                var response = await TeamRepository.MostWins();
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Get Team with most goals
        public async Task<IEnumerable<ITeamDomain>> ReadMostGoals()
        {
            try
            {
                var response = await TeamRepository.MostGoals();
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Get All Teams
        public async Task<IEnumerable<ITeamDomain>> ReadAll()
        {
            try
            {
                var response = await TeamRepository.GetAll();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Get winner League Tournament Teams
        public async Task<ITeamDomain> ReadWinnerLeagueTournament(Guid tournamentId)
        {
            try
            {
                var response = await TeamRepository.GetLeagueTournamentWinner(tournamentId);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Get Two best League Tournament Teams
        public async Task<IEnumerable<ITeamDomain>> ReadFirstTwoLeagueTournament(Guid tournamentId)
        {
            try
            {
                var response = await TeamRepository.GetLeagueTournamentFirstTwoTeams(tournamentId);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Update Team 
        public async Task<int> Update(ITeamDomain entry)
        {
            try
            {
                return await TeamRepository.Update(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get All Teams Where Tournament Id == Tournament Id
        public async Task<IEnumerable<ITeamDomain>> GetWhereTournamentId(Guid tournamentId)
        {
            try
            {
                var response = await TeamRepository.GetAllWhereTournamentId(tournamentId);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
