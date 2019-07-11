using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Custom.BusinessLogic.Identity.Dtos;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using Custom.EntityFramework.Entities;

namespace Custom.BusinessLogic.Identity.Mappers.Configuration
{
    public interface ICustomMapperConfigurationBuilder
    {
        HashSet<Type> ProfileTypes { get; }

        ICustomMapperConfigurationBuilder AddProfilesType(HashSet<Type> profileTypes);

        ICustomMapperConfigurationBuilder UseCustomMappingProfile<TPersonDto, TPersonDtoKey, TPerson, TUser, TUserDto, TUserDtoKey, TPepoleDto, TKey, TUserKey>()
             where TPersonDto : PersonDto<TKey, TUserDto, TUserDtoKey>
            where TUserDto : UserDto<TUserDtoKey>
            where TUser : IdentityUser<TUserKey>
            where TPerson : Person
            where TPepoleDto : PepoleDto<TPersonDto, TKey, TUserDto, TUserDtoKey>
            where TKey : IEquatable<TKey>
            where TUserKey : IEquatable<TUserKey>;
    }
}
