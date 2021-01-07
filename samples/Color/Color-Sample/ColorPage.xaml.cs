using System;
using Windows.UI.Xaml.Controls;
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

        private void ColorPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Loaded -= ColorPage_Loaded;
            
            Image.Source = new BitmapImage(new Uri(imageUrl));
        }
    }
}
