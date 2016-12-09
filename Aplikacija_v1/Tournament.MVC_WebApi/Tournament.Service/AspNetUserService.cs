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
    public class AspNetUserService : IAspNetUserService
    {
        protected IAspNetUserRepository AspNetUserRepository { get; private set; }

        public AspNetUserService(IAspNetUserRepository rep)
        {
            this.AspNetUserRepository = rep;
        }

        //Add AspNetUser
        public async Task<int> Add(IAspNetUserDomain entry)
        {
            try
            {
                return await AspNetUserRepository.Add(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete AspNetUser by id
        public async Task<int> Delete(string id)
        {
            try
            {
                return await AspNetUserRepository.Delete(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete AspNetUser by object
        public async Task<int> Delete(IAspNetUserDomain entry)
        {
            try
            {
                return await AspNetUserRepository.Delete(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get AspNetUser by Id
        public async Task<IAspNetUserDomain> Read(string id)
        {
            try
            {
                var response = await AspNetUserRepository.Get(id);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get All AspNetUser
        public async Task<IEnumerable<IAspNetUserDomain>> ReadAll()
        {
            try
            {
                var response = await AspNetUserRepository.GetAll();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Update AspNetUser 
        public async Task<int> Update(IAspNetUserDomain entry)
        {
            try
            {
                return await AspNetUserRepository.Update(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Find AspNetUser By username
        public async Task<IAspNetUserDomain> FindByUserName(string userName)
        {
            try
            {
                var user = await AspNetUserRepository.GetByUsername(userName);
                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
