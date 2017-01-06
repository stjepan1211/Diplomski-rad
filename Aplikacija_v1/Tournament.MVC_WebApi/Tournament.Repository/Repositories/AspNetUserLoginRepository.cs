using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.DAL;
using Tournament.Model.Common;
using Tournament.Repository.Common.IGenericRepository;
using Tournament.Repository.Common.IRepositories;

namespace Tournament.Repository.Repositories
{
    public class AspNetUserLoginRepository : IAspNetUserLoginRepository
    {
        protected IGenericRepository GenericRepository { get; set; }

        public AspNetUserLoginRepository(IGenericRepository genericRepository)
        {
            this.GenericRepository = genericRepository;
        }

        //Create new AspNetUserLogin
        public async Task<int> Add(IAspNetUserLoginDomain entity)
        {
            try
            {
                return await GenericRepository.Add(Mapper.Map<AspNetUserLogin>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete AspNetUserLogin by Id
        public async Task<int> Delete(string id)
        {
            try
            {
                var item = await GenericRepository.Get<AspNetUserLogin>(id);

                if (item == null)
                    return 0;

                return await GenericRepository.Delete(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete AspNetUserLogin by Object
        public async Task<int> Delete(IAspNetUserLoginDomain entity)
        {
            try
            {
                return await GenericRepository.Delete(Mapper.Map<AspNetUserLogin>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get AspNetUserLogin by Id
        public async Task<IAspNetUserLoginDomain> Get(string id)
        {
            try
            {
                var response = Mapper.Map<IAspNetUserLoginDomain>(await GenericRepository.Get<AspNetUserLogin>(id));
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get all AspNetUserLogins
        public async Task<IEnumerable<IAspNetUserLoginDomain>> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<IAspNetUserLoginDomain>>(await GenericRepository.GetAll<AspNetUserLogin>());
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Update AspNetUserLogin
        public async Task<int> Update(IAspNetUserLoginDomain entity)
        {
            try
            {
                return await GenericRepository.Update(Mapper.Map<AspNetUserLogin>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
