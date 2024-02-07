using QueueManagementApi.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public string Email { get; set; }

    [Required]
    [StringLength(15)]
    public string Password { get; set; }

    [Required]
    [StringLength(1)]
    public char Role { get; set; } // change this to enum?

    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public DateTime? UpdatedOn { get; set; }

    [Required]
    public int? ExhibitId { get; set; }

    [ForeignKey(nameof(ExhibitId))]
    public virtual Exhibit? Exhibit { get; set; }
}