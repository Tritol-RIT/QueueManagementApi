namespace QueueManagementApi.Application.Dtos;

public class ResetPasswordEmailDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordResetTokenLink { get; set; }
}

