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

namespace _500pxManager.Api.Services
{
    public class PxService
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

        public async Task LoginAsync()
        {
            if (GetAccessToken() == null)
            {
                if (requestToken == null)
                {
                    requestToken = await GetRequestTokenAsync();
                }

                var client = new OAuthAuthorizer(consumerKey, consumerSecret);

                var authorizeUrl = client.BuildAuthorizeUrl("https://api.500px.com/v1/oauth/authorize", requestToken);

                WebAuthenticationBroker.AuthenticateAndContinue(new Uri(authorizeUrl), new Uri("http://www.robertiagar.com"));
            }
        }

        public async Task GetAccessTokenAsync(WebAuthenticationResult result)
        {
            var client = new OAuthAuthorizer(consumerKey, consumerSecret);

            var verifierIndex = result.ResponseData.IndexOf("oauth_verifier");
            var verifier = result.ResponseData.Substring(verifierIndex).Split('=')[1];

            var accessToken = await client.GetAccessToken("https://api.500px.com/v1/oauth/access_token", requestToken, verifier);

            settings.SaveSetting("accessTokenSecret", accessToken.Token.Secret);
            settings.SaveSetting("accessTokenKey", accessToken.Token.Key);
        }

        private AccessToken GetAccessToken()
        {
            var key = settings.GetSetting("accessTokenKey");
            var secret = settings.GetSetting("accessTokenSecret");
            if (key != null && secret != null)
            {
                return new AccessToken(key, secret);
            }
            else
            {
                return null;
            }
        }

        private HttpClient GetOAuthClient()
        {
            var accessToken = GetAccessToken();
            return OAuthUtility.CreateOAuthClient(consumerKey, consumerSecret, accessToken);
        }

        public async Task GetUserAsync()
        {
            var client = GetOAuthClient();

            var result = await client.GetStringAsync("https://api.500px.com/v1/users");

            Windows.UI.Popups.MessageDialog dialog = new Windows.UI.Popups.MessageDialog(result);
            await dialog.ShowAsync();
        }

        public void UploadPhoto()
        {

            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.PickSingleFileAndContinue();


        }

        public async Task UploadPhotoContinueAsync(IEnumerable<StorageFile> files)
        {
            var uri = string.Format("https://api.500px.com/v1/photos/upload?name={0}&description={1}&privacy=0&category=0", "test name", "test description");
            var backgroudUploader = new BackgroundUploader();
            var headers = OAuthUtility.BuildBasicParameters(consumerKey, consumerSecret, uri, HttpMethod.Post, GetAccessToken());
            var header = string.Empty;
            var file = files.First();
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
            part.SetFile(file);
            parts.Add(part);

            var op = await backgroudUploader.CreateUploadAsync(new Uri(uri), parts, "", boundary);
            try
            {
                var result = await op.StartAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
