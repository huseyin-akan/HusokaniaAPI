using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public class AppUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool Status { get; set; }
    public DateTime Created { get; set; }
    public virtual DateTime LastModified { get; set; }
    public virtual DateTime? Deleted { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public Guid? DeletedBy { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}