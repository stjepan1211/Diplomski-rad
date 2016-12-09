using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;
using Tournament.Repository.Common.IGenericRepository;
using Tournament.Repository.Common.IRepositories;
using AutoMapper;
using Tournament.DAL;

namespace Tournament.Repository.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        protected IGenericRepository GenericRepository { get; set; }

        public PlayerRepository(IGenericRepository genericRepository)
        {
            this.GenericRepository = genericRepository;
        }

        //Create new Player
        public async Task<int> Add(IPlayerDomain entity)
        {
            try
            {
                return await GenericRepository.Add(Mapper.Map<Player>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Player by Id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                var item = await GenericRepository.Get<Player>(id);

                if (item == null)
                    return 0;

                return await GenericRepository.Delete(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Player by Object
        public async Task<int> Delete(IPlayerDomain entity)
        {
            try
            {
                return await GenericRepository.Delete(Mapper.Map<Player>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Player by Id
        public async Task<IPlayerDomain> Get(Guid id)
        {
            try
            {
                var response = Mapper.Map<IPlayerDomain>(await GenericRepository.Get<Player>(id));
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get all Players
        public async Task<IEnumerable<IPlayerDomain>> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<IPlayerDomain>>(await GenericRepository.GetAll<Player>());
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Update Player
        public async Task<int> Update(IPlayerDomain entity)
        {
            try
            {
                return await GenericRepository.Update(Mapper.Map<Player>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
