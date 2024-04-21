using App.Core.Meta;
using App.Core.Meta.RewardsDOD;
using UnityEngine;
using UnityEngine.UIElements;

namespace App.AppUI.Rewards
{
    public class UIRewardsCountLabel : Label
    {
        new class UxmlFactory : UxmlFactory<UIRewardsCountLabel, UxmlTraits> { }

        new class UxmlTraits : Label.UxmlTraits
        {
            private readonly UxmlEnumAttributeDescription<RewardsKeys> rewardsType = new UxmlEnumAttributeDescription<RewardsKeys> { name = "rewards-type", defaultValue = RewardsKeys.Coins };
            
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var label = (UIRewardsCountLabel)ve;

                label.RewardsType = rewardsType.GetValueFromBag(bag, cc);
            }
        }

        [SerializeField] private RewardsKeys rewardsType = RewardsKeys.Coins;

        public RewardsKeys RewardsType
        {
            get => rewardsType;
            private set => rewardsType = value;
        }
    }
}