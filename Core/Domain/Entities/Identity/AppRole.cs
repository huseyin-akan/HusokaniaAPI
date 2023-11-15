using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public class AppRole : IdentityRole<Guid>
{
    public AppRole(string role) : base(role)
    {}

    public AppRole() : base()
    {}
}
