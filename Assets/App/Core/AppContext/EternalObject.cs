
using UnityEngine;

namespace App.Core.AppContext
{
    public class EternalObject : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
