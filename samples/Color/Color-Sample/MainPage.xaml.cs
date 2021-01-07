using Windows.UI.Xaml.Controls;

namespace Color_Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void AddBlade(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ColorsBladeView.Items.Add(new ColorPage(UriTextBox.Text));
        }
    }
}
