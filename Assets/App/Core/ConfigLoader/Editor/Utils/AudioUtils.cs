using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Common.Tools;
using App.Core.UI.LocalizationsCustomService;
using App.Core.UI.LocalizationsCustomService.Audio;
using UnityEditor;
using UnityEngine;
using Logger = App.Common.Tools.Logger;

namespace App.Core.ConfigLoader
{
    public static class AudioUtils
    {
        private static string pathToAudioData => Path.Combine("Assets", "App", "Common", "Audio");

        private static string pathToEnglishAudioData => Path.Combine(pathToAudioData, "English");
        private static string pathToGermanAudioData => Path.Combine(pathToAudioData, "German");
        
        private const string SearchPatternAudioMp3 = "*.mp3";
        private const string SearchPatternAsset = "*.asset";
        
        private static List<ClipContainer> clipContainers = new List<ClipContainer>();

        public static void GenerateClipUnits()
        {
            clipContainers.Clear();
            clipContainers.Add(PrepareClipContainer(pathToEnglishAudioData));
            clipContainers.Add(PrepareClipContainer(pathToGermanAudioData));

            if (CheckAudioClips(clipContainers))
            {
                List<string> contentForCodeGen = new List<string>();

                foreach (var clip in clipContainers[0].ClipData)
                {
                    contentForCodeGen.Add(CodeGenerator.GetFieldForGenerator<string>(clip.ClipName, clip.ClipName));
                }
                
                CodeGenerator.Generate(LocalizationsCustom.Scripts.AudioLocalizationUidsPath, contentForCodeGen);
            }
        }

        private static ClipContainer PrepareClipContainer(string path)
        {
            var sourceDirInfo = new DirectoryInfo(path);
            var allFiles = sourceDirInfo.GetFiles(SearchPatternAudioMp3, SearchOption.AllDirectories);
            var containerFile = sourceDirInfo.GetFiles(SearchPatternAsset, SearchOption.AllDirectories);
            var containerPath = Path.Combine(path, $"{path.Split(Path.DirectorySeparatorChar).Last()}.asset");
            var container = AssetDatabase.LoadAssetAtPath<ClipContainer>(containerPath);

            var clipUnits = new List<ClipUnit>();
            
            foreach (var file in allFiles)
            {
                bool isLocalDirectory = false;
                string filePath = string.Empty;
                
                foreach (var directoryName in file.FullName.Split(Path.DirectorySeparatorChar))
                {
                    if (directoryName.Equals("Assets"))
                    {
                        isLocalDirectory = true;
                    }

                    if (isLocalDirectory)
                    {
                        filePath = Path.Combine(filePath, directoryName);
                    }
                }
                var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(filePath);
                
                if (clip != null)
                {
                    clipUnits.Add(new ClipUnit()
                    {
                        ClipName = clip.name,
                        Resource = clip,
                    });
                }
            }
            
            container.AddNewClipData(clipUnits);
            AssetDatabase.SaveAssets();
            return container;
        }

        private static bool CheckAudioClips(List<ClipContainer> containers)
        {
            bool result = true;
            
            for (int i = 0; i < containers.Count; i++)
            {
                var targetContainer = containers[i];
                
                for (int j = 0; j < containers.Count; j++)
                {
                    if (j == i)
                    {
                        continue;
                    }
                    
                    var observeContainer = containers[j];
                    var observeContainerKeys = observeContainer.GetKeys();

                    foreach (var targetContainerAudioKey in targetContainer.GetKeys())
                    {
                        if (observeContainerKeys.Contains(targetContainerAudioKey) == false)
                        {
                            result = false;
                            Logger.LogError($"[AudioUtils] => CheckAudioClips: container: {observeContainer.name} - Has not key {targetContainerAudioKey}");
                        }
                    }
                }
            }

            return result;
        }
    }
}