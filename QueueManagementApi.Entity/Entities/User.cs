using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Converters;
using QueueManagementApi.Core.Enums;

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
    // use both json ignores so we can ensure that even if our serializer is being mismatched we wont return the password hash.
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public string? PasswordHash { get; set; }

    // Role as enum
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public UserRole Role { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
    
    public int? ExhibitId { get; set; }

    [ForeignKey(nameof(ExhibitId))]
    public virtual Exhibit? Exhibit { get; set; }

    public bool Active { get; set; } = true;
}