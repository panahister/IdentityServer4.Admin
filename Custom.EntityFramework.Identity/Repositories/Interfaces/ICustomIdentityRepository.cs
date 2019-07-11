using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Custom.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Skoruba.IdentityServer4.Admin.EntityFramework.Extensions.Common;

namespace Custom.EntityFramework.Identity.Repositories.Interfaces
{
	public interface ICustomIdentityRepository<TPerson, TKey>	    
	    where TPerson : Person
       where TKey : IEquatable<TKey>
    {
        Task<bool> ExistsPersonAsync(long personId);
        Task<PagedList<TPerson>> GetPersonAsync(int page = 1, int pageSize = 10);
        Task<TPerson> GetPersonAsync(long personId);
        Task<TPerson> GetPersonByUserIdAsync(string userId);
        Task<int> CreatePersonAsync(TPerson person);
        Task<int> UpdatePersonAsync(TPerson person);
        Task<int> DeletePersonAsync(long personId);
        Task<int> DeletePersonByUserIdAsync(string userId);
    }
}