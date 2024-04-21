using System;

namespace PopupSystem
{
    public interface IButtonView
    {
        void RemoveAllListeners();
        void AddListener(Action<IButtonView> onClick);
        T GetAdditionalData<T>() where T : class;
    }
}