using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TestCase.Models.User
{
    public class AppUser : IdentityUser<Guid>
    {
        [StringLength(50, ErrorMessageResourceName = "StringLength")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessageResourceName = "StringLength")]
        public string LastName { get; set; }
    }
}
