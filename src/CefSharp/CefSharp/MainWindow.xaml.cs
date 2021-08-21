using CefSharp;
using CefSharp.Wpf;
using CefSharp.Handlers;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CefSharp
{
    public partial class MainWindow : Window
    {
        public ChromiumWebBrowser browser;
        private Region region;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;

            //string starturl = "http://localhost:4200";
            string starturl = "http://www.naver.com";

            CefSettings settings = new CefSettings();
            settings.CefCommandLineArgs.Add("disable-usb-keyboard-detect", "1");
            Cef.Initialize(settings);

            browser = new ChromiumWebBrowser();

            browser.AllowDrop = true;
            browser.Address = starturl;
            Grid.SetRow(browser, 0);
            grd1.Children.Add(browser);


            var downloadHandler = new DownloadHandler();
            browser.DownloadHandler = downloadHandler;

            browser.DisplayHandler = new DisplayHandler();

            var dragHandler = new DragHandler();
            dragHandler.RegionsChanged += OnDragHandlerRegionsChanged;
            browser.DragHandler = dragHandler;

            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void OnDragHandlerRegionsChanged(Region region)
        {
            if (region != null)
            {
                //Only wire up event handler once
                if (this.region == null)
                {
                    browser.PreviewMouseLeftButtonDown += OnBrowserMouseLeftButtonDown;
                }

                this.region = region;
            }
        }

        private void OnBrowserMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(browser);

            if (region.IsVisible((float)point.X, (float)point.Y))
            {
                var window = Window.GetWindow(this);
                window.DragMove();

                e.Handled = true;
            }
        }






    }
}
