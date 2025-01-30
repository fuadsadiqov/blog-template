using GP.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace GP.Application.Commands.UserCommands.StartPasswordRestoration
{
    public class StartPasswordRestorationRequest
    {
        [Required(ErrorMessage = ResourceKey.Required)]
        public string EmailOrUsername { get; set; }
    }
}
