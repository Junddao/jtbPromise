using Dropbox.Api.Files;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace jtbPromise
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchDocPage : ContentPage
	{
        CDropBox cDropbox;
        string strAuthenticationURL = string.Empty;
        string strAccessToken = string.Empty;

        CSearchFileViewModel searchFileViewModel = new CSearchFileViewModel();


        public SearchDocPage ()
		{
			InitializeComponent ();
            DocView.ItemsSource = searchFileViewModel.Files;

            CheckFileList();
        }

        public async void CheckFileList()
        {

            string folderName = GetPhoneNumber();

            cDropbox = new CDropBox();
            strAuthenticationURL = cDropbox.GeneratedAuthenticationURL();
            strAccessToken = cDropbox.GenerateAccessToken();

            if(cDropbox.FolderExists("/Dropbox/jtbPromise/" + folderName) == true)
            {
                IList<Metadata> IliFiles = await cDropbox.ListFiles("/Dropbox/jtbPromise/" + folderName);
                List<string> liFiles = new List<string>();
                foreach(var file in IliFiles)
                {
                    if ((file.Name.Contains("first") || file.Name.Contains("second")) == false)
                    {
                        searchFileViewModel.Files.Add(new CSearchFile()
                        {
                            FileName = file.Name
                        });
                    }
                }
            }
            else
            {
                searchFileViewModel.Files.Add(new CSearchFile()
                {
                    FileName = "No Itemes."
                });
            }
        }

        public string GetPhoneNumber()
        {
            string PhoneNumber = DependencyService.Get<PhoneNumberInterface>().GetPhoneNumber();
            return PhoneNumber;
        }


        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            if (AppConstants.ShowAds)
            {
                await DependencyService.Get<IAdmobInterstitialAds>().Display(AppConstants.InterstitialAdId);
            }
            Debug.WriteLine("Continue button click implementation");
        }
    }
}