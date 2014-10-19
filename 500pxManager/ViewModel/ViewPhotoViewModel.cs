using _500pxManager.Api.Entities;
using _500pxManager.Api.Extensions;
using _500pxManager.Api.Interfaces;
using _500pxManager.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace _500pxManager.ViewModel
{
    public class ViewPhotoViewModel : ViewModelBase
    {
        private Category _SelectedCategory;
        private Privacy _SelectedPrivacy;
        private string _Tags;
        private string _Name;
        private string _Description;
        private Photo _Photo;
        private IPxService pxService;
        private INavigationService navigationService;

        public ViewPhotoViewModel(IPxService pxService, INavigationService navigationService)
        {
            Categories = new ObservableCollection<string>(Enum.GetNames(typeof(Category)).Select(p => p.ToWords()));
            Privacies = new ObservableCollection<string>(Enum.GetNames(typeof(Privacy)).Select(p => p.ToWords()));
            UpdatePhotoCommand = new RelayCommand(async () => await UpdatePhotoAsync());
            DeletePhotoCommand = new RelayCommand(async () => await DeletePhotoAsync());
            this.pxService = pxService;
            this.navigationService = navigationService;
        }

        private async Task DeletePhotoAsync()
        {
            var dialog = new MessageDialog("Are you sure you want to delete this photo?", "Confirm");
            var delete = false;
            dialog.Commands.Add(new UICommand("Yes", p =>
            {
                delete = true;
            }));
            dialog.Commands.Add(new UICommand("No",p=>{
                delete = false;
            }));
            await dialog.ShowAsync();

            if (delete)
            {
                var deleted = await pxService.DeletePhotoAsync(Photo.id);
                if (deleted)
                {
                    dialog = new MessageDialog("Photo has been succesfully deleted!", "Deleted");
                    dialog.Commands.Add(new UICommand("OK", p =>
                    {
                        MessengerInstance.Send<int>(Photo.id);
                        navigationService.GoBack();
                    }));
                    await dialog.ShowAsync();
                }
                else
                {
                    dialog = new MessageDialog("There was a problem deleting the photo!", "Error");
                    dialog.Commands.Add(new UICommand("OK", p =>
                    {
                    }));
                    await dialog.ShowAsync();
                }
            }
        }

        private async Task UpdatePhotoAsync()
        {
            var updated = await pxService.UpdatePhotoAsync(Photo.id, Name, Description, Tags, SelectedCategory, SelectedPrivacy);
            if (updated)
            {
                var dialog = new MessageDialog("Photo has been successfully updated!", "Updated");
                dialog.Commands.Add(new UICommand("OK", p =>
                {
                    navigationService.GoBack();
                })
                );
                await dialog.ShowAsync();
            }
            else
            {
                var dialog = new MessageDialog("There was a problem updating the photo!", "Error");
                dialog.Commands.Add(new UICommand("OK",p=>{

                }));
                await dialog.ShowAsync();
            }
        }

        public ICommand UpdatePhotoCommand { get; private set; }
        public ICommand DeletePhotoCommand { get; private set; }

        public IList<string> Categories { get; private set; }
        public IList<string> Privacies { get; private set; }

        public Category SelectedCategory
        {
            get { return _SelectedCategory; }
            set
            {
                Set<Category>(() => SelectedCategory, ref _SelectedCategory, value);
            }
        }

        public string Tags
        {
            get { return _Tags; }
            set
            {
                Set<string>(() => Tags, ref _Tags, value);
            }
        }

        public string Name
        {
            get { return _Name; }
            set
            {
                Set<string>(() => Name, ref _Name, value);
            }
        }

        public string Description
        {
            get { return _Description; }
            set
            {
                Set<string>(() => Description, ref _Description, value);
            }
        }

        public void SetPhoto(Photo photo)
        {
            Photo = photo;
            Name = photo.name;
            Description = photo.description;
            Tags = string.Join(",", photo.tags);
            SelectedCategory = (Category)photo.category;
            SelectedPrivacy = photo.privacy ? Privacy.Private : Privacy.Public;
        }

        public Photo Photo
        {
            get { return _Photo; }
            set
            {
                Set<Photo>(() => Photo, ref _Photo, value);
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
    }
}
