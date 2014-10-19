using _500pxManager.Api.Entities;
using AsyncOAuth;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Security.Authentication.Web;
using Windows.Storage;

namespace _500pxManager.Api.Interfaces
{
    public interface IPxService
    {
        Task SaveAccessTokenAsync(WebAuthenticationResult result);
        Task<User> GetUserAsync();
        Task<bool> LoginAsync();
        Task UploadPhotoAsync(IStorageFile file, Action<UploadOperation> progressAction, string name, string description, Privacy privacy, Category category);
        Task UploadPhotosAsync(IEnumerable<StorageFile> files, Action<UploadOperation> progressAction);
        Task<bool> HaveAccessTokenAsync();
        Task<IEnumerable<Photo>> GetOtherPhotosPagesForUserAsync();
        Task<IEnumerable<Photo>> GetFirstPhotosPageForUserAsync();
    }
}
