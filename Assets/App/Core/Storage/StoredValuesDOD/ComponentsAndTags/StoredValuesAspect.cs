using Unity.Entities;
using Unity.Transforms;

namespace App.Core.Storage.StoredValuesDOD
{
    public readonly partial struct StoredValuesAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<LocalTransform> transform;
        private readonly RefRO<StoredValuesProperties> storedValuesProperties;
        private readonly DynamicBuffer<StoredValuesBufferElement> storageBuffer;

        public float AutoSaveDelayInSec => storedValuesProperties.ValueRO.AutoSaveDelayInSec;

        public DynamicBuffer<StoredValuesBufferElement> StorageBuffer => storageBuffer;
        
        public bool IsStorageBufferEmpty()
        {
            return storageBuffer.IsEmpty;
        }
    }
}
