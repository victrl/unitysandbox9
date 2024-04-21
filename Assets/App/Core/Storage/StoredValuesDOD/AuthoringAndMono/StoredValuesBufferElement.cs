
using Unity.Collections;
using Unity.Entities;

namespace App.Core.Storage.StoredValuesDOD
{
    // Buffer element of values to be stored/restored to/from file.
    // For transfer elements from source values to save data system.
    [InternalBufferCapacity(8)]
    public struct StoredValuesBufferElement : IBufferElementData
    {
        public FixedString64Bytes Key;
        public FixedString64Bytes Value;
    }
}