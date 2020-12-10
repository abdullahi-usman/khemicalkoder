using Microsoft.AspNetCore.Identity;

namespace KhemicalKoder.Models
{
    public class KhemicalKoderUser : IdentityUser
    {
        [PersonalData] public string Name { set; get; }
        public bool IsAdmin { set; get; }
    }
}