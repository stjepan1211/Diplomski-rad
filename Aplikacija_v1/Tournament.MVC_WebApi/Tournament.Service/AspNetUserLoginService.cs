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
    public class AspNetUserLoginService : IAspNetUserLoginService
    {
        protected IAspNetUserLoginRepository AspNetUserLoginRepository { get; private set; }
        public AspNetUserLoginService(IAspNetUserLoginRepository rep)
        {
            this.AspNetUserLoginRepository = rep;
        }

        //Add AspNetUserLogin
        public async Task<int> Add(IAspNetUserLoginDomain entry)
        {
            try
            {
                return await AspNetUserLoginRepository.Add(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Delete AspNetUserLogin by id
        public async Task<int> Delete(string id)
        {
            try
            {
                return await AspNetUserLoginRepository.Delete(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Delete AspNetUserLogin by object
        public async Task<int> Delete(IAspNetUserLoginDomain entry)
        {
            try
            {
                return await AspNetUserLoginRepository.Delete(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Get AspNetUserLogin by Id
        public async Task<IAspNetUserLoginDomain> Read(string id)
        {
            try
            {
                var response = await AspNetUserLoginRepository.Get(id);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Get All AspNetUserLogins
        public async Task<IEnumerable<IAspNetUserLoginDomain>> ReadAll()
        {
            try
            {
                var response = await AspNetUserLoginRepository.GetAll();

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Update AspNetUserLogin Entry
        public async Task<int> Update(IAspNetUserLoginDomain entry)
        {
            try
            {
                return await AspNetUserLoginRepository.Update(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
