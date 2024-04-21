
using Unity.Collections;
using Unity.Entities;

namespace App.Core.Storage.StoredValuesDOD
{
    // For any values to be stored/restored to/from file
    public interface IStoredValueComponent : IComponentData
    {
        public StoredValuesKeyContainer KeyContainer { get; }

        public FixedString64Bytes StoredValue { get; set; }
    }
}