using _500pxManager.Api.Interfaces;
using _500pxManager.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.Networking.Connectivity;
using System;

namespace _500pxManager.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private int _SelectedIndex;
        private string _AddLabel;
        private INavigationService navigationService;
        private IList<PhotoViewModel> photos;
        private IPxService pxService;
        private IStatusBarService statusBarService;

        public MainViewModel(INavigationService navigationService, IPxService pxService, IStatusBarService statusBarService)
        {
            SelectedIndex = 0;
            AddLabel = "add photo";
            this.navigationService = navigationService;
            this.AddPhotoCommand = new RelayCommand(() => NavigateToAddPhoto());
            this.RefreshCommand = new RelayCommand(async () => await RefreshAsync());
            this.photos = new ObservableCollection<PhotoViewModel>();
            this.pxService = pxService;
            this.statusBarService = statusBarService;
        }

        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                Set<int>(() => SelectedIndex, ref _SelectedIndex, value);
                switch (value)
                {
                    case 0:
                    //AddLabel = "add collection";
                    //Flyout = App.Current.Resources["AddCollectionFlyout"] as Flyout;
                    //break;
                    case 1:
                        AddLabel = "add photo";
                        break;
                }
            }
        }

        public IList<PhotoViewModel> Photos
        {
            get { return photos; }
        }

        public ICommand AddPhotoCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public string AddLabel
        {
            get { return _AddLabel; }
            set
            {
                Set<string>(() => AddLabel, ref _AddLabel, value);
            }
        }

        private void NavigateToAddPhoto()
        {
            navigationService.Navigate(typeof(AddPhotoPage));
        }

        public async Task GetPhotosAsync()
        {
            var hasInternet = await HasInternetAsync();
            if (hasInternet)
            {
                if (Photos.Count != 0)
                {
                    await RefreshAsync();
                }
                else
                {
                    statusBarService.DisplayMessage("Getting photos...");
                    await statusBarService.ShowProgressAsync();
                    var photos = await pxService.GetFirstPhotosPageForUserAsync();
                    var list = photos.Select(p => new PhotoViewModel(p));
                    foreach (var photo in list)
                    {
                        Photos.Add(photo);
                    }

                    photos = await pxService.GetOtherPhotosPagesForUserAsync();
                    list = photos.Select(p => new PhotoViewModel(p));
                    foreach (var photo in list)
                    {
                        Photos.Add(photo);
                    }
                }
                await statusBarService.DisplayMessage("Done!", 3000, false);
                await statusBarService.HideProgressAsync();
            }
        }

        public async Task RefreshAsync()
        {
            var hasInternet = await HasInternetAsync();
            if (hasInternet)
            {
                statusBarService.DisplayMessage("Refreshing...");
                await statusBarService.ShowProgressAsync();
                var photos = await pxService.GetFirstPhotosPageForUserAsync();
                foreach (var photo in photos)
                {
                    var exists = Photos.Any(p => p.Photo.id == photo.id);
                    if (!exists)
                    {
                        var photoViewModel = new PhotoViewModel(photo);
                        var list = Photos.ToList();
                        int index = list.BinarySearch(photoViewModel);
                        int insertIndex = ~index;
                        Photos.Insert(insertIndex, photoViewModel);
                    }
                }

                photos = await pxService.GetOtherPhotosPagesForUserAsync();
                foreach (var photo in photos)
                {
                    var exists = Photos.Any(p => p.Photo.id == photo.id);
                    if (!exists)
                    {
                        var photoViewModel = new PhotoViewModel(photo);
                        var list = Photos.ToList();
                        int index = list.BinarySearch(photoViewModel);
                        int insertIndex = ~index;
                        Photos.Insert(insertIndex, photoViewModel);
                    }
                }
                await statusBarService.DisplayMessage("Done!", 3000, false);
                await statusBarService.HideProgressAsync();
            }
        }

        public async Task<bool> HasInternetAsync()
        {
            string connectionProfileInfo = string.Empty;
            try
            {
                ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

                if (InternetConnectionProfile == null)
                {
                    statusBarService.DisplayMessage("Offline", false);
                    await statusBarService.ShowProgressAsync();
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}