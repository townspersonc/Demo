using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using DG.Tweening;
using WhackAMole;

public abstract class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private const float c_retryLoadSecs = 5f;
    
    [SerializeField] private Button _showAdButton;
    [SerializeField] private bool _logging;

    protected abstract string AdUnitID { get; }

    private void OnEnable()
    {
        _showAdButton.gameObject.Disable();
        LoadAd();
    }

    public void U_Click()
    {
        _showAdButton.gameObject.Disable();
        Advertisement.Show(AdUnitID, this);
    }

    private void LoadAd()
    {
        if (Advertisement.isInitialized)
        {
            Log("Loading Ad: " + AdUnitID);
            Advertisement.Load(AdUnitID, this);
        }
    }

    private void RetryAdLoad()
    {
        if (LevelManager.GameOn) LoadAd();
    }

    protected abstract void GrantReward();

    void IUnityAdsLoadListener.OnUnityAdsAdLoaded(string adUnitId)
    {
        Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(AdUnitID))
        {
            _showAdButton.gameObject.Enable();
        }
    }
    void IUnityAdsLoadListener.OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Log($"Ad Load Failed. {error}");

        DOVirtual.DelayedCall(c_retryLoadSecs, RetryAdLoad);
    }

    void IUnityAdsShowListener.OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(AdUnitID) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Log("Unity Ads Rewarded Ad Completed");
            GrantReward();
        }

        LoadAd();
    }
    void IUnityAdsShowListener.OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }
    void IUnityAdsShowListener.OnUnityAdsShowStart(string placementId) { }
    void IUnityAdsShowListener.OnUnityAdsShowClick(string placementId) { }

    private void Log(string message)
    {
        if (_logging)
        {
            Debug.Log("Ads: " + message);
        }
    }
}