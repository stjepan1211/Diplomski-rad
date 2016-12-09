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
    public class PlayerService : IPlayerService
    {
        protected IPlayerRepository PlayerRepository { get; set; }

        public PlayerService(IPlayerRepository rep)
        {
            this.PlayerRepository = rep;
        }

        //Add Player
        public async Task<int> Add(IPlayerDomain entry)
        {
            try
            {
                return await PlayerRepository.Add(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Player by id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                return await PlayerRepository.Delete(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Player by object
        public async Task<int> Delete(IPlayerDomain entry)
        {
            try
            {
                return await PlayerRepository.Delete(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get Player by Id
        public async Task<IPlayerDomain> Read(Guid id)
        {
            try
            {
                var response = await PlayerRepository.Get(id);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get All Players
        public async Task<IEnumerable<IPlayerDomain>> ReadAll()
        {
            try
            {
                var response = await PlayerRepository.GetAll();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Update Player 
        public async Task<int> Update(IPlayerDomain entry)
        {
            try
            {
                return await PlayerRepository.Update(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
