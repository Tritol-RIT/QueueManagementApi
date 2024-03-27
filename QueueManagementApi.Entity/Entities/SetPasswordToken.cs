using System.ComponentModel.DataAnnotations.Schema;

namespace QueueManagementApi.Core.Entities;

public class SetPasswordToken : BaseEntity, IAuditable
{
    public int UserId { get; set; }

    public string Token { get; set; }

    public DateTime ExpirationDate { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool Active { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}