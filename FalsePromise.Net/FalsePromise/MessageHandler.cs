using Jil;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FalsePromise
{
    public class MessageHandler
    {
        private WebView2 _view;

        public MessageHandler(WebView2 view)
        {
            view.CoreWebView2Ready += CoreReady;
            view.WebMessageReceived += WebMessageReceived;
            _view = view;
        }

        public virtual async Task<string> Requested(string message)
        {
            await _view.ExecuteScriptAsync("console.error('[FalsePromise] Override the Requested method to handle requests.')");
            return null;
        }

        private void CoreReady(WebView2 view, WebView2CoreWebView2ReadyEventArgs args)
        {
            view.CoreWebView2.Settings.IsWebMessageEnabled = true;
        }

        private async void WebMessageReceived(WebView2 view, WebView2WebMessageReceivedEventArgs args)
        {
            FalsePromiseRequest req;
            try
            {
                req = JSON.Deserialize<FalsePromiseRequest>(args.WebMessageAsString);
            }
            catch
            {
                await view.ExecuteScriptAsync("console.error('[FalsePromise] Failed to deserialize request.')");
                throw;
            }

            string result;
            try
            {
                result = await Requested(req.Message);
            }
            catch
            {
                await view.ExecuteScriptAsync("console.error('[FalsePromise] Exception occurred in Requested method.')");
                throw;
            }

            var response = result == null ? null : $"\"{result}\"";
            var jsCommand = $"window.falsePromise.callback(\"{req.Id}\", null, {response})";
            try
            {
                await view.ExecuteScriptAsync(jsCommand);
            }
            catch
            {
                await view.ExecuteScriptAsync("console.error('[FalsePromise] Error firing callback method.')");
                throw;
            }
        }
    }
    public class FalsePromiseRequest
    {
        public string Id { get; set; }
        public string Message { get; set; }
    }
}
