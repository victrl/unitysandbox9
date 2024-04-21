using System;

namespace App.Core.ConfigLoader
{
    public interface IConfigLoader
    {
        void LoadData<T>(string configPath, Action<T> onLoad);
    }
}