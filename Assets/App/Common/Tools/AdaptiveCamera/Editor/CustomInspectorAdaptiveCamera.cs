#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace App.Common.Tools.AdaptiveCamera
{
    [CustomEditor(typeof(AdaptiveCameraComponent))]
    public class CustomInspectorAdaptiveCamera : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var currentInstance = (AdaptiveCameraComponent) target;
           

            if (GUILayout.Button("Set Parameters"))
            {
                if (currentInstance == null)
                {
                    return;
                }
                
                var camera = currentInstance.gameObject.GetComponent<Camera>();
                
                if (camera == null)
                {
                    return;
                }

                currentInstance.ApplyCurrentParameters(currentInstance.gameObject.GetComponent<Camera>());
            }
        }
    }
}
#endif