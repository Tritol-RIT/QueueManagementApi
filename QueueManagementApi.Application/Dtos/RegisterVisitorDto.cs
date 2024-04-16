namespace QueueManagementApi.Application.Dtos;

public class RegisterVisitorDto
{
    public int ExhibitId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int NumberOfPeople { get; set; }
}