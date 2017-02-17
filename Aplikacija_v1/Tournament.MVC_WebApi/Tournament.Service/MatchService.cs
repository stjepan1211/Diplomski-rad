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
    public class MatchService : IMatchService
    {
        protected IMatchRepository MatchRepository { get; set; }

        public MatchService(IMatchRepository rep)
        {
            this.MatchRepository = rep;
        }

        //Add Match
        public async Task<int> Add(IMatchDomain entry)
        {
            try
            {
                return await MatchRepository.Add(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Match by id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                return await MatchRepository.Delete(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Match by object
        public async Task<int> Delete(IMatchDomain entry)
        {
            try
            {
                return await MatchRepository.Delete(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get Match by Id
        public async Task<IMatchDomain> Read(Guid id)
        {
            try
            {
                var response = await MatchRepository.Get(id);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get Matches by Tournament
        public async Task<IEnumerable<IMatchDomain>> ReadMatchesByTournament(Guid tournamentId)
        {
            try
            {
                var response = await MatchRepository.GetMatchesByTournament(tournamentId);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get All Matches
        public async Task<IEnumerable<IMatchDomain>> ReadAll()
        {
            try
            {
                var response = await MatchRepository.GetAll();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Update Match 
        public async Task<int> Update(IMatchDomain entry)
        {
            try
            {
                return await MatchRepository.Update(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
