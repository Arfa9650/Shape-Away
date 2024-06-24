using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdsInitializer : MonoBehaviour
{

    public void Awake()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            Debug.Log("Ads Initialized");
        });
    }
}
