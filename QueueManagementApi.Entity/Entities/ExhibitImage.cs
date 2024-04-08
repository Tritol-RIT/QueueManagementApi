using QueueManagementApi.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueueManagementApi.Core.Entities;

public class ExhibitImage : BaseEntity
{
    [Required]
    public string ImageUrl { get; set; }

    [Required]
    public int ExhibitId { get; set; }

    [Required]
    public int DisplayOrder { get; set; }

    // Navigation properties
    [ForeignKey(nameof(ExhibitId))]
    public virtual Exhibit Exhibit { get; set; }
}