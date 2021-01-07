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
using Windows.UI.Xaml;
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

            List<(Color, int)> colors = await Task.Run(async () => await LoadColors(imageUrl));

            RowDefinitionCollection rowDefinitions = RootGrid.RowDefinitions;

            int i = 0;
            foreach (var color in colors)
            {
                GridLength gridLength = new GridLength(color.Item2, GridUnitType.Star);
                rowDefinitions.Add(new RowDefinition() { Height = gridLength });

                Grid bgGrid = new Grid();
                bgGrid.Background = new SolidColorBrush(color.Item1);

                Grid.SetRow(bgGrid, i);

                RootGrid.Children.Add(bgGrid);
                i++;
            }

            Grid.SetRowSpan(Image, i+1);
        }

        private async Task<List<(Color, int)>> LoadColors(string url)
        {
            var image = await ImageParser.GetImage(url);

            if (image is null)
                return null;

            var rgbColors = ImageParser.GetImageColors(image, 64);
            var hsvColors = rgbColors.Select(x => x.AsHsv());
            GaussianKernel kernel = new GaussianKernel(.05);
            List<(MeanShiftCluster<HSVColor, HSVShape>, int)> clusters = MeanShiftMethod.MeanShift<HSVColor, HSVShape>(hsvColors, kernel);
            
            List<(Color, int)> weightedColors = clusters.Select(x =>
            {
                var rgbColor = x.Item1.GetNearestToCenter().AsRgb();
                Color color = Color.FromArgb(255, rgbColor.R, rgbColor.G, rgbColor.B);
                return (color, x.Item2);
            }).ToList();

            return weightedColors;
        }
    }
}
