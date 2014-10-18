using _500pxManager.Api.Interfaces;
using _500pxManager.Interfaces;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Security.Authentication.Web;

namespace _500pxManager.ViewModel
{
    public class LoginViewModel
    {
        private IPxService _pxService;
        private INavigationService _navigationService;

        public LoginViewModel(IPxService pxService, INavigationService navigationService)
        {
            _pxService = pxService;
            _navigationService = navigationService;
            LoginCommand = new RelayCommand(async () => await this.LoginAsync());
        }

        private async Task LoginAsync()
        {
            var loggingIn = await _pxService.LoginAsync();
            if (!loggingIn)
            {
                _navigationService.Navigate(typeof(MainPage));
            }
        }

        public ICommand LoginCommand { get; private set; }



        public async Task ContinueLoginAsync(WebAuthenticationResult result)
        {
            await _pxService.SaveAccessTokenAsync(result);

            _navigationService.Navigate(typeof(MainPage));
        }

        public async Task HasAccessToken()
        {
            var hasAccessToken = await _pxService.HaveAccessTokenAsync();
            if (hasAccessToken)
            {
                _navigationService.Navigate(typeof(MainPage));
            }
        }
    }
}
