using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.DAL;
using Tournament.Model.Common;
using Tournament.Repository.Common.IGenericRepository;
using Tournament.Repository.Common.IRepositories;

namespace Tournament.Repository.Repositories
{
    public class GalleryRepository : IGalleryRepository
    {
        protected IGenericRepository GenericRepository { get; set; }

        public GalleryRepository(IGenericRepository genericRepository)
        {
            this.GenericRepository = genericRepository;
        }

        //Create new Gallery
        public async Task<int> Add(IGalleryDomain entity)
        {
            try
            {
                return await GenericRepository.Add(Mapper.Map<Gallery>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Gallery by Id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                var item = await GenericRepository.Get<Gallery>(id);

                if (item == null)
                    return 0;

                return await GenericRepository.Delete(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Gallery by Object
        public async Task<int> Delete(IGalleryDomain entity)
        {
            try
            {
                return await GenericRepository.Delete(Mapper.Map<Gallery>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Gallery by Id
        public async Task<IGalleryDomain> Get(Guid id)
        {
            try
            {
                var response = Mapper.Map<IGalleryDomain>(await GenericRepository.Get<Gallery>(id));
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get all Galleries
        public async Task<IEnumerable<IGalleryDomain>> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<IGalleryDomain>>(await GenericRepository.GetAll<Gallery>());
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ////Get Images By Tournament
        //public async Task<IEnumerable<IGalleryDomain>> GetByTournament(Guid tournamentId)
        //{
        //    try
        //    {
        //        var response = Mapper.Map<IEnumerable<IGalleryDomain>>(await GenericRepository.GetQueryable<Gallery>()
        //                .Where(g => g.TournamentId == tournamentId).ToListAsync());
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //Update Gallery
        public async Task<int> Update(IGalleryDomain entity)
        {
            try
            {
                return await GenericRepository.Update(Mapper.Map<Gallery>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Gallery Where TournamentId
        public async Task<IEnumerable<IGalleryDomain>> GetAllWhereTournamentId(Guid tournamentId)
        {
            try
            {
                var response = Mapper.Map<IEnumerable<IGalleryDomain>>
                    (await GenericRepository.GetQueryable<Gallery>().Where(g => g.TournamentId == tournamentId).ToListAsync());
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
