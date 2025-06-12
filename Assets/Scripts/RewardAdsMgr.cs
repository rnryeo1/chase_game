using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using TMPro;
public class RewardAdsMgr : MonoBehaviour
{
    public static RewardAdsMgr instance;
    //#if UNITY_ANDROID
    //test
    //private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";  
    private string _adUnitId = "ca-app-pub-4803684126798138/2489621873";

    private int lifecount = 5;


    public TextMeshProUGUI textLifecount;
    // #elif UNITY_IPHONE
    //   private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
    // #else
    //   private string _adUnitId = "unused";
    //#endif

    private RewardedAd rewardedAd;
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Stage")
        {
            lifecount = 5;
        }
    }
    public void setlifecount(int _lifecount)
    {
        lifecount = _lifecount;
    }
    public int getlifecount()
    {
        return lifecount;
    }
    void Update()
    {
        textLifecount.text = "start from current state watching ads count : " + lifecount;
    }

    void Awake()
    {
        if (instance == null) //instance가 null. 즉, 시스템상에 존재하고 있지 않을때
        {
            instance = this; //내자신을 instance로 넣어줍니다.
            DontDestroyOnLoad(gameObject); //OnLoad(씬이 로드 되었을때) 자신을 파괴하지 않고 유지
        }
        else
        {
            if (instance != this) //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미
                Destroy(this.gameObject); //둘 이상 존재하면 안되는 객체이니 방금 AWake된 자신을 삭제
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        //MobileAds.Initialize(initStatus => { Debug.LogError("log find 15"); });
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            LoadRewardedAd();
        });

        //LoadRewardedAd();
    }


    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.

        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {

                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {

                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;

                RegisterEventHandlers(rewardedAd);


            });

    }

    public void ShowRewardedAd()
    {

        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";


        if (rewardedAd != null && rewardedAd.CanShowAd())
        {

            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.

                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
                SettingCanvas.instance.watchedRewardAds();
            });
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));

        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            LoadRewardedAd();
        };
    }

    public void ShowAdsButton()
    {

        if (lifecount >= 0)
        {
            StartCoroutine(ShowAdsRoutine());
        }
    }


    private IEnumerator ShowAdsRoutine()
    {
        ShowRewardedAd();
        yield return null;

    }
}

