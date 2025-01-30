using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GP.Domain.Entities.Identity
{
    public class UserToken : IdentityUserToken<string>
    {
      
    }
}
