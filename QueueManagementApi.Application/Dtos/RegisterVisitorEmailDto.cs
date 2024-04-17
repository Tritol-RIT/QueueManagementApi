namespace QueueManagementApi.Application.Dtos;

public class RegisterVisitorEmailDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ExhibitTitle { get; set; }
    public string QrCodeImage { get; set; }
    public string VisitDate { get; set; }
}
