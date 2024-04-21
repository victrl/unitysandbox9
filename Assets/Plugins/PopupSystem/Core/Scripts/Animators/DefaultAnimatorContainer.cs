using UnityEngine;

namespace PopupSystem.Animators
{
    public class DefaultAnimatorContainer : MonoBehaviour
    {
        [Header("Animatinons parameters")] [SerializeField]
        protected Animator animator;

        [SerializeField] private AnimationClip openClip;
        [SerializeField] private AnimationClip closeClip;

        public AnimationClip OpenWindowClip => openClip;
        public AnimationClip CloseWindowClip => closeClip;
        public Animator Animator => animator;
    }
}