
using App.Common.Tools;
using App.Core.Common.Services;
using App.Core.Storage;
using Zenject;
using ProfileSaveService = App.Core.Storage.ProfileService.ProfileSaveService;

namespace App.Core.Meta
{
    public class RewardsService : AppService
    {
        [Inject]
        private ProfileSaveService profileSaveService;

        private RewardSaveComponent rewardSaveComponent;

        public void AddCoins(int count)
        {
            if (count < 1)
            {
                Logger.LogError($"[RewardService] => AddCoins: count = {count}");

                return;
            }

            rewardSaveComponent.Coins += count;
        }

        public int GetCoins() => rewardSaveComponent.Coins;

        protected override void FinishInitialize()
        {
            base.FinishInitialize();

            LoadSave();
        }

        private void LoadSave()
        {
            rewardSaveComponent = profileSaveService.GetSaveComponent<RewardSaveComponent>();
        }
    }
}