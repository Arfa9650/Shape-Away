/// <summary>
/// Live AdMob ad unit IDs, used only by release builds. Editor and development
/// builds ignore these and serve Google's test units instead - see Banner.cs and
/// Interestial.cs.
/// </summary>
public static class AdUnits
{
    public const string AndroidBanner = "ca-app-pub-7418823270776132/8450806245";

    public const string AndroidInterstitial = "ca-app-pub-7418823270776132/4068708106";

    // iOS is not a configured build target yet; these are Google's test units.
    public const string IosBanner = "ca-app-pub-3940256099942544/2934735716";

    public const string IosInterstitial = "ca-app-pub-3940256099942544/4411468910";
}
