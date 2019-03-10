using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


        ObservableCollection<CFiles> files = new ObservableCollection<CFiles>();

        public SearchDocPage ()
		{
			InitializeComponent ();
		}

        public async void CheckFileList()
        {
            
            DocView.ItemsSource = files;

            string folderName = GetPhoneNumber();

            cDropbox = new CDropBox();
            strAuthenticationURL = cDropbox.GeneratedAuthenticationURL();
            strAccessToken = cDropbox.GenerateAccessToken();

            if(cDropbox.FolderExists("/Dropbox/jtbPromise/" + folderName) == false)
            {
                //List<string> liFiles =  await cDropbox.ListFiles("/Dropbox/jtbPromise/" + folderName);
            }
        }


        public string GetPhoneNumber()
        {

            string PhoneNumber = DependencyService.Get<PhoneNumberInterface>().GetPhoneNumber();
            return PhoneNumber;
        }

    }

    public class CFiles
    {
        public string DisplayFileName { get; set; }
    }
}