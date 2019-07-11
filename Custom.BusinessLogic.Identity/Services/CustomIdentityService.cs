using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Custom.BusinessLogic.Identity.Dtos;
using Custom.BusinessLogic.Identity.Services.Interfaces;
using Custom.EntityFramework.Entities;
using Custom.EntityFramework.Identity.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Resources;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Services.Interfaces;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Shared.Dtos.Common;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Shared.ExceptionHandling;
using Skoruba.IdentityServer4.Admin.EntityFramework.Extensions.Common;
using Skoruba.IdentityServer4.Admin.EntityFramework.Identity.Repositories.Interfaces;

namespace Custom.BusinessLogic.Identity.Services
{
    public class CustomIdentityService<TPersonDto, TPersonDtoKey, TPerson, TUserDto, TUserDtoKey,
            TKey, TPepoleDto> : ICustomIdentityService<TPersonDto, TPersonDtoKey, TPerson, TUserDto, TUserDtoKey,
            TKey, TPepoleDto>
            where TPersonDto : PersonDto<TKey, TUserDto, TUserDtoKey>
            where TUserDto : UserDto<TUserDtoKey>
            where TPerson : Person
            where TKey : IEquatable<TKey>
            where TPepoleDto : PepoleDto<TPersonDto, TKey, TUserDto, TUserDtoKey>
    {
        protected readonly ICustomIdentityRepository<TPerson, TKey> CustomIdentityRepository;
        protected readonly IIdentityServiceResources IdentityServiceResources;
        protected readonly IMapper Mapper;

        public CustomIdentityService(ICustomIdentityRepository<TPerson, TKey> customIdentityRepository,
            IIdentityServiceResources identityServiceResources,
            IMapper mapper)
        {
            CustomIdentityRepository = customIdentityRepository;
            IdentityServiceResources = identityServiceResources;
            Mapper = mapper;
        }


        public virtual async Task<bool> ExistsPersonAsync(long personId)
        {
            var exists = await CustomIdentityRepository.ExistsPersonAsync(personId);
            //if (!exists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userId), IdentityServiceResources.UserDoesNotExist().Description);
            if (!exists) throw new UserFriendlyErrorPageException("");

            return true;
        }

        public virtual async Task<TPepoleDto> GetPersonAsync(int page = 1, int pageSize = 10)
        {
            var pagedList = await CustomIdentityRepository.GetPersonAsync(page, pageSize);
            var pepoleDto = Mapper.Map<TPepoleDto>(pagedList);
            return pepoleDto;
        }

        public virtual async Task<TPersonDto> GetPersonAsync(long personId)
        {
           var person = await CustomIdentityRepository.GetPersonAsync(personId);
            //if (person == null) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.RoleDoesNotExist().Description, roleId), IdentityServiceResources.RoleDoesNotExist().Description);
            if (person == null) throw new UserFriendlyErrorPageException("");
            var personDto = Mapper.Map<TPersonDto>(person);
            return personDto;
        }

        public virtual async Task<TPersonDto> GetPersonByUserIdAsync(string userId)
        {
            var person = await CustomIdentityRepository.GetPersonByUserIdAsync(userId);
            //if (person == null) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.RoleDoesNotExist().Description, roleId), IdentityServiceResources.RoleDoesNotExist().Description);
            if (person == null) throw new UserFriendlyErrorPageException("");
            var personDto = Mapper.Map<TPersonDto>(person);
            return personDto;
        }

        public virtual async Task<int> CreatePersonAsync(TPersonDto dto)
        {
            var person = Mapper.Map<TPerson>(dto);
            return  await CustomIdentityRepository.CreatePersonAsync(person);
        }

        public virtual async Task<int> UpdatePersonAsync(TPersonDto dto)
        {
            var person = Mapper.Map<TPerson>(dto);

            return await CustomIdentityRepository.UpdatePersonAsync(person);
        }

        public virtual async Task<int> DeletePersonAsync(long personId)
        {
            return await CustomIdentityRepository.DeletePersonAsync(personId);
        }

        public virtual async Task<int> DeletePersonByUserIdAsync(string userId)
        {
            return await CustomIdentityRepository.DeletePersonByUserIdAsync(userId);
        }
    }
}