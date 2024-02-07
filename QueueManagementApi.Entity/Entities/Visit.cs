using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueueManagementApi.Core.Entities;

public class Visit : BaseEntity, IAuditable
{
    [Required]
    public int GroupId { get; set; }

    [Required]
    public int ExhibitId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    [Required]
    public string QrCode { get; set; }

    public string QrCodeUrl { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public DateTime? UpdatedOn { get; set; }

    // Navigation properties
    [ForeignKey(nameof(GroupId))]
    public virtual Group Group { get; set; }

    [ForeignKey(nameof(ExhibitId))]
    public virtual Exhibit Exhibit { get; set; }
}