using QueueManagementApi.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueueManagementApi.Core.Entities;

public class Visitor : BaseEntity, IAuditable
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public int GroupId { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    // Navigation properties
    [ForeignKey(nameof(GroupId))]
    public virtual Group Group { get; set; }

    public virtual ICollection<Insurance> Insurances { get; set; }
}