using System.IO;
using App.Core.Storage.StorageService;
using UnityEditor;
using UnityEngine;
using Logger = App.Common.Tools.Logger;


#if UNITY_EDITOR
namespace App.Core.Storage.ProfileService
{
    public partial class ProfileSaveService
    {
        private const string commonPath = "App/Editor/Saves";

#if UNITY_EDITOR_OSX
        [MenuItem("App/Save/Player Prefs/Show", false, 100)]
        public static void ShowPlayerPrefs()
        {
            EditorUtility.OpenWithDefaultApp(string.Format("~/Library/Preferences/unity.{0}.{1}.plist", PlayerSettings.companyName, PlayerSettings.productName));
        }
#endif

        [MenuItem("App/Save/Player Prefs/Export Save", false, 101)]
        public static void ExportPrefsProfileData()
        {
            IStorageService storagePlayerPrefsService = new StoragePlayerPrefs();

            var path = Path.Combine(Application.dataPath, commonPath, "PrefsSave.txt");

            if (File.Exists(path) == false)
            {
                File.Create(path);
                Logger.Log($"[ProfileSaveService] => ImportPrefsProfileData: File was created");
            }

            storagePlayerPrefsService.Load(Storages.Profile, saveData =>
            {
                File.WriteAllText(path, saveData);
                EditorUtility.DisplayDialog("Profile Service", "Export was success", "ok");
            });
        }
        
        [MenuItem("App/Save/Player Prefs/Import Save", false, 102)]
        public static void ImportPrefsProfileData()
        {
            IStorageService storagePlayerPrefsService = new StoragePlayerPrefs();

            var path = Path.Combine(Application.dataPath, commonPath, "PrefsSave.txt");

            
            if (File.Exists(path) == false)
            {
                EditorUtility.DisplayDialog("Profile Service", "File isn't exist", "ok");
                return;
            }
            
            var rawData = File.ReadAllText(path);

            storagePlayerPrefsService.Keep(Storages.Profile, ref rawData, (result) =>
            {
                if (result)
                {
                    EditorUtility.DisplayDialog("Profile Service", "Import was success", "ok");
                    return;
                }
                
                EditorUtility.DisplayDialog("Profile Service", "Import was failed", "ok");
            });
        }
        
        [MenuItem("App/Save/Player Prefs/Clear All", false, 109)]
        public static void ClearPrefsProfileData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
        
        [MenuItem("App/Save/Persistent Datа/Show Path", false, 151)]
        public static void ShowPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("App/Save/Persistent Datа/Сlear Data", false, 152)]
        public static void CleanPersistentDatа()
        {
            FileUtil.DeleteFileOrDirectory(Application.persistentDataPath);
        }
    }
}
#endif