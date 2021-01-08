using ColorExtractor.ColorSpaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ColorExtractor
{
    public static class ImageParser
    {
        public static async Task<Image<Argb32>> GetImage(string uri)
        {
            return (await Image.LoadAsync(await GetImageStreamAsync(uri))).CloneAs<Argb32>();
        }
        private static async Task<Stream> GetImageStreamAsync(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return null;
            }

            var response = await HttpWebRequest.CreateHttp(uri).GetResponseAsync();
            return response.GetResponseStream();
        }

        public static List<RGBColor> GetImageColors(
            Image<Argb32> image,
            int quality = 1920)
        {
            List<RGBColor> colors = new List<RGBColor>();

            int nth = (image.Width * image.Height) / quality;

            for (int rows = 0; rows < image.Height; rows++)
            {
                Span<Argb32> rowPixels = image.GetPixelRowSpan(rows);
                for (int i = 0; i < rowPixels.Length; i += nth)
                {
                    byte b = rowPixels[i].B;
                    byte g = rowPixels[i].G;
                    byte r = rowPixels[i].R;
                    byte a = rowPixels[i].A;

                    RGBColor color = new RGBColor(r, g, b);

                    colors.Add(color);
                }
            }

            return colors;
        }
    }
}
