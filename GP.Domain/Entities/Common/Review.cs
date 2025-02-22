using System.ComponentModel.DataAnnotations.Schema;
using GP.Core.Configurations.Entity;
using GP.Core.Enums.Enitity;
using GP.Domain.Entities.Identity;

namespace GP.Domain.Entities.Common;

public class Review: IEntity
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    [ForeignKey(nameof(User))]
    public string UserId { get; set; }
    [ForeignKey(nameof(Blog))]
    public Guid BlogId { get; set; }
    public RecordStatusEnum Status { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
    public User User { get; set; }
    public Blog Blog { get; set; }
}