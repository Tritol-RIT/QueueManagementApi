using QueueManagementApi.Core;
using System.ComponentModel.DataAnnotations;

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
    public DateTime InitialDuration { get; set; }

    [Required]
    public bool InsuranceFormRequired { get; set; }

    [Required]
    public bool AgeRequired { get; set; }

    public string? InsuranceFormFileUrl { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public DateTime? UpdatedOn { get; set; }

    // Navigation properties
    public virtual ICollection<User> Users { get; set; }
    public virtual ICollection<Visit> Visits { get; set; }
    public virtual ICollection<ExhibitImage> ExhibitImages { get; set; }
    public virtual ICollection<Insurance> Insurances { get; set; }
}