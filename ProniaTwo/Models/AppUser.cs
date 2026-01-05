using Microsoft.AspNetCore.Identity;

namespace ProniaTwo.Models;

public class AppUser:IdentityUser
{
    public string FullName { get; set; }

}
