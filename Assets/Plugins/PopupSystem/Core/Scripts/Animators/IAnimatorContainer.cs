using UnityEngine;

namespace PopupSystem.Animators
{
    public interface IAnimatorContainer
    {
        public Animator Animator { get; }
        public AnimationClip OpenWindowClip { get; }
        public AnimationClip CloseWindowClip { get; }
    }
}