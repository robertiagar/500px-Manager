using System;
using System.Threading.Tasks;
namespace _500pxManager.Api.Interfaces
{
    public interface IStatusBarService
    {
        void DisplayMessage(string message);
        void DisplayMessage(string message, bool displayProgressBar = false);
        Task DisplayMessage(string message, int delay);
        Task DisplayMessage(string messsage, int delay, bool displayProgressBar = true);
        void DisplayProgress(double? progress, string message = null);
        Task HideProgressAsync();
        Task ShowProgressAsync();
    }
}
