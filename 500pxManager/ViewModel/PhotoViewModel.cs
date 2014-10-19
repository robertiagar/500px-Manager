using _500pxManager.Api.Entities;
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
    public class PhotoViewModel : IComparable
    {
        private Photo photo;
        public PhotoViewModel(Photo photo)
        {
            this.photo = photo;
        }

        public Photo Photo { get { return photo; } }

        public ImageSource Image
        {
            get { return new BitmapImage(new Uri(photo.image_url)); }
        }

        public int CompareTo(object obj)
        {
            var photo = obj as PhotoViewModel;
            if (photo != null)
            {
                var objDateTime = DateTime.Parse(photo.photo.created_at);
                var thisDateTime = DateTime.Parse(this.photo.created_at);
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
