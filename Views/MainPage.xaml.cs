using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI_Learn.Views
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

        private void Ex1_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Ex1Page));
        }

        private void Ex2_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Ex2Page));
        }

        private void Ex3_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Ex3Page));
        }

        private void Ex4_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Ex4Page));
        }
    }
}
