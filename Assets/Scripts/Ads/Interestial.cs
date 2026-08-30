using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class Interestial : MonoBehaviour
{
    bool removeAds = false;

    private float adInterval = 120f; // Time interval for showing ads in seconds
    private float timer = 0f;

<<<<<<< HEAD
    // Editor and development builds always serve Google's test ads, so debugging
    // never generates live impressions. Release builds use the real unit, which is
    // read from AdUnits so the ID is not hardcoded here.
    #if UNITY_EDITOR || DEVELOPMENT_BUILD
    private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
    #elif UNITY_ANDROID
    private string _adUnitId = AdUnits.AndroidInterstitial;
    #elif UNITY_IPHONE
    private string _adUnitId = AdUnits.IosInterstitial;
    #else
    private string _adUnitId = "unused";
=======
    // These ad units are configured to always serve test ads.
    #if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-7418823270776132/4068708106";
    #elif UNITY_IPHONE
     private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
    #else
      private string _adUnitId = "unused";
>>>>>>> e34c3daa72f96a1c593e0133c322be6c654cea16
    #endif

    private InterstitialAd _interstitialAd;

    private void Start()
    {
        if(PlayerPrefs.HasKey("RemoveAds"))
        {
            removeAds = true;
        }

        //LoadInterstitialAd();
        PlayAd();
    }

    private void Update()
    {
        // Increment timer
        timer += Time.deltaTime;

        // Check if it's time to show ad
        if (timer >= adInterval)
        {
            // Reset timer
            timer = 0f;

            // Show ad
            PlayAd();
        }
    }

    void PlayAd()
    {
        if (!removeAds)
        {
            //Only for testing, for realtime load in advance and then show not at the same time
            LoadInterstitialAd();
            ShowInterstitialAd();
        }
    }

    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
            });
    }

    /// <summary>
    /// Shows the interstitial ad.
    /// </summary>
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

}
