using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _500pxManager.Api.Interfaces
{
    public interface ISettingsService
    {
        bool SaveSetting(string name, string value);
        string GetSetting(string name);
        Task<bool> SaveObjectAsync<T>(T obj) where T : class, new();
        Task<T> GetObjectAsync<T>() where T : class, new();
    }
}
