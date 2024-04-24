using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Windows;

namespace WebView2App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
            LoadMain();
        }

        private void WebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                MessageBox.Show("CoreWebView2InitializationFailed");
                return;
            }
            if (!(sender is WebView2 webViewSender))
            {
                MessageBox.Show("CoreWebView2InitializationFailed, sender is not a WebView2");
                return;
            }
            webViewSender.CoreWebView2.ProcessFailed += CoreWebView2_ProcessFailed;
        }

        private void CoreWebView2_ProcessFailed(object sender, CoreWebView2ProcessFailedEventArgs e)
        {
            MessageBox.Show($"WebView2 ProcessFailed {e.ProcessFailedKind}");
        }

        private async void LoadMain()
        {
            var env = await CoreWebView2Environment.CreateAsync(null, "C:\\temp");
            await webView.EnsureCoreWebView2Async(env);
            webView.Source = new Uri("http://example.com");
        }
    }
}
