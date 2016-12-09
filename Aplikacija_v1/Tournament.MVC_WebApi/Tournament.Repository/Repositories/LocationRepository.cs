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
    public class LocationRepository : ILocationRepository
    {
        protected IGenericRepository GenericRepository { get; set; }

        public LocationRepository(IGenericRepository genericRepository)
        {
            this.GenericRepository = genericRepository;
        }

        //Create new Location
        public async Task<int> Add(ILocationDomain entity)
        {
            try
            {
                return await GenericRepository.Add(Mapper.Map<Location>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Location by Id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                var item = await GenericRepository.Get<Location>(id);

                if (item == null)
                    return 0;

                return await GenericRepository.Delete(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Location by Object
        public async Task<int> Delete(ILocationDomain entity)
        {
            try
            {
                return await GenericRepository.Delete(Mapper.Map<Location>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Location by Id
        public async Task<ILocationDomain> Get(Guid id)
        {
            try
            {
                var response = Mapper.Map<ILocationDomain>(await GenericRepository.Get<Location>(id));
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get all Locations
        public async Task<IEnumerable<ILocationDomain>> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<ILocationDomain>>(await GenericRepository.GetAll<Location>());
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Update Location
        public async Task<int> Update(ILocationDomain entity)
        {
            try
            {
                return await GenericRepository.Update(Mapper.Map<Location>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
