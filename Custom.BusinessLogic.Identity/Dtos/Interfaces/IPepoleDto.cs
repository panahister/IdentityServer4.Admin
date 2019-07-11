using System;
using System.Collections.Generic;
using System.Text;

namespace Custom.BusinessLogic.Identity.Dtos.Interfaces
{
    public interface IPepoleDto
    {
        int PageSize { get; set; }
        int TotalCount { get; set; }
        List<IPersonDto> Pepole { get; }
    }
}
