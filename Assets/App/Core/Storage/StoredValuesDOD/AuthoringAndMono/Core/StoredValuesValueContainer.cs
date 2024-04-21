
using App.Common.Tools;
using Unity.Collections;

namespace App.Core.Storage.StoredValuesDOD
{
    public struct StoredValuesValueContainer
    {
        public FixedString64Bytes Value { get; set; }

        public StoredValuesValueContainer(FixedString64Bytes value)
        {
            Value = value;
        }

        public StoredValuesValueContainer(string value)
        {
            Value = default;
            if (Value.Capacity < value.Length)
            {
                Logger.LogError($"[StoredValuesValueContainer] => Wrong value length");
            }
            
            Value = value;
        }

        public bool Equals(StoredValuesKeyContainer other)
        {
            return Value.Equals(other.Key);
        }
    }
}