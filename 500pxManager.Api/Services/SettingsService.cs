using _500pxManager.Api.Interfaces;
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
