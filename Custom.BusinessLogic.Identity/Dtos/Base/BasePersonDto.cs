using Custom.BusinessLogic.Identity.Dtos.Interfaces;
using System.Collections.Generic;

namespace Custom.BusinessLogic.Identity.Dtos.Base
{
    /// <summary>
    /// Mehdi
    /// </summary>
    /// <typeparam name="TPersonId"></typeparam>
    /// <typeparam name="TUserDtoKey"></typeparam>
    public class BasePersonDto<TPersonId,TUserDtoKey> : IBasePersonDto
    {
        public TPersonId Id { get; set; }
        public TUserDtoKey UserId { get; set; }

        public bool IsDefaultId() => EqualityComparer<TPersonId>.Default.Equals(Id, default(TPersonId));

        object IBasePersonDto.Id => Id;
        object IBasePersonDto.UserId => UserId;
    }
}