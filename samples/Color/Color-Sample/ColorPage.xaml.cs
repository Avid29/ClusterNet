using ClusterNet;
using ClusterNet.Kernels;
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
        private string _imageUrl;
        private double _bandwidth;
        private int _quality;
        
        public ColorPage(string url, double bandwidth, int quality)
        {
            this.InitializeComponent();
            _imageUrl = url;
            _bandwidth = bandwidth;
            _quality = quality;

            this.Loaded += ColorPage_Loaded;
        }

        private async void ColorPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= ColorPage_Loaded;
            
            Image.Source = new BitmapImage(new Uri(_imageUrl));

            List<(Color, int)> colors = await Task.Run(async () => await LoadColors());

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

        private async Task<List<(Color, int)>> LoadColors()
        {
            var image = await ImageParser.GetImage(_imageUrl);

            if (image is null)
                return null;

            RGBColor[] rgbColors = ImageParser.SampleImage(ImageParser.GetImageColors(image), _quality, _quality);
            GaussianKernel kernel = new GaussianKernel(_bandwidth);
            (RGBColor, int)[] clusters = ClusterAlgorithms.MeanShiftMultiThreaded<RGBColor, RGBShape, GaussianKernel>(rgbColors, kernel);

            List<(Color, int)> weightedColors = clusters.Select(x =>
            {
                var rgbColor = x.Item1;
                Color color = Color.FromArgb(255,
                    (byte)(rgbColor.R * 255),
                    (byte)(rgbColor.G * 255),
                    (byte)(rgbColor.B * 255));
                return (color, x.Item2);
            }).ToList();

            return weightedColors;
        }
    }
}
