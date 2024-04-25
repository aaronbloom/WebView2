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
        var env = await CoreWebView2Environment.CreateAsync();
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
                  chrome.webview.addEventListener("sharedbufferreceived", async (event) => {
                    console.log("SharedBuffer event received", event);
                    const sharedBuffer = event.getBuffer();
                    console.log("SharedBuffer: ", sharedBuffer);
                    const sharedArray = new Int32Array(sharedBuffer);
                    console.log("SharedArray: ", sharedArray);

                    console.log("Trying different Atomics operations on the SharedArrayBuffer");
                    // Atomics methods
                    // Atomics.add()
                    // Atomics.and()
                    // Atomics.compareExchange()
                    // Atomics.exchange()
                    // Atomics.isLockFree()
                    // Atomics.load()
                    // Atomics.notify()
                    // Atomics.or()
                    // Atomics.store()
                    // Atomics.sub()
                    // Atomics.wait()
                    // Atomics.waitAsync()
                    // Atomics.xor()

                    try {
                        console.log("Trying to Atomics.add(sharedArray, 0, 1)");
                        const result = Atomics.add(sharedArray, 0, 1);
                        console.log("Atomics.add succeeded", result);
                    } catch (e) {
                        console.log("Atomics.add failed", e);
                    }

                    try {
                        console.log("Trying to Atomics.and(sharedArray, 0, 1)");
                        const result = Atomics.and(sharedArray, 0, 1);
                        console.log("Atomics.and succeeded", result);
                    } catch (e) {
                        console.log("Atomics.and failed", e);
                    }

                    try {
                        console.log("Trying to Atomics.compareExchange(sharedArray, 0, 1, 2)");
                        const result = Atomics.compareExchange(sharedArray, 0, 1, 2);
                        console.log("Atomics.compareExchange succeeded", result);
                    } catch (e) {
                        console.log("Atomics.compareExchange failed", e);
                    }

                    try {
                        console.log("Trying to Atomics.exchange(sharedArray, 0, 1)");
                        const result = Atomics.exchange(sharedArray, 0, 1);
                        console.log("Atomics.exchange succeeded", result);
                    } catch (e) {
                        console.log("Atomics.exchange failed", e);
                    }

                    try {
                        console.log("Trying to Atomics.isLockFree(sharedArray)");
                        const result = Atomics.isLockFree(sharedArray);
                        console.log("Atomics.isLockFree succeeded", result);
                    } catch (e) {
                        console.log("Atomics.isLockFree failed", e);
                    }

                    try {
                        console.log("Trying to Atomics.load(sharedArray, 0)");
                        const result = Atomics.load(sharedArray, 0);
                        console.log("Atomics.load succeeded", result);
                    } catch (e) {
                        console.log("Atomics.load failed", e);
                    }

                    try {
                        console.log("Trying to Atomics.notify(sharedArray, 0, 1)");
                        const result = Atomics.notify(sharedArray, 0, 1);
                        if (result === 0) {
                            // See https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Atomics/notify
                            console.log("Atomics.notify returned 0, no waiters were notified");
                        } else {
                            console.log("Atomics.notify succeeded", result);
                        }
                    } catch (e) {
                        console.log("Atomics.notify failed", e);
                    }

                    try {
                        console.log("Trying to Atomics.or(sharedArray, 0, 1)");
                        const result = Atomics.or(sharedArray, 0, 1);
                        console.log("Atomics.or succeeded", result);
                    } catch (e) {
                        console.log("Atomics.or failed", e);
                    }

                    try {
                        console.log("Trying to Atomics.store(sharedArray, 0, 1)");
                        const result = Atomics.store(sharedArray, 0, 1);
                        console.log("Atomics.store succeeded", result);
                    } catch (e) {
                        console.log("Atomics.store failed", e);
                    }

                    try {
                        console.log("Trying to Atomics.sub(sharedArray, 0, 1)");
                        const result = Atomics.sub(sharedArray, 0, 1);
                        console.log("Atomics.sub succeeded", result);
                    } catch (e) {
                        console.log("Atomics.sub failed", e);
                    }

                    try {
                        console.log("Trying to Atomics.wait(sharedArray, 0, 1)");
                        Atomics.wait(sharedArray, 0, 1);
                        console.log("Atomics.wait succeeded");
                    } catch (e) {
                        console.log("Atomics.wait failed", e);
                    }

                    try {
                        console.log("Trying to Atomics.waitAsync(sharedArray, 0, 1)");
                        const result = Atomics.waitAsync(sharedArray, 0, 1);
                        if (result.async) {
                            console.log("Atomics.waitAsync is async");
                            const waitResult = await result.value;
                            console.log("Atomics.waitAsync result", waitResult);
                        } else {
                            console.log("Atomics.waitAsync is not async");
                        }
                        console.log("Atomics.waitAsync succeeded");
                    } catch (e) {
                        console.log("Atomics.waitAsync failed", e);
                    }

                    try {
                        console.log("Trying to Atomics.xor(sharedArray, 0, 1)");
                        const result = Atomics.xor(sharedArray, 0, 1);
                        console.log("Atomics.xor succeeded", result);
                    } catch (e) {
                        console.log("Atomics.xor failed", e);
                    }

                    console.log("Atomics operations on the SharedArrayBuffer completed", sharedArray);
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