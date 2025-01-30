using System;
using GP.Core.Enums.Enitity;

namespace GP.Core.Configurations.Entity
{
    public interface IEntity
    {
        RecordStatusEnum Status { get; set; }
        DateTime DateCreated { get; set; }
        DateTime? DateModified { get; set; }
        DateTime? DateDeleted { get; set; }
    }
}
