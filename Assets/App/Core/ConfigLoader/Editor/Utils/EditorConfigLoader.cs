using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.Networking;

namespace App.Core.ConfigLoader
{
    public static class EditorConfigLoader
    {
        private const string LocalizationConfigUrl = "https://docs.google.com/spreadsheets/d/1Gk8Yo1ph8YQyhyjFHfCC838ZKvSeuMyH_3SCWW8UoL4/gviz/tq?tqx=out:csv";

        [MenuItem("App/Configs/Localization/Load localization config")]
        public static void LoadData()
        {
            SendRequest(LocalizationConfigUrl, LocalizationConfigParser.ParseAndSaveData);
        }
        
        [MenuItem("App/Configs/Localization/Generate audio units")]
        public static void GenerateAudioUnits()
        {
            AudioUtils.GenerateClipUnits();
        }

        private static async void SendRequest(string url, Action<string> onLoad)
        {
            using var request = UnityWebRequest.Get(url);
            request.SendWebRequest();

            while (!request.isDone)
            {
                await Task.Yield();
            }

            onLoad?.Invoke(request.downloadHandler.text);
        }
    }

}