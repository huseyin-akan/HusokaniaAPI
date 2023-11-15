using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services.Security;

public class TokenOptions
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public int JwtTokenExpiration { get; set; }
    public string SecurityKey { get; set; }
    public int RefreshTokenExpiration { get; set; }
}
