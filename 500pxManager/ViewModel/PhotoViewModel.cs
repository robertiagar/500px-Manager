using _500pxManager.Api.Entities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace _500pxManager.ViewModel
{
    public class PhotoViewModel : ObservableObject, IComparable
    {
        private Photo _Photo;
        public PhotoViewModel(Photo photo)
        {
            Photo = photo;
        }


        public Photo Photo
        {
            get { return _Photo; }
            set
            {
                Set<Photo>(() => Photo, ref _Photo, value);
            }
        }

        public ImageSource Image
        {
            get { return new BitmapImage(new Uri(_Photo.image_url)); }
        }

        public int CompareTo(object obj)
        {
            var photo = obj as PhotoViewModel;
            if (photo != null)
            {
                var objDateTime = DateTime.Parse(photo._Photo.created_at);
                var thisDateTime = DateTime.Parse(this._Photo.created_at);
                if (thisDateTime > objDateTime)
                    return -1;
                else if (thisDateTime < objDateTime)
                    return 1;
                else
                    return 0;
            }

            throw new ArgumentException("Invalid object", "obj");
        }
    }
}
