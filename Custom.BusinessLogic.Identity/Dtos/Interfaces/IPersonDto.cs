using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;
using System;

namespace Custom.BusinessLogic.Identity.Dtos.Interfaces
{
    /// <summary>
    /// Mehdi
    /// </summary>
    public interface IPersonDto : IBasePersonDto
    {
        IUserDto User { get; }
         string FirstName { get; set; }
         string LastName { get; set; }
         string NationalCode { get; set; }
         Gender Gender { get; set; }
    }
}
