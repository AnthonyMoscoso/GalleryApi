using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto.Auth
{
    public class LoginRequest
    {
        public string Identifier { get; set; } = string.Empty;
        public string Credential { get; set; } = string.Empty;
    }


}
