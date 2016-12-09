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
    public class TournamentService : ITournamentService
    {
        protected ITournamentRepository TournamentRepository { get; set; }

        public TournamentService(ITournamentRepository rep)
        {
            this.TournamentRepository = rep;
        }

        //Add Tournament
        public async Task<int> Add(ITournamentDomain entry)
        {
            try
            {
                return await TournamentRepository.Add(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Tournament by id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                return await TournamentRepository.Delete(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Tournament by object
        public async Task<int> Delete(ITournamentDomain entry)
        {
            try
            {
                return await TournamentRepository.Delete(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get Tournament by Id
        public async Task<ITournamentDomain> Read(Guid id)
        {
            try
            {
                var response = await TournamentRepository.Get(id);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get All Tournaments
        public async Task<IEnumerable<ITournamentDomain>> ReadAll()
        {
            try
            {
                var response = await TournamentRepository.GetAll();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Update Tournament 
        public async Task<int> Update(ITournamentDomain entry)
        {
            try
            {
                return await TournamentRepository.Update(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
