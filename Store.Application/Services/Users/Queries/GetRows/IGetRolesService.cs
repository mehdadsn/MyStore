using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Common.Dto;

namespace Store.Application.Services.Users.Queries.GetRows
{
    public interface IGetRolesService
    {
        ResultDto<List<RolesDto>> Execute();
    }
}
