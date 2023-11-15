using Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class File : BaseEntity
{
    public string FileName { get; set; }
    public string Path { get; set; }
    
    [NotMapped]
    public override DateTime LastModified { get => base.LastModified; set => base.LastModified = value; }
}