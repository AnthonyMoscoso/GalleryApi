using Model.Dto.Auth;
using Model.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.DataServices.Users.Interfaces
{
    public interface IUserService
    {      
        public Task<UserDto> Insert(UserInfo userInfo, UserInputDto user);
    }
}
