using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = App.Common.Tools.Logger;

namespace App.Core.UI.LocalizationsCustomService.Audio
{
    [Serializable]
    public class ClipUnit
    {
        public string ClipName;
        public AudioClip Resource;
    }

    [CreateAssetMenu(fileName = "ClipsContainer", menuName = "AudioSystem/ClipsContainer", order = 1)]
    public class ClipContainer : ScriptableObject
    {
        [SerializeField] private List<ClipUnit> clipData;

        private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
        public IReadOnlyList<ClipUnit> ClipData => clipData;

        public void Init()
        {
            clips.Clear();
            foreach (var clip in clipData)
            {
                clips.Add(clip.ClipName, clip.Resource);
            }
        }

        public void AddNewClipData(List<ClipUnit> data)
        {
            clipData = new List<ClipUnit>();
            clipData.AddRange(data);
        }

        public AudioClip GetAudioClip(string clipName)
        {
            if (clips.Count == 0)
            {
                Init();
            }

            if (clips.TryGetValue(clipName, out var clip))
            {
                return clip;
            }

            Logger.LogWarning($"[ClipContainer] => GetAudioClip: clipName isn't exist");
            return null;
        }

        public List<string> GetKeys()
        {
            Init();
            return clips.Keys.ToList();
        }
    }
}
