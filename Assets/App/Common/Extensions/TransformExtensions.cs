
using UnityEngine;

namespace App.Common.Extensions
{
    public static class TransformExtensions
    {
        public static void SetLayerRecursive(this Transform view, int layer)
        {
            var objects = view.GetComponentsInChildren<Transform>(true);
        
            foreach (var includedObject in objects)
            {
                includedObject.gameObject.layer = layer;
            }
        
            view.gameObject.layer = layer;
        }

        public static Transform GetChildByName(this Transform holder, string targetName)
        {
            var childCount = holder.childCount;

            if (childCount == 0)
            {
                return null;
            }

            for (var i = 0; i < childCount; i++)
            {
                var currentElement = holder.GetChild(i);
                if (string.Equals(currentElement.name, targetName))
                {
                    return currentElement;
                }

                var child = currentElement.GetChildByName(targetName);
                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }
    }
}