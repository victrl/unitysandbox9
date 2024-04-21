using System;
using System.IO;
using Newtonsoft.Json;

namespace App.Core.ConfigLoader
{
    public class LocalConfigLoader : IConfigLoader
    {
        public void LoadData<T>(string configPath, Action<T> onLoad)
        {
            string data = File.ReadAllText(configPath);
            T result = JsonConvert.DeserializeObject<T>(data);
            onLoad?.Invoke(result);
        }
    }
}