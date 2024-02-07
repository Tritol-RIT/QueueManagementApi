using QueueManagementApi.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueueManagementApi.Core.Entities;

public class Insurance : BaseEntity
{
    public int VisitorId { get; set; }
    public int ExhibitId { get; set; }
    public bool VisitorPhotoIdRequired { get; set; }
    public DateTime ApprovalTime { get; set; }

    // Navigation properties
    [ForeignKey(nameof(VisitorId))]
    public virtual Visitor Visitor { get; set; }

    [ForeignKey(nameof(ExhibitId))]
    public virtual Exhibit Exhibit { get; set; }
}