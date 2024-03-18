namespace QueueManagementApi.Application.Services.QrCodeService;

public interface IQrCodeService
{
    public string GenerateQrCode(Guid visitID);
}
