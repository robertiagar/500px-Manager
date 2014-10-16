using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using _500pxManager.Api;
using System.Diagnostics;
using Windows.ApplicationModel.Activation;
using Windows.Security.Authentication.Web;
using _500pxManager.Api.Services;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace _500pxManager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IWebAuthenticationContinuable, IFileOpenPickerContinuable
    {
        private PxService pxService;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            
            pxService = new PxService(new SettingsService());
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            await pxService.LoginAsync();
        }

        public async void ContinueWebAuthentication(WebAuthenticationBrokerContinuationEventArgs args)
        {
            WebAuthenticationResult result = args.WebAuthenticationResult;

            await pxService.GetAccessTokenAsync(result);

            await pxService.GetUserAsync();


            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                Debug.WriteLine(result.ResponseData.ToString());
            }
            else if (result.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
            {
                Debug.WriteLine("HTTP Error returned by AuthenticateAsync() : " + result.ResponseErrorDetail.ToString());
            }
            else
            {
                Debug.WriteLine("Error returned by AuthenticateAsync() : " + result.ResponseStatus.ToString());
            }
        }

        public async void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args)
        {
            var result = args.Files;

            await pxService.UploadPhotoContinueAsync(result);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pxService.UploadPhoto();
        }
    }
}
