using System;
using Xamarin.Forms;

namespace jtbPromise
{
    public class AppConstants : BindableObject
    {
        public static string AppId
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        return "";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// These Ids are test Ids from https://developers.google.com/admob/android/test-ads
        /// </summary>
        /// <value>The banner identifier.</value>
        public static string BannerId
        {

            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        return "ca-app-pub-9695790043722201/4327342491";    // my code
                                                                            //return "ca-app-pub-3940256099942544/6300978111";      // test code
                    default:
                        return "ca-app-pub-9695790043722201/4327342491";
                }
            }
        }

        /// <summary>
        /// These Ids are test Ids from https://developers.google.com/admob/android/test-ads
        /// </summary>
        /// <value>The Interstitial Ad identifier.</value>
        public static string InterstitialAdId
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:

                        return "ca-app-pub-9695790043722201/8437977894";    //my code
                                                                            //return "ca-app-pub-3940256099942544/1033173712";    //test code
                    default:
                        return "ca-app-pub-9695790043722201/8437977894";
                }
            }
        }

        public static bool ShowAds
        {
            get
            {
                _adCounter++;
                if (_adCounter % 1 == 0)
                {
                    return true;
                }
                return false;
            }
        }

        private static int _adCounter;

    }
}
