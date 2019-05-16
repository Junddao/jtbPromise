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

            AdmobControl admobControl = new AdmobControl()
            {
                AdUnitId = AppConstants.BannerId
            };
            Label adLabel = new Label() { Text = "Ads will be displayed here!" };

            Button showInterstitialAdsButton = new Button();
            showInterstitialAdsButton.Clicked += ShowInterstitialAdsButton_Clicked;
            showInterstitialAdsButton.Text = "Show Interstitial Ads";

            Content = new StackLayout()
            {
                Children = { adLabel, admobControl, showInterstitialAdsButton }
            };

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
            //playAds();
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
