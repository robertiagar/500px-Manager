using System;
using System.Threading.Tasks;
namespace _500pxManager.Api.Interfaces
{
    public interface IStatusBarService
    {
        void DisplayMessage(string message);
        void DisplayProgress(double? progress, string message = null);
        Task HideProgressAsync();
        Task ShowProgressAsync();
    }
}
