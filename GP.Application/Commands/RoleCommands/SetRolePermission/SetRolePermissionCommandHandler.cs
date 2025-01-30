using GP.Core.Enums.Enitity;
using GP.DataAccess.Repository.RoleRepository;
using GP.DataAccess.Repository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository.RolePermissionCategoryRepository;

namespace GP.Application.Commands.RoleCommands.SetRolePermission
{
    public class SetRolePermissionCommandHandler : ICommandHandler<SetRolePermissionCommand, SetRolePermissionResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionCategoryRepository _rolePermissionCategoryRepository;
        private readonly ExceptionService _exceptionService;

        public SetRolePermissionCommandHandler(IUnitOfWork unitOfWork, IRoleRepository roleRepository,
            IRolePermissionCategoryRepository rolePermissionCategoryRepository, ExceptionService exceptionService)
        {
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
            _rolePermissionCategoryRepository = rolePermissionCategoryRepository;
            _exceptionService = exceptionService;
        }

        public async Task<SetRolePermissionResponse> Handle(SetRolePermissionCommand command,
            CancellationToken cancellationToken)
        {

            var role = await _roleRepository.GetRoleByIdAsync(command.Request.RoleId, "Users");
            if (role == null)
                throw _exceptionService.RecordNotFoundException();


            _rolePermissionCategoryRepository.SetGlobalQueryFilterStatus(false);

            var data = _rolePermissionCategoryRepository.FindBy(c => c.RoleId == role.Id).ToList();

            var removedData = data
                .Where(f => !command.Request.PermissionsIds.Contains(f.PermissionCategoryPermissionId)).ToList();

            foreach (var item in removedData)
            {
                _rolePermissionCategoryRepository.Delete(item);
            }

            if (command.Request.PermissionsIds != null && command.Request.PermissionsIds.Any())
            {
                foreach (var requestPermissionsId in command.Request.PermissionsIds)
                {
                    var permission = data.FirstOrDefault(c => c.PermissionCategoryPermissionId == requestPermissionsId);
                    if (permission != null)
                    {
                        permission.Status = RecordStatusEnum.Active;
                    }
                    else
                    {
                        await _rolePermissionCategoryRepository.AddAsync(new RolePermissionCategory()
                        {
                            RoleId = role.Id,
                            PermissionCategoryPermissionId = requestPermissionsId,
                        });
                    }
                }
            }

            //await _roleRepository.UpdateAsync(role).ConfigureAwait(false);
            await _unitOfWork.CompleteAsync(cancellationToken).ConfigureAwait(false);
            _rolePermissionCategoryRepository.SetGlobalQueryFilterStatus(true);

            return new SetRolePermissionResponse();
        }
    }
}
