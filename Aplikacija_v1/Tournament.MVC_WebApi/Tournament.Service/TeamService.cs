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
    public class TeamService : ITeamService
    {
        protected ITeamRepository TeamRepository { get; set; }

        public TeamService(ITeamRepository rep)
        {
            this.TeamRepository = rep;
        }

        //Add Team
        public async Task<int> Add(ITeamDomain entry)
        {
            try
            {
                return await TeamRepository.Add(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Team by id
        public async Task<int> Delete(Guid id)
        {
            try
            {
                return await TeamRepository.Delete(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Delete Team by object
        public async Task<int> Delete(ITeamDomain entry)
        {
            try
            {
                return await TeamRepository.Delete(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get Team by Id
        public async Task<ITeamDomain> Read(Guid id)
        {
            try
            {
                var response = await TeamRepository.Get(id);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //Get All Teams
        public async Task<IEnumerable<ITeamDomain>> ReadAll()
        {
            try
            {
                var response = await TeamRepository.GetAll();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Update Team 
        public async Task<int> Update(ITeamDomain entry)
        {
            try
            {
                return await TeamRepository.Update(entry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
