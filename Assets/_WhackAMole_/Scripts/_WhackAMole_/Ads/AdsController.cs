using UnityEngine;
using UnityEngine.Advertisements;

namespace WhackAMole
{
    public class AdsController : MonoBehaviour, IUnityAdsInitializationListener
    {
        [SerializeField] private bool _logging;

        private const string c_androidGameId = "5501415";
        private const string c_iOSGameId = "5501414";
        private const bool c_testMode = true;

        private void Awake()
        {
            InitializeAds();
        }

        public void InitializeAds()
        {
            string _gameId = c_androidGameId;

#if UNITY_IOS
            _gameId = _iOSGameId;
#endif

            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(_gameId, c_testMode, this);
            }
        }

        public void OnInitializationComplete()
        {
            Log("Unity Ads initialization complete.");
        }
        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }

        private void Log(string message)
        {
            if (_logging)
            {
                Debug.Log("Ads: " + message);
            }
        }
    }
}