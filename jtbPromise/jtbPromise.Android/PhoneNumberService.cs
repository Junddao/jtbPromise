using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

[assembly: Dependency(typeof(jtbPromise.Droid.PhoneNumberService))]
namespace jtbPromise.Droid
{
    public class PhoneNumberService : PhoneNumberInterface
    {
        public string GetPhoneNumber()
        {
            var tMgr = (TelephonyManager)Forms.Context.ApplicationContext.GetSystemService(Android.Content.Context.TelephonyService);
            return tMgr.Line1Number;
        }
    }
}