﻿namespace Domain.Entities.Common;

public class BaseEntity
{
    public Guid Id { get; set; }
    public bool Status { get; set; }
    public DateTime Created { get; set; }
    public virtual DateTime LastModified { get; set; }
    public virtual DateTime? Deleted { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public Guid? DeletedBy { get; set; }
}