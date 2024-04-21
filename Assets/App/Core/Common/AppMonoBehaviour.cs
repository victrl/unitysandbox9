using App.Core.Common.Services;
using UnityEngine;
using UnityEngine.UI;

namespace App.Core.Common
{
    public abstract class AppMonoBehaviour : MonoBehaviour
    {
        private Transform cachedTransform = null;
        private RectTransform cachedRectTransform = null;
        private LayoutElement cachedLayoutElement = null;
        private Collider cachedCollider = null;
        private Rigidbody cachedRigidbody = null;
        private Rigidbody2D cachedRigidbody2D = null;
        private Animator cachedAnimator = null;
        private Animation cachedAnimation = null;
        private CanvasGroup cachedCanvasGroup = null;
        private Renderer cachedRenderer = null;
        private Camera cachedCamera = null;

        public Renderer CachedRenderer => TryGetComponent(ref cachedRenderer);

        public CanvasGroup CachedCanvasGroup
        {
            get => TryGetComponent(ref cachedCanvasGroup);
            set => cachedCanvasGroup = value;
        }

        protected virtual void Awake()
        {
            if (DIInstaller.GlobalContainer != null)
            {
                DIInstaller.GlobalContainer.Inject(this);
            }
        }

        public RectTransform CachedRectTransform => TryGetComponent(ref cachedRectTransform);
        public LayoutElement CachedLayoutElement => TryGetComponent(ref cachedLayoutElement);
        public Transform CachedTransform => TryGetComponent(ref cachedTransform);
        public Collider CachedCollider => TryGetComponent(ref cachedCollider);
        public Rigidbody CachedRigidbody => TryGetComponent(ref cachedRigidbody);
        public Rigidbody2D CachedRigidbody2D => TryGetComponent(ref cachedRigidbody2D);
        public Animator CachedAnimator => TryGetComponent(ref cachedAnimator);
        public Animation CachedAnimation => TryGetComponent(ref cachedAnimation);
        public Camera CachedCamera => TryGetComponent(ref cachedCamera);

        private T TryGetComponent<T>(ref T value) where T : Component
        {
            if (ReferenceEquals(value, null))
            {
                value = transform.GetComponent<T>();
            }

            return value;
        }
    }
}