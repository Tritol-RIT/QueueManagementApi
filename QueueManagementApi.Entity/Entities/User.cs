using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QueueManagementApi.Core.Entities;

public class User : BaseEntity, IAuditable
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(75)]
    public string LastName { get; set; }

    [Required]
    [StringLength(255)]
    [EmailAddress]
    public string Email { get; set; }

    // Don't store plain passwords. This field should store a hash.
    [Required]
    [JsonIgnore]
    public string PasswordHash { get; set; }

    // Role as enum
    [Required]
    public UserRole Role { get; set; } // Assuming UserRole is an enum

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedOn { get; set; }
    
    public int? ExhibitId { get; set; }

    [ForeignKey(nameof(ExhibitId))]
    public virtual Exhibit? Exhibit { get; set; }

    public bool Active { get; set; } = true;
}

public enum UserRole
{
    Admin,
    Staff,
    Committee
}
