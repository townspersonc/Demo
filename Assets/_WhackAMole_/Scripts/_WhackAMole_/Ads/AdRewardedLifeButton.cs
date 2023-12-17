namespace WhackAMole
{
    public class AdRewardedLifeButton : RewardedAdsButton
    {
        private string c_androidUnitID = "Rewarded_Android";
#if UNITY_IOS
        private string c_iOSUnitID = "Rewarded_iOS";
#endif

        protected override string AdUnitID
        {
            get
            {
#if UNITY_IOS
                return c_iOSUnitID;
#endif
                return c_androidUnitID;
            }
        }

        protected override void GrantReward()
        {
            GameManager.Instance.LevelManager.GrantExtraLife();
        }
    }
}
