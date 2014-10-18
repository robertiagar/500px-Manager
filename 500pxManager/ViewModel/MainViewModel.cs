using _500pxManager.Api.Interfaces;
using _500pxManager.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace _500pxManager.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private int _SelectedIndex;
        private string _AddLabel;
        private INavigationService navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            SelectedIndex = 0;
            AddLabel = "add photo";
            this.navigationService = navigationService;
            this.AddPhotoCommand = new RelayCommand(() => NavigateToAddPhoto());
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

        public ICommand AddPhotoCommand { get; private set; }

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
    }
}