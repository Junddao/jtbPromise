using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace jtbPromise
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchOptionPage : ContentPage
	{
        string phoneNumber;

        public SearchOptionPage()
        {
            InitializeComponent();

            etrYourNumber.Text = GetPhoneNumber(); ;
        }


        public string GetPhoneNumber()
        {
            string PhoneNumber = DependencyService.Get<PhoneNumberInterface>().GetPhoneNumber();
            return PhoneNumber;
        }


        private async void BtnSearch_Clicked(object sender, EventArgs e)
        {
            phoneNumber = etrYourNumber.Text;
            await Navigation.PushAsync(new SearchDocPage(phoneNumber), false);
        }
    }
}