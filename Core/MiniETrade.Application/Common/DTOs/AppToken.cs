using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.DTOs;

public class AppToken
{
    public string? JwtToken { get; set; }
    public DateTime JwtTokenExpiration { get; set; }
    public string? RefreshToken { get; set; }
}