using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _500pxManager.Interfaces
{
    public interface INavigationService
    {
        void Navigate(Type type);
        void Navigate(Type type, object parameter);
        void Navigate(string type);
        void Navigate(string type, object parameter);
        void GoBack();
    }
}
