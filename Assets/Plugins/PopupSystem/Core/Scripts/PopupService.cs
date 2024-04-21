using System;
using System.Collections;
using System.Collections.Generic;
using PopupSystem.Data;
using UnityEngine;
using UnityEngine.UI;

namespace PopupSystem
{
    public class PopupService : MonoBehaviour
    {
        [Header("List popups")] [SerializeField]
        PopupsStorage storage;

        protected Stack<PopupView> currentPopups = new Stack<PopupView>();
        protected Stack<GameObject> currentBackground = new Stack<GameObject>();

        private void Awake()
        {
            storage.Init();
        }

        public void ShowPopup(string name, IPopupModel model)
        {
            PopupDefinition definition = storage.GetPopupDefinition(name);
            ShowPopup(definition.Prefab, model);
        }
        
        public void ShowPopup(GameObject prefab, IPopupModel model)
        {
            GameObject popup = Instantiate(prefab, transform);
            PopupView popupView = popup.GetComponent<PopupView>();
            popupView.Init(this, model);
            currentPopups.Push(popupView);
            popupView.Show();
            
            GameObject background = BackgroundController.CreateBackground(popup.transform);
            currentBackground.Push(background);
            StartCoroutine(BackgroundController.FadeIn(background.GetComponent<Image>(), 0.2f));
        }

        public void ClosePopup()
        {
            var closingPopup = currentPopups.Pop();
            closingPopup.Hide(() => { Destroy(closingPopup.gameObject); });

            var topmostBackground = currentBackground.Pop();
            if (topmostBackground != null)
            {
                StartCoroutine(BackgroundController.FadeOut(topmostBackground.GetComponent<Image>(), 0.2f, () => Destroy(topmostBackground)));
            }
        }

        private static class BackgroundController
        {
            public static GameObject CreateBackground(Transform parent)
            {
                var panel = new GameObject("Panel");
                var panelImage = panel.AddComponent<Image>();
                var color = Color.black;
                color.a = 0;
                panelImage.color = color;
                var panelTransform = panel.GetComponent<RectTransform>();
                panelTransform.anchorMin = Vector2.zero;
                panelTransform.anchorMax = Vector2.one;
                panelTransform.pivot = new Vector2(0.5f, 0.5f);
                
                foreach (var child in parent.GetComponentsInChildren<RectTransform>(true))
                {
                    if (child.GetComponent<Canvas>())
                    {
                        panel.transform.SetParent(child, false);
                        panel.transform.SetSiblingIndex(0);
                    }
                }
                return panel;
            }

            public static IEnumerator FadeIn(Image image, float time)
            {
                var alpha = image.color.a;
                for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
                {
                    var color = image.color;
                    color.a = Mathf.Lerp(alpha, 220 / 256.0f, t);
                    image.color = color;
                    yield return null;
                }
            }

            public static IEnumerator FadeOut(Image image, float time, Action onComplete)
            {
                var alpha = image.color.a;
                for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
                {
                    var color = image.color;
                    color.a = Mathf.Lerp(alpha, 0, t);
                    image.color = color;
                    yield return null;
                }

                onComplete?.Invoke();
            }
        }
    }
}