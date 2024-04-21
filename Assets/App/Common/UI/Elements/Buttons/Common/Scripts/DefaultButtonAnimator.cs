
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Common.UI.Elements.Buttons
{
    public class DefaultButtonAnimator : MonoBehaviour, IButtonAnimator
    {
        private const float Duration = 0.2f;
        private const float HalfDuration = Duration / 2.0f;
        private readonly Vector3 animScale = new Vector3(0.95f, 0.9f, 0.9f);
        private readonly Color animColor = new Color(0.9f, 0.9f, 0.9f);
        
        public void OnClickAnimation(Image view)
        {
            if (view == null)
            {
                return;
            }

            DOTween.Kill(this);
            DOTween.Sequence().SetId(this)
                .Append(DoColorAnimation(view))
                .Insert(0, DoScaleAnimation(view.transform));
        }

        private Sequence DoColorAnimation(Image image)
        {
            var seq = DOTween.Sequence()
                .Append(image.DOColor(animColor, HalfDuration))
                .Append(image.DOColor(Color.white, HalfDuration));

            return seq;
        }

        private Sequence DoScaleAnimation(Transform transformView)
        {
            var seq = DOTween.Sequence()
                .Append(transformView.DOScale(animScale, HalfDuration))
                .Append(transformView.DOScale(Vector3.one, HalfDuration));

            return seq;
        }
    }
}