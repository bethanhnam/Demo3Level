using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using com.adjust.sdk;

public class FirebaseEvent : MonoBehaviour
{
    private static string nameSuperAdsimpression = "super_impression_ads";

    public static void LogEventAdsImpresstion(string platform, string adsUnitName, string adsPlacment, string adFormat, double adRev, string adNetwork)
    {
        if (RemoteConfigController.instance.isFetching)
        {
            return;
        }
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression,
            new Parameter[] {
                new Parameter(FirebaseAnalytics.ParameterAdPlatform,platform),
                new Parameter(FirebaseAnalytics.ParameterAdUnitName,adsUnitName),
                new Parameter(FirebaseAnalytics.ParameterAdFormat,adFormat),
                new Parameter(FirebaseAnalytics.ParameterValue,adRev* RemoteConfigController.instance.Value_impress_ads),
                new Parameter(FirebaseAnalytics.ParameterCurrency,"USD"),
                new Parameter(FirebaseAnalytics.ParameterAdSource,adNetwork)
            }
            );

        FirebaseAnalytics.LogEvent(nameSuperAdsimpression,
         new Parameter[] {
                new Parameter(FirebaseAnalytics.ParameterAdPlatform,platform),
                new Parameter(FirebaseAnalytics.ParameterAdUnitName,adsUnitName),
                new Parameter(FirebaseAnalytics.ParameterAdFormat,adFormat),
                new Parameter(FirebaseAnalytics.ParameterValue,adRev* RemoteConfigController.instance.Value_super_impression_ads),
                new Parameter(FirebaseAnalytics.ParameterCurrency,"USD"),
                new Parameter(FirebaseAnalytics.ParameterAdSource,adNetwork)
         }
         );

        AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(platform);
        adjustAdRevenue.setRevenue(adRev, "USD");
        adjustAdRevenue.setAdRevenueNetwork(adNetwork);
        adjustAdRevenue.setAdRevenueUnit(adsUnitName);
        adjustAdRevenue.setAdRevenuePlacement(adsPlacment);
        Adjust.trackAdRevenue(adjustAdRevenue);
    }
}
