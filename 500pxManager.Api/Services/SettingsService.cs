using _500pxManager.Api.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace _500pxManager.Api.Services
{
    public class SettingsService : ISettingsService
    {
        public async Task<bool> SaveObjectAsync<T>(T obj) where T : class, new()
        {
            try
            {
                var json = JsonConvert.SerializeObject(obj);
                var foloder = ApplicationData.Current.LocalFolder;
                var file = await foloder.CreateFileAsync(obj.GetType().Name, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<T> GetObjectAsync<T>() where T : class, new()
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.GetFileAsync(typeof(T).Name);
                var json = await FileIO.ReadTextAsync(file);
                var returnObj = JsonConvert.DeserializeObject<T>(json);
                return returnObj;
            }
            catch
            {
                return null;

            }
        }


        public bool SaveSetting(string name, string value)
        {
            try
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values[name] = value;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private object GetSettingObject(string name)
        {
            var localSeetings = ApplicationData.Current.LocalSettings;
            var value = localSeetings.Values[name];
            return value;
        }

        public string GetSetting(string name)
        {
            var value = GetSettingObject(name);
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
