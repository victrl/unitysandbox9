
using Unity.Entities;

namespace App.Core.Storage.StoredValuesDOD
{
    public struct StoredValuesProperties : IComponentData
    {
        public float AutoSaveDelayInSec;
    }

    // Storage singleton entity Tag
    public struct StoredValuesStorageTag : IComponentData {}
    
    public struct StoredValuesWasLoadedTag : IComponentData {}

    public struct StoredValuesWaitingForLoadTag : IComponentData {}
}