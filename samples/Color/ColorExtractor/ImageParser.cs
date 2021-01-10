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

        public static RGBColor[] GetImageColors(
            Image<Argb32> image,
            int quality = 1920)
        {
            int nth = image.Width * image.Height / quality;

            int pixelsPerRow = image.Width / nth;

            RGBColor[] colors = new RGBColor[image.Height * pixelsPerRow];


            int pos = 0;
            for (int row = 0; row < image.Height; row++)
            {
                Span<Argb32> rowPixels = image.GetPixelRowSpan(row);
                for (int i = 0; i < pixelsPerRow; i++)
                {
                    float b = rowPixels[i * nth].B / 255f;
                    float g = rowPixels[i * nth].G / 255f;
                    float r = rowPixels[i * nth].R / 255f;
                    //float a = rowPixels[i].A / 255;

                    RGBColor color = new RGBColor(r, g, b);

                    colors[pos] = color;
                    pos++;
                }
            }

            return colors;
        }
    }
}
