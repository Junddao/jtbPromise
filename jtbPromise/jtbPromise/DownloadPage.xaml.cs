using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dropbox.Api.Files;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace jtbPromise
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DownloadPage : ContentPage
	{

        CDropBox cDropbox = new CDropBox();
        public static string folderPathforSave = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, "jtbPromise");
        DirectoryInfo di = new DirectoryInfo(folderPathforSave);

        public DownloadPage(CSearchFile cSearchFileTyepSelectedFile)
		{
			InitializeComponent ();

            BindingContext = new DownloadPageViewModel();

            string FileName = cSearchFileTyepSelectedFile.FileName;

            //DownloadFile(FileName);

        }

        public async Task DownloadFile(string filename)
        {
            try
            {
                string folderName = GetPhoneNumber();

                if (cDropbox.FolderExists("/Dropbox/jtbPromise/" + folderName) == true)
                {
                    IList<Metadata> IliFiles = await cDropbox.ListFiles("/Dropbox/jtbPromise/" + folderName);
                    List<string> liFiles = new List<string>();

                    cDropbox.Download("/Dropbox/jtbPromise/" + folderName, filename, di.FullName, filename);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public string GetPhoneNumber()
        {
            string PhoneNumber = DependencyService.Get<PhoneNumberInterface>().GetPhoneNumber();
            return PhoneNumber;
        }

        private async void BtDownload_Clicked(object sender, EventArgs e)
        {
            if (AppConstants.ShowAds)
            {
                await DependencyService.Get<IAdmobInterstitialAds>().Display(AppConstants.InterstitialAdId);
            }
            Debug.WriteLine("Continue button click implementation");

            //DownloadFile();
        }
    }
}