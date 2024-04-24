using System.Windows;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace WebView2Atomics;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        WebView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
        LoadMain();
    }

    private void WebView_CoreWebView2InitializationCompleted(object? sender,
        CoreWebView2InitializationCompletedEventArgs e)
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

    private void CoreWebView2_ProcessFailed(object? sender, CoreWebView2ProcessFailedEventArgs e)
    {
        MessageBox.Show($"WebView2 ProcessFailed {e.ProcessFailedKind}");
    }

    private async void LoadMain()
    {
        var env = await CoreWebView2Environment.CreateAsync(@"C:\tools\Microsoft.WebView2.FixedVersionRuntime.124.0.2478.51.x64");
        await WebView.EnsureCoreWebView2Async(env);
        // Open dev tools
        WebView.CoreWebView2.OpenDevToolsWindow();
        // Create a simple HTML page that uses JS Atomics on a SharedArrayBuffer
        await WebView.CoreWebView2.ExecuteScriptAsync(
            $$"""
                document.write(`
                <html>

                <head>
                  <title>WebView2Atomics</title>
                </head>

                <body>
                  <h1>WebView2Atomics</h1>
                  <p>WebView2Atomics is a simple WebView2 app that uses JS Atomics on a SharedArrayBuffer from C#.</p>
                  <p>BrowserVersion: {{env.BrowserVersionString}}</p>
                  <p>.NET Version: {{Environment.Version}}</p>
                  <p>OS Version: {{Environment.OSVersion}}</p>
                  <script>
                    chrome.webview.addEventListener("sharedbufferreceived", (event) => {
                      console.log("SharedBuffer event received", event);
                      const sharedBuffer = event.getBuffer();
                      console.log("SharedBuffer: ", sharedBuffer);
                      const sharedArray = new Int32Array(sharedBuffer);
                      // Try to lock the buffer using Atomics.wait
                      console.log("Trying to lock buffer using Atomics.wait");
                      Atomics.wait(sharedArray, 0, 0);
                      // If we get here, the buffer was locked
                      console.log("Buffer locked");
                    });
                  </script>
                </body>

                </html>
                `);
                """);

        // Create a SharedArrayBuffer and send it to the WebView
        var sharedBuffer = env.CreateSharedBuffer(256);
        WebView.CoreWebView2.PostSharedBufferToScript(sharedBuffer, CoreWebView2SharedBufferAccess.ReadWrite, "{}");
    }
}