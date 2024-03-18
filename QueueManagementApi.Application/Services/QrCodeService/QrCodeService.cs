using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace QueueManagementApi.Application.Services.QrCodeService;

public class QrCodeService : IQrCodeService
{
    public string GenerateQrCode(Guid visitId)
    {
        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        {
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(visitId.ToString(), QRCodeGenerator.ECCLevel.Q);
            using (QRCode qrCode = new QRCode(qrCodeData))
            {
                using (Bitmap bitmap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bitmap.Save(stream, ImageFormat.Png);
                        byte[] imageBytes = stream.ToArray();
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
        }
    }
}
