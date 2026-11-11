using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace Tool
{
    public class QRCodeUtils
    {
        public static Bitmap CreateQRCode(string sourceStr)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 4;
            qrCodeEncoder.QRCodeVersion = 6;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

            Bitmap qrImage = qrCodeEncoder.Encode(sourceStr);

            return qrImage;

        }

        public static Bitmap CreateQRCode(string sourceStr, Bitmap centerLogo)
        {
            Bitmap qrImage = CreateQRCode(sourceStr);

            Graphics g = Graphics.FromImage(qrImage);
            g.DrawImage(centerLogo, new Point(qrImage.Width / 2 - 20, qrImage.Height / 2 - 20));

            return qrImage;
        }

        public static byte[] CreateQRCodeToByte(string sourceStr)
        {
            byte[] retImg = new byte[] { };

            var img = CreateQRCode(sourceStr);

            MemoryStream ms = null;
            ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            retImg = ms.ToArray();

            return retImg;
        }
        /// <summary>
        /// 二维码解码
        /// </summary>
        /// <param name="filePath">图片路径</param>
        /// <returns></returns>
        public static string QRCodeDecoder(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                return null;
            Bitmap myBitmap = new Bitmap(Image.FromFile(filePath));
            QRCodeDecoder decoder = new QRCodeDecoder();
            string decodedString = decoder.decode(new QRCodeBitmapImage(myBitmap));
            return decodedString;
        }

    }
}
