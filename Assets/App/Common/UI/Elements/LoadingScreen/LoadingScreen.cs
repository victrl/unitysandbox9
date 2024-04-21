
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace App.Common.UI.Elements.LoadingScreen
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private List<Transform> loadingPoints;
        [SerializeField] private CanvasGroup canvasGroup;

        private Sequence screenAnimSeq;
        private Sequence loadingAnimSeq;
    
        public void SetAnimationState(bool active)
        {
            if (loadingPoints == null || loadingPoints.Count < 1) return;

            canvasGroup.gameObject.SetActive(true);

            var d1 = 0.4f;
            var alpha = active ? 1.0f : 0.0f;

            screenAnimSeq?.Kill();
            screenAnimSeq = DOTween.Sequence()
                .Append(canvasGroup.DOFade(alpha, d1))
                .AppendCallback(() =>
                {
                    SetPointsAnimationState(active);
                    canvasGroup.gameObject.SetActive(active);
                });
        }
    
        private void SetPointsAnimationState(bool active)
        {
            loadingAnimSeq?.Kill();

            if (!active) return;
        
            loadingAnimSeq = DOTween.Sequence();

            const float duration = 0.6f;
            const float d1 = duration * 0.6f;
            const float d2 = (duration - d1) * 0.5f;
            const float d3 = duration - d2 - d1;

            const float scale1 = 2.0f;

            foreach (var loadingPoint in loadingPoints)
            {
                loadingPoint.localScale = Vector3.one;

                loadingAnimSeq.Append(loadingPoint.DOScale(Vector3.one * scale1, d1).SetEase(Ease.InOutSine));
                loadingAnimSeq.Append(loadingPoint.DOScale(Vector3.one, d2).SetEase(Ease.InSine));
                loadingAnimSeq.Append(loadingPoint.DOScale(Vector3.one, d3).SetEase(Ease.OutElastic));
            }

            loadingAnimSeq.SetLoops(-1);
        }

        private void OnDestroy()
        {
            screenAnimSeq?.Kill();
            loadingAnimSeq?.Kill();
        }
    }
}
