using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;
using QueueManagementApi.Core.Enums;

namespace QueueManagementApi.Application.Dtos.UserDtos;

public sealed class UserDto
{
    public int Id { get; set; }

    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public UserRole Role { get; set; }
    public string Email { get; set; }
    public bool Active { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; } 
    public int? ExhibitId { get; set; }
    public string? ExhibitName { get; set; }
}