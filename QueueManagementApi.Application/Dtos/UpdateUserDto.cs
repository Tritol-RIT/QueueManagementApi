using System.ComponentModel.DataAnnotations;

namespace QueueManagementApi.Application.Dtos;

public class UserUpdateDto
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(75)]
    public string LastName { get; set; }

    public int? ExhibitId { get; set; }

    public bool Active { get; set; }
}
