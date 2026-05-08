namespace WMS.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public long? CreatedBy { get; set; }
    public long? UpdatedBy { get; set; }
    public long? DeletedBy { get; set; }
}