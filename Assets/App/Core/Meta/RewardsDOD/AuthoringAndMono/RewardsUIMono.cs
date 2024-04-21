using System.Collections.Generic;
using System.Linq;
using App.AppUI.Rewards;
using UnityEngine;
using UnityEngine.UIElements;

namespace App.Core.Meta.RewardsDOD
{
    public class RewardsUIMono : MonoBehaviour
    {
        private List<UIRewardsCountLabel> rewardsLabels;

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            rewardsLabels = root.Query<UIRewardsCountLabel>().ToList();
        }

        public void UpdateRewardsValue(int value, RewardsKeys updatedRewardKey)
        {
            if (rewardsLabels == null || rewardsLabels.Count < 0) return;
            
            foreach (var rewardsLabel in rewardsLabels
                         .Where(rewardsLabel => rewardsLabel.RewardsType == updatedRewardKey))
            {
                rewardsLabel.text = value.ToString();
            }
        }
    }
}
