using Custom.BusinessLogic.Identity.Dtos.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Custom.BusinessLogic.Identity.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPersonDto"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TUserDto"></typeparam>
    /// <typeparam name="TUserDtoKey"></typeparam>
    public class PepoleDto<TPersonDto, TKey, TUserDto, TUserDtoKey> : IPepoleDto
         where TPersonDto : PersonDto<TKey, TUserDto, TUserDtoKey>
      
    {
        public PepoleDto()
        {
            Pepole = new List<TPersonDto>();
        }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<TPersonDto> Pepole { get; set; }

        List<IPersonDto> IPepoleDto.Pepole => Pepole.Cast<IPersonDto>().ToList();
    }
}
