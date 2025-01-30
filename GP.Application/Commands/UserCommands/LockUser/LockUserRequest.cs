using GP.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace GP.Application.Commands.UserCommands.LockUser
{
    public class LockUserRequest
    {
        [Required(ErrorMessage = ResourceKey.Required)]
        public string UserId { get; set; }
        [Required(ErrorMessage = ResourceKey.Required)]
        public DateTime ExpireDate { get; set; }
    }
}
