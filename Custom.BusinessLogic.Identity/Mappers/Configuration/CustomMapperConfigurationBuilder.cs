using System;
using System.Collections.Generic;
using Custom.BusinessLogic.Identity.Dtos;
using Custom.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;

namespace Custom.BusinessLogic.Identity.Mappers.Configuration
{
    public class CustomMapperConfigurationBuilder : ICustomMapperConfigurationBuilder
    {
        public HashSet<Type> ProfileTypes { get; } = new HashSet<Type>();

        public ICustomMapperConfigurationBuilder AddProfilesType(HashSet<Type> profileTypes)
        {
            if (profileTypes == null) return this;

            foreach (var profileType in profileTypes)
            {
                ProfileTypes.Add(profileType);
            }

            return this;
        }


        public ICustomMapperConfigurationBuilder UseCustomMappingProfile<TPersonDto, TPersonDtoKey, TPerson, TUser, TUserDto, TUserDtoKey, TPepoleDto, TKey, TUserKey>()
            where TPersonDto : PersonDto<TKey, TUserDto, TUserDtoKey>
            where TUserDto : UserDto<TUserDtoKey>
            where TUser : IdentityUser<TUserKey>
            where TPerson : Person
            where TPepoleDto : PepoleDto<TPersonDto, TKey, TUserDto, TUserDtoKey>
            where TKey : IEquatable<TKey>
            where TUserKey : IEquatable<TUserKey>
        {
            ProfileTypes.Add(typeof(CustomMapperProfile<TPersonDto, TPersonDtoKey, TPerson, TUser, TUserDto, TUserDtoKey, TPepoleDto, TKey, TUserKey>));
            return this;
        }

    }
}