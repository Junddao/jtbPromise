using jtbPromise.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xam.FormsPlugin.Abstractions;
using Xamarin.Forms;


namespace jtbPromise
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        async void BtnCreateOffLine_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MakeDocPage(), false);
            Navigation.RemovePage(this);
        }

        async void BtnSearch_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SearchDocPage(), false);
        }

        private void BtnCreateOnLine_Clicked(object sender, EventArgs e)
        {
            //online 은 나중에..
        }

        async void ShowInterstitialAdsButton_Clicked(object sender, EventArgs e)
        {
            if (AppConstants.ShowAds)
            {
                await DependencyService.Get<IAdmobInterstitialAds>().Display(AppConstants.InterstitialAdId);
            }
            Debug.WriteLine("Continue button click implementation");
        }
    }
}
