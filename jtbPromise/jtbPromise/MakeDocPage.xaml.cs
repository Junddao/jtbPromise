//using iTextSharp.text;
//using iTextSharp.text.pdf;
using Android.Telephony;
using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Stone;
using PCLStorage;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace jtbPromise
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MakeDocPage : ContentPage
    {
        public static string folderPathforSave = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, "jtbPromise");
        public static string _accessKey = string.Empty;
        DirectoryInfo di = new DirectoryInfo(folderPathforSave);

        CDropBox cDropbox;
        string strAuthenticationURL = string.Empty;
        string strAccessToken = string.Empty;

        public MakeDocPage()
        {
            InitializeComponent();

            CreateFolder(MakeDocPage.folderPathforSave);

           
        }

        async void BtnFirstPersonCert_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CertificatePage("first"), false);
        }

        async void BtnSecondPersonCert_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CertificatePage("second"), false);
        }

        async void BtnPreview_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PreviewPage(edtTitle.Text, edtContent.Text, edtFirstPersonName.Text, edtSecondPersonName.Text), false);
        }


        public async Task<string> PCLReadFile(string path)
        {
            IFile file = await FileSystem.Current.GetFileFromPathAsync(path);
            return file.Path;
        }

        public string GetPhoneNumber()
        {

            string PhoneNumber = DependencyService.Get<PhoneNumberInterface>().GetPhoneNumber();
            return PhoneNumber;
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            // jtbPromise 폴더 서버로 보내고 삭제하기
            var answer = await DisplayAlert("", "저장하시겠습니까?", "Yes", "No");
            try
            {
                if (answer)
                {


                    string folderName = GetPhoneNumber();

                    // 서버로 보내기
                    cDropbox = new CDropBox();
                    strAuthenticationURL = cDropbox.GeneratedAuthenticationURL();
                    strAccessToken = cDropbox.GenerateAccessToken();

                    if (cDropbox.FolderExists("/Dropbox/jtbPromise/" + folderName) == true)
                    {
                        cDropbox.Delete("/Dropbox/jtbPromise/" + folderName);
                    }
                    cDropbox.CreateFolder("/Dropbox/jtbPromise/" + folderName);



                    List<string> liFiles = new List<string>();

                    
                    foreach (var s in di.GetFiles())
                    {
                        cDropbox.Upload("/Dropbox/jtbPromise/" + folderName, s.Name, s.FullName);
                    }
                    
                    // 삭제하기
                    DeleteFolrder(MakeDocPage.folderPathforSave);

                    await Navigation.PushAsync(new MainPage());
                    Navigation.RemovePage(this);
                }
                else
                {

                }
            }
            catch(Exception)
            {

            }
            
        }


        async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            // jtbPromise 폴더 삭제하기
            DeleteFolrder(MakeDocPage.folderPathforSave);

            await Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);
        }

        private void CreateFolder(string dirPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(dirPath);
            if (dInfo.Exists == false)
            {
                dInfo.Create();
            }
        }

        private void DeleteFolrder(string dirPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(dirPath);
            if(dInfo.Exists == true)
            {
                dInfo.Delete(true);
            }
        }
    }
}