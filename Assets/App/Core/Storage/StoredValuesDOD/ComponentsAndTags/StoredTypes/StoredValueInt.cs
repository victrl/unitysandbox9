// Stored values component with type of int

using App.Core.Meta.RewardsDOD;
using Unity.Collections;
using Logger = App.Common.Tools.Logger;

namespace App.Core.Storage.StoredValuesDOD
{
    public struct StoredValueInt : IStoredValueComponent
    {
        public int Value;

        public StoredValuesKeyContainer KeyContainer { get; }

        // store/restore stored values by strings
        public FixedString64Bytes StoredValue
        {
            get
            {
                FixedString64Bytes fsValue = default;
                fsValue.Append(Value);
                return fsValue;
            }
            set
            {
                var offset = 0;
                var restoredValue = 0;
                var parseResult = value.Parse(ref offset, ref restoredValue);
                if (parseResult != ParseError.None)
                {
                    Logger.LogError($"[StoredValueInt] for Key {KeyContainer.Key} => {parseResult}");
                }
                else
                {
                    Value += restoredValue;
                }
            }
        }

        public StoredValueInt(StoredValuesKeyContainer storedValuesKeyContainer) : this()
        {
            KeyContainer = storedValuesKeyContainer;
        }
    }
}