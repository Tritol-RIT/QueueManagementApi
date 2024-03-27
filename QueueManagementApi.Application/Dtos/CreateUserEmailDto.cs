using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Application.Dtos;

public class CreateUserEmailDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordSetTokenLink { get; set; }
    public UserRole Role { get; set; }
}