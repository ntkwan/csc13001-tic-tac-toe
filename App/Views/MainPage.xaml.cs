using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using CaroBotAlgorithm;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

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

        private void Button_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var button = sender as Button;
            button.Foreground = new SolidColorBrush(Colors.White);
        }

        private void EasyModeButton_Click(object sender, RoutedEventArgs e)
        {
            var plugin = LoadPlugin("Easy");
            if (plugin != null)
            {
                // Chuyển đến trang chơi và truyền plugin
                this.Frame.Navigate(typeof(CaroPage), plugin);
            }
            else
            {
                ShowError("Không load được plugin Easy.");
            }
        }

        private void MediumModeButton_Click(object sender, RoutedEventArgs e)
        {
            var plugin = LoadPlugin("Medium");
            if (plugin != null)
            {
                this.Frame.Navigate(typeof(CaroPage), plugin);
            }
            else
            {
                ShowError("Không load được plugin Medium.");
            }
        }

        private void HardModeButton_Click(object sender, RoutedEventArgs e)
        {
            var plugin = LoadPlugin("Hard");
            if (plugin != null)
            {
                this.Frame.Navigate(typeof(CaroPage), plugin);
            }
            else
            {
                ShowError("Không load được plugin Hard.");
            }
        }

        // Hàm lấy plugin theo tên lớp (ví dụ: "EasyPlayer")
        private IAlgorithm LoadPlugin(string pluginName)
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            Debug.WriteLine("Base directory: " + folder);
            var fis = (new DirectoryInfo(folder)).GetFiles("*.dll");
            IAlgorithm algorithm = null;
            foreach (var fi in fis)
            {
                if (fi.Name.StartsWith(pluginName, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var assembly = Assembly.LoadFrom(fi.FullName);
                        var types = assembly.GetTypes();
                        foreach (var type in types)
                        {
                            if (typeof(IAlgorithm).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                            {
                                Debug.WriteLine("Found type: " + type.FullName);
                                algorithm = Activator.CreateInstance(type) as IAlgorithm;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Lỗi khi load assembly " + fi.FullName + ": " + ex.Message);
                    }
                }
            }
            return algorithm;
        }

        private async void ShowError(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Lỗi",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}
