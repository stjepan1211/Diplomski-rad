using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.DAL;
using Tournament.Model.Common;
using Tournament.Repository.Common.IGenericRepository;
using Tournament.Repository.Common.IRepositories;
using AutoMapper;
using System.Data.Entity;

namespace Tournament.Repository.Repositories
{
    public class AspNetUserRepository : IAspNetUserRepository
    {
        protected IGenericRepository GenericRepository { get; set; }

        public AspNetUserRepository(IGenericRepository genericRepository)
        {
            this.GenericRepository = genericRepository;
        }

        //Create new AspNetUser
        public async Task<int> Add(IAspNetUserDomain entity)
        {
            try
            {
                return await GenericRepository.Add(Mapper.Map<AspNetUser>(entity));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //Delete AspNetUser by Id
        public async Task<int> Delete(string id)
        {
            try
            {
                var item = await GenericRepository.Get<AspNetUser>(id);

                if (item == null)
                    return 0;

                return await GenericRepository.Delete(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete AspNetUser by Object
        public async Task<int> Delete(IAspNetUserDomain entity)
        {
            try
            {
                return await GenericRepository.Delete(Mapper.Map<AspNetUser>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get AspNetUser by Id
        public async Task<IAspNetUserDomain> Get(string id)
        {
            try
            {
                var response = Mapper.Map<IAspNetUserDomain>(await GenericRepository.Get<AspNetUser>(id));
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get all AspNetUser
        public async Task<IEnumerable<IAspNetUserDomain>> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<IAspNetUserDomain>>(await GenericRepository.GetAll<AspNetUser>());
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Update AspNetUser
        public async Task<int> Update(IAspNetUserDomain entity)
        {
            try
            {
                return await GenericRepository.Update(Mapper.Map<AspNetUser>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get user by username
        public async Task<IAspNetUserDomain> GetByUsername(string username)
        {
            try
            {
                var response = Mapper.Map<IAspNetUserDomain>(await GenericRepository
                    .GetQueryable<AspNetUser>().Where(x => x.UserName == username)
                    .Include(a => a.Tournaments).FirstOrDefaultAsync());
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
