
using System;
using App.Common.Tools;
using Unity.Collections;

namespace App.Core.Storage.StoredValuesDOD
{
    public struct StoredValuesKeyContainer : IFixedKey<FixedString64Bytes>, IEquatable<StoredValuesKeyContainer>
    {
        public FixedString64Bytes Key { get; set; }

        public StoredValuesKeyContainer(FixedString64Bytes key)
        {
            Key = key;
        }

        public StoredValuesKeyContainer(string key)
        {
            Key = default;
            if (Key.Capacity < key.Length)
            {
                Logger.LogError($"[StoredValuesKeyContainer] => Wrong key length");
            }
            
            Key = key;
        }

        public bool Equals(StoredValuesKeyContainer other)
        {
            return Key.Equals(other.Key);
        }
    }
}