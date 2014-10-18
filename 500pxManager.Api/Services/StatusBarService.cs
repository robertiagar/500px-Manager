using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace _500pxManager.Api.Services
{
    public class StatusBarService
    {
        private StatusBar statusBar;
        public StatusBarService()
        {
            statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
        }

        public StatusBarService(Color color)
        {
            statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            statusBar.BackgroundColor = color;
            statusBar.BackgroundOpacity = 1;
        }

        public void DisplayProgress(double? progress, string message = null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                statusBar.ProgressIndicator.Text = message;
            }
            statusBar.ProgressIndicator.ProgressValue = progress;
        }

        public void DisplayMessage(string message)
        {
            statusBar.ProgressIndicator.Text = message;
        }

        public async Task ShowProgressAsync()
        {
            await statusBar.ProgressIndicator.ShowAsync();
        }

        public async Task HideProgressAsync()
        {
            await statusBar.ProgressIndicator.HideAsync();
        }
    }
}
