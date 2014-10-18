﻿using _500pxManager.Api.Entities;
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
        Task UploadPhotoAsync(StorageFile file, Action<UploadOperation> progressAction);
        Task UploadPhotosAsync(IEnumerable<StorageFile> files, Action<UploadOperation> progressAction);
        Task<bool> HaveAccessTokenAsync();
    }
}