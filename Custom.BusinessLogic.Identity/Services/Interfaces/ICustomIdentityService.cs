using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Custom.BusinessLogic.Identity.Dtos;
using Custom.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;

namespace Custom.BusinessLogic.Identity.Services.Interfaces
{
    public interface ICustomIdentityService<TPersonDto, TPersonDtoKey, TPerson, TUserDto, TUserDtoKey,
            TKey, TPepoleDto>
       where TPersonDto : PersonDto<TKey, TUserDto, TUserDtoKey>
            where TUserDto : UserDto<TUserDtoKey>
            where TPerson : Person
            where TKey : IEquatable<TKey>
          where TPepoleDto : PepoleDto<TPersonDto, TKey, TUserDto, TUserDtoKey>
    {
        Task<bool> ExistsPersonAsync(long personId);


        Task<TPepoleDto> GetPersonAsync(int page = 1, int pageSize = 10);
        Task<TPersonDto> GetPersonAsync(long personId);
        Task<TPersonDto> GetPersonByUserIdAsync(string userId);

        Task<int> CreatePersonAsync(TPersonDto person);

        Task<int> UpdatePersonAsync(TPersonDto person);

        Task<int> DeletePersonAsync(long personId);
        Task<int> DeletePersonByUserIdAsync(string userId);
        //Task CreatePersonAsync<TPersonDto, TKey, TUserDto, TUserDtoKey>(TPersonDto dto)
        //    where TPersonDto : PersonDto<TKey, TUserDto, TUserDtoKey>, new()
        //    where TKey : IEquatable<TKey>
        //    where TUserDto : UserDto<TUserDtoKey>, new();
    }
}