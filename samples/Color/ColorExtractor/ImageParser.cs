using ColorExtractor.ColorSpaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;
using System.Net;
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

        public static RGBColor[,] GetImageColors(
            Image<Argb32> image)
        {
            RGBColor[,] colors = new RGBColor[image.Height, image.Width];

            for (int row = 0; row < image.Height; row++)
            {
                Span<Argb32> rowPixels = image.GetPixelRowSpan(row);
                for (int col = 0; col < image.Width; col++)
                {
                    float b = rowPixels[col].B / 255f;
                    float g = rowPixels[col].G / 255f;
                    float r = rowPixels[col].R / 255f;
                    //float a = rowPixels[col].A / 255;

                    RGBColor color = new RGBColor(r, g, b);

                    colors[row, col] = color;
                }
            }

            return colors;
        }

        public static RGBColor[] SampleImage(RGBColor[,] colors, int rowSamples, int colSamples)
        {
            int rows = colors.GetLength(0);
            int cols = colors.GetLength(1);
            int nthRow = rows / rowSamples;
            int nthCol = cols / colSamples;
            
            RGBColor[] output = new RGBColor[rowSamples * colSamples];

            int pos = 0;
            for (int row = 0; row < rows; row += nthRow)
            {
                for (int col = 0; col < cols; col += nthCol)
                {
                    if (pos == output.Length)
                    {
                        return output;
                    }

                    output[pos] = colors[row, col];
                    pos++;
                }
            }

            return output;
        }
    }
}
