// Storage singleton entity components baker

using Unity.Entities;
using UnityEngine;

namespace App.Core.Storage.StoredValuesDOD
{
    public class StoredValuesMono : MonoBehaviour
    {
        [Tooltip("Delay in seconds between executions of saving data")]
        [Range(StoredValuesSettings.StorageSaveDataDelaySecMin, StoredValuesSettings.StorageSaveDataDelaySecMax)]
        public float saveDataDelaySec = StoredValuesSettings.StorageSaveDataDelaySecMax;
    }
    
    public class StoredValuesBaker : Baker<StoredValuesMono>
    {
        public override void Bake(StoredValuesMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddBuffer<StoredValuesBufferElement>(entity);
            AddComponent<StoredValuesStorageTag>(entity);
            AddComponent(entity, new StoredValuesProperties
            {
                AutoSaveDelayInSec = authoring.saveDataDelaySec
            });
        }
    }
}