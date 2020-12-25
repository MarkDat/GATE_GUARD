using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GATE_GUARD2.Common
{
    public static class GetImgUrl
    {
        public static void SaveImage(PictureBox avatar, string url)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(url);
            Bitmap bitmap;
            bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                avatar.Image = bitmap;
            }

            stream.Flush();
            stream.Close();
            client.Dispose();
        }
        public static Stream ToStream(this Image image, ImageFormat format)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }
        public static Image DownloadImage(string fromUrl)
        {
            using (System.Net.WebClient webClient = new System.Net.WebClient())
            {
                using (Stream stream = webClient.OpenRead(fromUrl))
                {
                    Console.WriteLine("LINK NÈ: "+fromUrl);
                    try
                    {
                        return Image.FromStream(stream);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                   
                }
            }
        }
    }
}
