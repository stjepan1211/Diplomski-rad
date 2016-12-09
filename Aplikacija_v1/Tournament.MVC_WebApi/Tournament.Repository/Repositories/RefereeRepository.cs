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

namespace Tournament.Repository.Repositories
{
    public class RefereeRepository : IRefereeRepository
    {
        protected IGenericRepository GenericRepository { get; set; }

        public RefereeRepository(IGenericRepository genericRepository)
        {
            this.GenericRepository = genericRepository;
        }

        //Create new Referee
        public async Task<int> Add(IRefereeDomain entity)
        {
            try
            {
                return await GenericRepository.Add(Mapper.Map<Referee>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Referee by Id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                var item = await GenericRepository.Get<Referee>(id);

                if (item == null)
                    return 0;

                return await GenericRepository.Delete(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Referee by Object
        public async Task<int> Delete(IRefereeDomain entity)
        {
            try
            {
                return await GenericRepository.Delete(Mapper.Map<Referee>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Referee by Id
        public async Task<IRefereeDomain> Get(Guid id)
        {
            try
            {
                var response = Mapper.Map<IRefereeDomain>(await GenericRepository.Get<Referee>(id));
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get all Referee
        public async Task<IEnumerable<IRefereeDomain>> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<IRefereeDomain>>(await GenericRepository.GetAll<Referee>());
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Update Referee
        public async Task<int> Update(IRefereeDomain entity)
        {
            try
            {
                return await GenericRepository.Update(Mapper.Map<Referee>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
