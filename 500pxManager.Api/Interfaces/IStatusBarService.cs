using System;
using System.Threading.Tasks;
namespace _500pxManager.Api.Interfaces
{
    public interface IStatusBarService
    {
        void DisplayMessage(string message);
        Task DisplayMessage(string messsage, int delay, bool displayProgressBar = true);
        Task DisplayMessage(string message, int delay);
        void DisplayProgress(double? progress, string message = null);
        Task HideProgressAsync();
        Task ShowProgressAsync();
    }
}
