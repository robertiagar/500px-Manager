using _500pxManager.Api.Interfaces;
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
    public class StatusBarService : IStatusBarService
    {
        private StatusBar statusBar;

        public StatusBarService()
        {
            statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            statusBar.BackgroundOpacity = 1;
        }

        //public StatusBarService(Color color)
        //{
        //    statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
        //    statusBar.BackgroundColor = color;
        //}

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
            statusBar.ProgressIndicator.ProgressValue = null;
        }


        public async Task DisplayMessage(string message, int delay)
        {
            statusBar.ProgressIndicator.Text = message;
            await Task.Delay(delay);
        }


        public async Task DisplayMessage(string messsage, int delay, bool displayProgressBar = true)
        {
            if (!displayProgressBar)
            {
                statusBar.ProgressIndicator.ProgressValue = 0;
            }
            statusBar.ProgressIndicator.Text = messsage;
            await Task.Delay(delay);
        }
    }
}
