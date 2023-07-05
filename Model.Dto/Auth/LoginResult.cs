using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model.Dto.Auth
{
    public class LoginResult
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }
}
