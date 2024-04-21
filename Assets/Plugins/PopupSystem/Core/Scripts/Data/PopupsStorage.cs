using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PopupSystem.Data
{
    [CreateAssetMenu(fileName = "PopupsStorage", menuName = "PopupSystem/Create storage", order = 51)]
    public class PopupsStorage : ScriptableObject
    {
        [Header("Popups")]
        [SerializeField] private List<GameObject> popups;

        private List<PopupDefinition> definitions;

        public void Init()
        {
            definitions = new List<PopupDefinition>();
                
            foreach (var prefab in popups)
            {
                definitions.Add(new PopupDefinition()
                {
                    Prefab = prefab
                });
            }
        }
        
        public PopupDefinition GetPopupDefinition(string name)
        {
            var resultPopup = definitions.Find((popup) => popup.Name.Equals(name));
            
            if (resultPopup != null)
            {
                return resultPopup;
            }

            throw new Exception($"[StoragePopups] => GetPopupDefinition: This type - [{name}] doesn't exist");
        }
    }
}
