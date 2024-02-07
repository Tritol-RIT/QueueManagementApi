using QueueManagementApi.Core;
using System.ComponentModel.DataAnnotations;

namespace QueueManagementApi.Core.Entities;

public class Group : BaseEntity
{
    [Required]
    public int NumberOfVisitors { get; set; }

    [Required]
    public DateTime VisitDate { get; set; }

    // Navigation properties
    public virtual Visit Visit { get; set; }
    public virtual ICollection<Visitor> Visitors { get; set; }
}