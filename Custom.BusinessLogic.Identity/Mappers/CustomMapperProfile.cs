using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Custom.BusinessLogic.Identity.Dtos;
using Custom.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.IdentityServer4.Admin.EntityFramework.Extensions.Common;

namespace Custom.BusinessLogic.Identity.Mappers
{
    public class CustomMapperProfile<TPersonDto, TPersonDtoKey, TPerson, TUser, TUserDto, TUserDtoKey, TPepoleDto, TKey, TUserKey>
      : Profile
     
      where TPersonDto : PersonDto<TKey, TUserDto, TUserDtoKey>
      where TUserDto : UserDto<TUserDtoKey>
      where TUser : IdentityUser<TUserKey>
      where TPerson : Person
      where TPepoleDto : PepoleDto<TPersonDto, TKey, TUserDto, TUserDtoKey>
      where TKey : IEquatable<TKey>
      where TUserKey : IEquatable<TUserKey>
        
    {
        public CustomMapperProfile()
        {
            // entity to model
            CreateMap<TPerson, TPersonDto>(MemberList.Destination);
            CreateMap<TUser, TUserDto>(MemberList.Destination);
            CreateMap<PagedList<TPerson>, TPepoleDto>(MemberList.Destination)
               .ForMember(x => x.Pepole,
                   opt => opt.MapFrom(src => src.Data));

            // model to entity
            CreateMap<TPersonDto, TPerson>(MemberList.Source);
            CreateMap<TUserDto, TUser>(MemberList.Source);

        }
    }
}
