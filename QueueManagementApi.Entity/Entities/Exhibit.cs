using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueueManagementApi.Core.Entities;

public class Exhibit : BaseEntity, IAuditable
{
    [Required, StringLength(100)]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public int MaxCapacity { get; set; }

    [Required]
    public int InitialDuration { get; set; }

    public int? CurrentDuration { get; set; }

    [Required]
    public bool InsuranceFormRequired { get; set; }

    [Required]
    public bool AgeRequired { get; set; }

    public int? AgeMinimum { get; set; }

    public string? InsuranceFormFileUrl { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? CategoryId { get; set; }

    // Navigation properties
    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; }

    public virtual ICollection<User> Users { get; set; }
    public virtual ICollection<Visit> Visits { get; set; }
    public virtual ICollection<ExhibitImage> ExhibitImages { get; set; }
    public virtual ICollection<Insurance> Insurances { get; set; }
}