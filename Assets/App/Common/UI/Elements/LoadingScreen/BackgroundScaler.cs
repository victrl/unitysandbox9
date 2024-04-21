
using System;
using App.Common.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace App.Common.UI.Elements.LoadingScreen
{
    public class BackgroundScaler : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private RectTransform canvas;

        private void Awake()
        {
            SetAdaptiveBackgroundRect();
        }

#if UNITY_EDITOR
        void Update()
        {
            SetAdaptiveBackgroundRect();
        }
#endif


        private void SetAdaptiveBackgroundRect()
        {
            var backgroundTransform = background.rectTransform;
            var mainTexture = background.mainTexture;

            if (Math.Abs(mainTexture.height - canvas.rect.height) < 0.1f)
            {
                BackgroundTransformRectToDefault();
                return;
            }
        
            if (mainTexture.height > canvas.rect.height)
            {
                float heightIncrement = mainTexture.height - canvas.rect.height;
                BackgroundTransformRectToDefault();
                backgroundTransform.SetTop(-heightIncrement);
                return;
            }

            float needWidth = canvas.rect.height * ((float)mainTexture.width / mainTexture.height);
            float widthIncrement = (needWidth - mainTexture.width) * -1;
            BackgroundTransformRectToDefault();
            backgroundTransform.SetLeft(widthIncrement / 2);
            backgroundTransform.SetRight(widthIncrement / 2);
        }

        private void BackgroundTransformRectToDefault()
        {
            var rectTransform = background.rectTransform;
            rectTransform.offsetMax = new Vector2(0, 0);
            rectTransform.offsetMin = new Vector2(0, 0);
        }
    }
}
