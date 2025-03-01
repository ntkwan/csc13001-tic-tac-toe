using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI_Learn.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI_Learn
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();

            // Create a Frame to act as the navigation context.
            Frame rootFrame = new Frame();

            // Place the frame in the current Window
            m_window.Content = rootFrame;  // <---- THIS IS CRUCIAL

            // Navigate to the first page
            rootFrame.Navigate(typeof(MainPage), args.Arguments);

            m_window.Activate();
        }

        private Window? m_window;
    }
}
