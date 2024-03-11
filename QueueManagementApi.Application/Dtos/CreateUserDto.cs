using QueueManagementApi.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace QueueManagementApi.Application.Dtos;

public class CreateUserDto
{
    [EmailAddress]
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public UserRole UserRole { get; set; }
    public int? ExhibitId { get; set; } = null;
}