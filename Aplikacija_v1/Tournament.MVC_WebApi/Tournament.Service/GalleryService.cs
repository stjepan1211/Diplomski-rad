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
    public class GalleryService : IGalleryService
    {
        protected IGalleryRepository GalleryRepository { get; set; }

        public GalleryService(IGalleryRepository rep)
        {
            this.GalleryRepository = rep;
        }

        //Add Gallery
        public async Task<int> Add(IGalleryDomain entry)
        {
            try
            {
                return await GalleryRepository.Add(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Gallery by id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                return await GalleryRepository.Delete(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Gallery by object
        public async Task<int> Delete(IGalleryDomain entry)
        {
            try
            {
                return await GalleryRepository.Delete(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get Gallery by Id
        public async Task<IGalleryDomain> Read(Guid id)
        {
            try
            {
                var response = await GalleryRepository.Get(id);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get All Teams
        public async Task<IEnumerable<IGalleryDomain>> ReadAll()
        {
            try
            {
                var response = await GalleryRepository.GetAll();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Update Team 
        public async Task<int> Update(IGalleryDomain entry)
        {
            try
            {
                return await GalleryRepository.Update(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get All Teams Where Tournament Id == Tournament Id
        public async Task<IEnumerable<IGalleryDomain>> GetWhereTournamentId(Guid tournamentId)
        {
            try
            {
                var response = await GalleryRepository.GetAllWhereTournamentId(tournamentId);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
