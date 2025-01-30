using GP.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace GP.Application.Commands.UserCommands.UnLockUser
{
    public class UnLockUserRequest
    {
        [Required(ErrorMessage = ResourceKey.Required)]
        public string UserId { get; set; }
    }
}
