using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Domain.Entities.Identity
{
    public class AppRole : IdentityRole<Guid>
    {
        public AppRole(string role) : base(role)
        {}

        public AppRole() : base()
        {}
    }
}
