using System;
using System.Windows;

namespace VetWindows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string vetDataPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\VetData");

            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Path.GetFullPath(vetDataPath));
        }
    }
}
