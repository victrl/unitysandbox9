Unity Sandbox project
===

Just my personal sandbox created based on the core of one of my projects. Initially, to explore new features of Unity DOTS (Entities 1.x.x).

## Table of Contents
- [Used](#used)
- [Implemented](#implemented)
- [Usage](#usage)
  - [Scenes](#scenes)
  - [Stored values](#stored-values)
  - [Rewards](#rewards)

Used
---
- Unity 2022
- DOTS (Entities, Jobs, Burst)
- UI Toolkit
- Zenject
- UniTask
- DOTween
- Unity Localization

Implemented
---
- Storage (as a service) - save and load data (txt file | PlayerPrefs - can be changed in AppContext.RegistrationServices())
- Profile save (as a service)
- [Stored values](#stored-values) (ECS) - any key-values to store by Storage service
- [Rewards](#rewards) (two implementations: serive and ECS) - earn and keep any user rewards
- Rewards UI (UI Toolkit: UIDocument)
- Popups (as a service)
- Scene transitions (as a service)
- Localizations (Unity)
- Localizations (as a service) - custom implementation

Usage
---
- Editor menu: **App**
- Start Scene: **Assets/App/AppScenes/AppContext**
- User data: **Assets/App/Editor/Saves/**

### Scenes
- AppContext - start scene
- AppMenu
- Game01 - temporarily, Game01 uses an example from a third-party educational project: https://www.youtube.com/watch?v=IO6_6Y_YUdE

### Stored values
To add a new type of saved data you must:
1. Create a new component type following the StoredValueInt example.
2. Register this type for Jobs at the beginning of the StoredValuesBufferSystem.cs file.
3. Add a call to buffer values of this type in StoredValuesBufferSystem.BufferingStoredValues
4. Add a call to fill data of this type in StoredValuesLoadSystem.PublishValuesToEntities

### Rewards
To implement rewards income event in script:

```csharp
var rewardsIncome = new RewardsIncome(1, RewardsKeys.Coins);
ECB.AddComponent(SORT_KEY, ENTITY, rewardsIncome);
```

