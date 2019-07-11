using System;
using System.ComponentModel.DataAnnotations;
using Custom.BusinessLogic.Identity.Dtos.Base;
using Custom.BusinessLogic.Identity.Dtos.Interfaces;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Custom.BusinessLogic.Identity.Dtos
{
    /// <summary>
    /// Mehdi
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TUserDto"></typeparam>
    /// <typeparam name="TUserDtoKey"></typeparam>
    public class PersonDto<TKey, TUserDto, TUserDtoKey> : BasePersonDto<TKey, TUserDtoKey>, IPersonDto
    {
        public PersonDto()
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public Gender Gender { get; set; }
        public TUserDto User { get; set; }
        IUserDto IPersonDto.User => (IUserDto)User;

       
    }
}
