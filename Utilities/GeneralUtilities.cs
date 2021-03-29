using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace CRM.Utilities
{
    public class GeneralUtilities
    {
        public static string ConvertToBase64(HttpPostedFile file)
        {
            System.Drawing.Bitmap bmpPostedImage = new System.Drawing.Bitmap(file.InputStream);
            System.IO.MemoryStream ms = new MemoryStream();
            Bitmap bmpImage = new Bitmap(bmpPostedImage);
            bmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] data = ms.GetBuffer();
            string document = Convert.ToBase64String(RemoveByteTrailingZeros(data));
            return document;
        }

        public static byte[] RemoveByteTrailingZeros(byte[] bytePacket)
        {
            var i = bytePacket.Length - 1;
            while (bytePacket[i] == 0)
            {
                --i;
            }
            var finalByte = new byte[i + 1];
            Array.Copy(bytePacket, finalByte, i + 1);
            return finalByte;
        }

        public static string FixInvalidChars(string text)
        {
            string result = text.Replace("'", "''");
            return result;
        }
    }
}