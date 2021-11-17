using System.Drawing;
using QRCoder;

namespace QrPayment.Extensions
{
    public static class PaymentExtensions
    {
        public static byte[] AsPng(
            this Model.Payment payment, 
            int pixelsPerModule = 16,
            Color? foregroundColor = null, 
            Color? backgroundColor = null)
        {
            foregroundColor ??= Color.Black;
            backgroundColor ??= Color.White;

            byte[] foregroundColorBytes =
            {
                foregroundColor.Value.R, 
                foregroundColor.Value.G, 
                foregroundColor.Value.B, 
                foregroundColor.Value.A
            };
            byte[] backgroundColorBytes =
            {
                backgroundColor.Value.R,
                backgroundColor.Value.G,
                backgroundColor.Value.B,
                backgroundColor.Value.A
            };

            var generator = new QRCodeGenerator();
            var data = generator.CreateQrCode(payment.CreatePaymentString(), QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(data);
            return qrCode.GetGraphic(pixelsPerModule, foregroundColorBytes, backgroundColorBytes);
        }
    }
}