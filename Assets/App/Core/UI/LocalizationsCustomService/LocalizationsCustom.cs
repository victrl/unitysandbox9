
using System.IO;
using UnityEngine;

namespace App.Core.UI.LocalizationsCustomService
{
    public static class LocalizationsCustom
    {
        public static class Configs
        {
            public static readonly string ConfigDirectoryPath = Path.Combine(Application.dataPath, "App", "Common", "LocalizationsCustom");
            public static readonly string LocalizationConfigPath = Path.Combine(ConfigDirectoryPath, "localizations.json");
        }

        public static class Scripts
        {
            public static readonly string TextLocalizationUidsPath =
                Path.Combine(Application.dataPath, "App", "Common", "LocalizationsCustom", "Constants", "TextLocalizationUids.cs");
            public static readonly string AudioLocalizationUidsPath =
                Path.Combine(Application.dataPath, "App", "Common", "LocalizationsCustom", "Constants", "AudioLocalizationUids.cs");
        }
    }
}