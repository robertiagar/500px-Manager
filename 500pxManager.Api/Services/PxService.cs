using _500pxManager.Api.Interfaces;
using AsyncOAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Security.Authentication.Web;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using System.Net.Http.Headers;
using _500pxManager.Api.Entities;
using Newtonsoft.Json.Linq;

namespace _500pxManager.Api.Services
{
    public class PxService : IPxService
    {
        private const string consumerKey = "nGr9Xrl4hMz5njA64KeOFuJS7u85NwOHSMS8BCAv";
        private const string consumerSecret = "TVJadczaVu0gm479gmbGWe32CoiVaN3Q8MbrfjRS";
        private RequestToken requestToken;
        private ISettingsService settings;

        public PxService(ISettingsService settings)
        {
            AsyncOAuth.OAuthUtility.ComputeHash = (key, buffer) =>
            {
                var crypt = Windows.Security.Cryptography.Core.MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");

                var keyBuffer = Windows.Security.Cryptography.CryptographicBuffer.CreateFromByteArray(key);

                var cryptKey = crypt.CreateKey(keyBuffer);


                var dataBuffer = Windows.Security.Cryptography.CryptographicBuffer.CreateFromByteArray(buffer);

                var signBuffer = Windows.Security.Cryptography.Core.CryptographicEngine.Sign(cryptKey, dataBuffer);

                byte[] value;

                Windows.Security.Cryptography.CryptographicBuffer.CopyToByteArray(signBuffer, out value);

                return value;
            };

            this.settings = settings;
        }

        private async Task<RequestToken> GetRequestTokenAsync()
        {
            var client = new OAuthAuthorizer(consumerKey, consumerSecret);
            var result = await client.GetRequestToken("https://api.500px.com/v1/oauth/request_token");

            return result.Token;
        }

        public async Task<bool> LoginAsync()
        {
            if (await GetAccessTokenAsync() == null)
            {
                if (requestToken == null)
                {
                    requestToken = await GetRequestTokenAsync();
                }

                var client = new OAuthAuthorizer(consumerKey, consumerSecret);

                var authorizeUrl = client.BuildAuthorizeUrl("https://api.500px.com/v1/oauth/authorize", requestToken);

                WebAuthenticationBroker.AuthenticateAndContinue(new Uri(authorizeUrl), new Uri("http://www.robertiagar.com"));

                return true;
            }

            return false;
        }

        public async Task SaveAccessTokenAsync(WebAuthenticationResult result)
        {
            var client = new OAuthAuthorizer(consumerKey, consumerSecret);

            var verifierIndex = result.ResponseData.IndexOf("oauth_verifier");
            var verifier = result.ResponseData.Substring(verifierIndex).Split('=')[1];

            var accessToken = await client.GetAccessToken("https://api.500px.com/v1/oauth/access_token", requestToken, verifier);

            await settings.SaveObjectAsync<AccessToken>(accessToken.Token);
        }

        private async Task<AccessToken> GetAccessTokenAsync()
        {
            var accessToken = await settings.GetObjectAsync<AccessToken>();
            if (accessToken != null)
            {
                return accessToken;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> HaveAccessTokenAsync()
        {
            var accessToken = await GetAccessTokenAsync();
            if (accessToken != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<HttpClient> GetOAuthClientAsync()
        {
            var accessToken = await GetAccessTokenAsync();
            return OAuthUtility.CreateOAuthClient(consumerKey, consumerSecret, accessToken);
        }

        public async Task<User> GetUserAsync()
        {
            var client = await GetOAuthClientAsync();

            var result = await client.GetStringAsync("https://api.500px.com/v1/users");

            var user = JsonConvert.DeserializeObject<UserRoot>(result);

            return user.User;
        }

        public async Task UploadPhotosAsync(IEnumerable<StorageFile> files, Action<UploadOperation> progressAction)
        {
            foreach (var file in files)
            {
                await Task.Delay(0);
            }
        }

        public async Task UploadPhotoAsync(IStorageFile file, Action<UploadOperation> progressAction, string name, string description, Privacy privacy, Category category)
        {
            var uri = string.Format("https://api.500px.com/v1/photos/upload?name={0}&description={1}&privacy={2}&category={3}", name, description, (int)privacy, (int)category);
            var backgroudUploader = new BackgroundUploader();
            var headers = OAuthUtility.BuildBasicParameters(consumerKey, consumerSecret, uri, HttpMethod.Post, await GetAccessTokenAsync());
            var header = string.Empty;
            foreach (var item in headers)
            {
                header += string.Format(@"{0}=""{1}"", ", item.Key, item.Value);
            }

            header = header.Remove(header.Length - 2);

            backgroudUploader.SetRequestHeader("Authorization", string.Format("OAuth {0}", header));
            backgroudUploader.SetRequestHeader("Filename", file.Name);
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            backgroudUploader.SetRequestHeader("Content-Type", "multipart/form-data; boundary=" + boundary);
            backgroudUploader.Method = "POST";

            var parts = new List<BackgroundTransferContentPart>();
            var part = new BackgroundTransferContentPart();
            part.SetHeader("Content-Disposition", @"form-data; name=""file""; filename=""" + file.Name + "\"");
            part.SetFile(file);
            parts.Add(part);

            var op = await backgroudUploader.CreateUploadAsync(new Uri(uri), parts, "", boundary);
            try
            {
                Progress<UploadOperation> progressCallback = new Progress<UploadOperation>(progressAction);
                var result = await op.StartAsync().AsTask(progressCallback);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task<IEnumerable<Photo>> GetOtherPhotosPagesForUserAsync()
        {
            var client = await GetOAuthClientAsync();
            var user = await GetUserAsync();

            var uri = string.Format("https://api.500px.com/v1/photos?feature=user&username={0}&page={1}", user.username, 2);
            var json = await client.GetStringAsync(uri);

            JObject jsonResult = JObject.Parse(json);
            var photosJson = jsonResult["photos"];
            var pages = (int)jsonResult["total_pages"];
            var currentPage = (int)jsonResult["current_page"];
            var photos = JsonConvert.DeserializeObject<IEnumerable<Photo>>(photosJson.ToString());

            var photosResult = new List<Photo>();
            photosResult.AddRange(photos);
            for (currentPage = 3; currentPage < pages; currentPage++)
            {
                uri = string.Format("https://api.500px.com/v1/photos?feature=user&username={0}&page={1}", user.username, currentPage);
                json = await client.GetStringAsync(uri);

                jsonResult = JObject.Parse(json);
                photosJson = jsonResult["photos"];
                photos = JsonConvert.DeserializeObject<IEnumerable<Photo>>(photosJson.ToString());
                photosResult.AddRange(photos);
            }

            return photosResult;
        }

        public async Task<IEnumerable<Photo>> GetFirstPhotosPageForUserAsync()
        {
            var client = await GetOAuthClientAsync();
            var user = await GetUserAsync();

            var uri = string.Format("https://api.500px.com/v1/photos?feature=user&username={0}&page={1}", user.username, 1);
            var json = await client.GetStringAsync(uri);

            JObject jsonResult = JObject.Parse(json);
            var photosJson = jsonResult["photos"];
            var pages = (int)jsonResult["total_pages"];
            var currentPage = (int)jsonResult["current_page"];
            var photos = JsonConvert.DeserializeObject<IEnumerable<Photo>>(photosJson.ToString());

            var photosResult = new List<Photo>();
            photosResult.AddRange(photos);
            return photosResult;
        }


        public async Task<IEnumerable<Photo>> RefreshPhotosAsync()
        {
            return await GetFirstPhotosPageForUserAsync();
        }
    }
}
