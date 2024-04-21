using System;
using UnityEngine;

namespace PopupSystem.Data
{
    [Serializable]
    public class PopupDefinition
    {
        public string Name => Prefab.name;
        public GameObject Prefab;
    }
}

