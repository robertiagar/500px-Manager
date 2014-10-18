using _500pxManager.Common;
using _500pxManager.Api.Entities;
using _500pxManager.Api.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

namespace _500pxManager.ViewModel
{
    public class AddPhotoViewModel : ViewModelBase
    {
        private string _Name;
        private string _Description;
        private Privacy _SelectedPrivacy;
        private Category _SelectedCategory;
        private IPxService pxService;

        public AddPhotoViewModel(IPxService pxService, IStatusBarService statusBarService)
        {
            AddPhotoCommand = new GalaSoft.MvvmLight.Command.RelayCommand(() => SelectPhoto());
            UploadPhotoCommand = new GalaSoft.MvvmLight.Command.RelayCommand(async () => await UploadFileAsync());
            this.pxService = pxService;
            this.statusBarService = statusBarService;

            PrivacyOptions = new ObservableCollection<string>(Enum.GetNames(typeof(Privacy)).Select(p => p.ToWords()));
            Categories = new ObservableCollection<string>(Enum.GetNames(typeof(Category)).Select(p => p.ToWords()));
        }

        public string Name
        {
            get { return _Name; }
            set
            {
                Set<string>(() => Name, ref _Name, value);
            }
        }

        public IList<string> PrivacyOptions { get; private set; }
        public IList<string> Categories { get; private set; }

        public string Description
        {
            get { return _Description; }
            set
            {
                Set<string>(() => Description, ref _Description, value);
            }
        }

        public Privacy SelectedPrivacy
        {
            get { return _SelectedPrivacy; }
            set
            {
                Set<Privacy>(() => SelectedPrivacy, ref _SelectedPrivacy, value);
            }
        }

        public Category SelectedCategory
        {
            get { return _SelectedCategory; }
            set
            {
                Set<Category>(() => SelectedCategory, ref _SelectedCategory, value);
            }
        }

        public ICommand AddPhotoCommand { get; private set; }
        public ICommand UploadPhotoCommand { get; private set; }

        private void SelectPhoto()
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.PickSingleFileAndContinue();
        }


        private IStorageFile _File;
        public IStorageFile File
        {
            get { return _File; }
            set
            {
                Set<IStorageFile>(() => File, ref _File, value);
            }
        }


        public async Task SetFileAsync(IStorageFile file)
        {
            File = file;
            using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                // Set the image source to the selected bitmap 
                BitmapImage bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(fileStream);
                Image = bitmapImage;
            }
        }

        private async Task UploadFileAsync()
        {
            statusBarService.DisplayMessage("Uploading picture");
            await statusBarService.ShowProgressAsync();

            await pxService.UploadPhotoAsync(File, op =>
            {
                var total = op.Progress.TotalBytesToSend;
                var sent = op.Progress.BytesSent;
                var percent = (sent * 100.0 )/total;
                statusBarService.DisplayProgress(percent);
            }
            , Name, Description, SelectedPrivacy, SelectedCategory);

            await statusBarService.HideProgressAsync();
        }

        private ImageSource _Image;
        private IStatusBarService statusBarService;
        public ImageSource Image
        {
            get { return _Image; }
            set
            {
                Set<ImageSource>(() => Image, ref _Image, value);
            }
        }
    }
}
