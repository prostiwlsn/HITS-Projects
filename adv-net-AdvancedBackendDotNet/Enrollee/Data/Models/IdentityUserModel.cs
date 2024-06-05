using Microsoft.AspNetCore.Identity;

namespace Auth.Data.Models
{
    public class IdentityUserModel : IdentityUser
    {
        private new string? UserName {  get; set; } 
    }
}
