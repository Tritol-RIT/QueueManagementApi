using QueueManagementApi.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueueManagementApi.Core.Entities;

public class Insurance : BaseEntity
{
    public int VisitorId { get; set; }
    public DateTime ApprovalTime { get; set; }
    public string VisitorImageUrl { get; set; }

    // Navigation properties
    [ForeignKey(nameof(VisitorId))]
    public virtual Visitor Visitor { get; set; }
}