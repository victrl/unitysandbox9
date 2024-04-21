using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = App.Common.Tools.Logger;

namespace App.Core.UI.LocalizationsCustomService.Audio
{
    public class AudioStorageService : MonoService
    {
        public List<ClipContainer> clipContainers = new List<ClipContainer>();

        public ClipContainer GetClipContainer(SystemLanguage language)
        {
            var clipContainer = clipContainers.Find(container => container.name.Equals(language.ToString()));

            if (clipContainer == null)
            {
                Logger.LogWarning($"[AudioStorageService] => GetClipContainer: Unsupported language {language.ToString()}");
                return clipContainers.First();
            }

            return clipContainer;
        }
    }
}