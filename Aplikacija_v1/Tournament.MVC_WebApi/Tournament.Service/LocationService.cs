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
    public class LocationService : ILocationService
    {
        protected ILocationRepository LocationRepository { get;  set; }

        public LocationService(ILocationRepository rep)
        {
            this.LocationRepository = rep;
        }

        //Add Location
        public async Task<int> Add(ILocationDomain entry)
        {
            try
            {
                return await LocationRepository.Add(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Location by id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                return await LocationRepository.Delete(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Location by object
        public async Task<int> Delete(ILocationDomain entry)
        {
            try
            {
                return await LocationRepository.Delete(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get Location by Id
        public async Task<ILocationDomain> Read(Guid id)
        {
            try
            {
                var response = await LocationRepository.Get(id);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get All Locations
        public async Task<IEnumerable<ILocationDomain>> ReadAll()
        {
            try
            {
                var response = await LocationRepository.GetAll();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Update Location 
        public async Task<int> Update(ILocationDomain entry)
        {
            try
            {
                return await LocationRepository.Update(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
