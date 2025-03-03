﻿using GP.Core.Constants;
using GP.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace GP.Application.Commands.RoleCommands.UpdateRole
{
    public class UpdateRoleRequest
    {
        public string Id { get; set; }
        [Required(ErrorMessage = ResourceKey.Required)]
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(StringLengthConstants.Medium)]
        public string Description { get; set; }
        public List<int> PermissionIds { get; set; }
        public List<string> UserIds { get; set; }
    }
}
