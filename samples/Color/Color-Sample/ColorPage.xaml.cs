using ClusterLib;
using ClusterLib.Kernels;
using ColorExtractor;
using ColorExtractor.ColorSpaces;
using ColorExtractor.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Color_Sample
{
    public sealed partial class ColorPage : UserControl
    {
        private string imageUrl;
        
        public ColorPage(string url)
        {
            this.InitializeComponent();
            imageUrl = url;

            this.Loaded += ColorPage_Loaded;
        }

        private async void ColorPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Loaded -= ColorPage_Loaded;
            
            Image.Source = new BitmapImage(new Uri(imageUrl));

            RGBColor? rgbColor = await Task.Run(async () => await UpdateBackgroundColor(imageUrl));

            if (!rgbColor.HasValue)
                return;

            Color color = Color.FromArgb(255, rgbColor.Value.R, rgbColor.Value.G, rgbColor.Value.B);

            RootGrid.Background = new SolidColorBrush(color);
        }

        private async Task<RGBColor?> UpdateBackgroundColor(string url)
        {
            var image = await ImageParser.GetImage(url);

            if (image is null)
                return null;

            var rgbColors = ImageParser.GetImageColors(image, 256);
            var hsvColors = rgbColors.Select(x => x.AsHsv());
            GaussianKernel kernel = new GaussianKernel(5);
            List<(MeanShiftCluster<HSVColor, HSVShape>, int)> clusters = MeanShiftMethod.MeanShift<HSVColor, HSVShape>(hsvColors, kernel);
            return clusters[0].Item1.Centroid.AsRgb();
        }
    }
}
