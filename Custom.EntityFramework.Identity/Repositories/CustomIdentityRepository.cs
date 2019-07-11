using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Skoruba.IdentityServer4.Admin.EntityFramework.Extensions.Common;
using Skoruba.IdentityServer4.Admin.EntityFramework.Extensions.Enums;
using Skoruba.IdentityServer4.Admin.EntityFramework.Extensions.Extensions;
using Custom.EntityFramework.Identity.Repositories.Interfaces;
using Custom.EntityFramework.DbContexts;
using Custom.EntityFramework.Entities;

namespace Custom.EntityFramework.Identity.Repositories
{
    public class CustomIdentityRepository<ICustomDbContext, TPerson, TKey>
        : ICustomIdentityRepository<TPerson, TKey>
        where ICustomDbContext : CustomDbContext
        where TPerson : Person
        where TKey : IEquatable<TKey>
    {
        protected readonly ICustomDbContext DbContext;
        protected readonly IMapper Mapper;

        public bool AutoSaveChanges { get; set; } = true;

        public CustomIdentityRepository(ICustomDbContext dbContext,
            IMapper mapper)
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        private async Task<int> AutoSaveChangesAsync()
        {
            return AutoSaveChanges ? await DbContext.SaveChangesAsync() : (int)SavedStatus.WillBeSavedExplicitly;
        }

        public async Task<bool> ExistsPersonAsync(long personId)
        {
            return await DbContext.Set<TPerson>().AnyAsync(c => c.Id == personId);
        }

        public async Task<PagedList<TPerson>> GetPersonAsync(int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<TPerson>();
            var pepole = await DbContext.Set<TPerson>()
                .PageBy(x => x.Id, page, pageSize)
                .ToListAsync();

            pagedList.Data.AddRange(pepole);
            pagedList.TotalCount = await DbContext.Set<TPerson>().CountAsync();
            pagedList.PageSize = pageSize;

            return pagedList;
        }

        public async Task<TPerson> GetPersonAsync(long personId)
        {
            var person = await DbContext.Set<TPerson>().Where(c => c.Id == personId).Include(blog => blog.UserIdentity).SingleOrDefaultAsync();
            return person;
        }

        public async Task<TPerson> GetPersonByUserIdAsync(string userId)
        {
            var person = await DbContext.Set<TPerson>().Where(c => c.UserId == userId).SingleOrDefaultAsync();
            return person;
        }

        public async Task<int> CreatePersonAsync(TPerson person)
        {
            await DbContext.Set<TPerson>().AddAsync(person);
            return await AutoSaveChangesAsync();
        }

        public async Task<int> UpdatePersonAsync(TPerson person)
        {
             DbContext.Set<TPerson>().Update(person);
            return await AutoSaveChangesAsync();
        }

        public async Task<int> DeletePersonAsync(long personId)
        {
            var person = await DbContext.Set<TPerson>().Where(x => x.Id == personId).SingleOrDefaultAsync();

            DbContext.Pepole.Remove(person);

            return await AutoSaveChangesAsync();
        }

        public async Task<int> DeletePersonByUserIdAsync(string userId)
        {
            var person = await DbContext.Set<TPerson>().Where(x => x.UserId == userId).SingleOrDefaultAsync();

            DbContext.Pepole.Remove(person);

            return await AutoSaveChangesAsync();
        }
    }
}