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


using System.IO.Compression;
using Rg.Plugins.Popup.Extensions;

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

            BindingContext = new MakeDocPageViewModel();

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
            await Navigation.PushAsync(new PreviewPage(lbTitle.Text, edtContent.Text, edtFirstPersonName.Text, edtSecondPersonName.Text), false);
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
            if (edtFirstPersonName.Text == string.Empty || edtSecondPersonName.Text == string.Empty)
            {
                await DisplayAlert("", "이름 작성 후 저장하세요.", "OK");
                return;
            }
            if (File.Exists(folderPathforSave + "/" + "first.3gp") == false)
            {
                await DisplayAlert("", "(갑)의 녹음파일이 없습니다. 다시 녹음후 진행하세요.", "OK");
                return;
            }
            if (File.Exists(folderPathforSave + "/" + "second.3gp") == false)
            {
                await DisplayAlert("", "(을)의 녹음파일이 없습니다. 다시 녹음후 진행하세요.", "OK");
                return;
            }
            if (File.Exists(folderPathforSave + "/" + edtFirstPersonName.Text + "_" + edtSecondPersonName.Text + ".png") == false)
            {
                await DisplayAlert("", "미리보기 후 저장하세요.", "OK");
                return;
            }

            // jtbPromise 폴더 서버로 보내고 삭제하기
            var answer = await DisplayAlert("", "저장하시겠습니까?", "Yes", "No");
            try
            {
                if (answer)
                {
                    //******** popup창 열기 *******//
                    var popupPage = new PopupView();
                    await Navigation.PushPopupAsync(popupPage);
                    await Task.Delay(2000);
                    

                    string folderName = GetPhoneNumber();
                    string zipFileName = edtFirstPersonName.Text + "_" + edtSecondPersonName.Text;

                    // 보내기 전에 압축한번 하고
                    string[] arrFileNames = Directory.GetFiles(folderPathforSave);
                    QuickZip(arrFileNames, folderPathforSave + "/" + zipFileName);

                    // 서버로 보내기
                    cDropbox = new CDropBox();
                    strAuthenticationURL = cDropbox.GeneratedAuthenticationURL();
                    strAccessToken = cDropbox.GenerateAccessToken();

                    if (cDropbox.FolderExists("/Dropbox/jtbPromise/" + folderName) == true)
                    {
                        cDropbox.Delete("/Dropbox/jtbPromise/" + folderName);
                    }
                    cDropbox.CreateFolder("/Dropbox/jtbPromise/" + folderName);
                    if(cDropbox.FolderExists("/Dropbox/jtbPromiseBackup/" + folderName) == false)
                    {
                        cDropbox.CreateFolder("/Dropbox/jtbPromiseBackup/" + folderName);
                    }
                    

                    List<string> liFiles = new List<string>();

                    await cDropbox.Upload("/Dropbox/jtbPromise/" + folderName, zipFileName, folderPathforSave + "/" + zipFileName);
                    await cDropbox.Upload("/Dropbox/jtbPromiseBackup/" + folderName, zipFileName, folderPathforSave + "/" + zipFileName);
                    //foreach (var s in di.GetFiles())
                    //{
                    //    await cDropbox.Upload("/Dropbox/jtbPromise/" + folderName, s.Name, s.FullName);
                    //}

                    // 삭제하기
                    DeleteFolder(MakeDocPage.folderPathforSave);

                    //******** popup창 닫기 *******//
                    await Navigation.RemovePopupPageAsync(popupPage);

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
            DeleteFolder(MakeDocPage.folderPathforSave);

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

        private void DeleteFolder(string dirPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(dirPath);
            if(dInfo.Exists == true)
            {
                dInfo.Delete(true);
            }
        }

        public bool QuickZip(string[] filesToZip, string destinationZipFullPath)
        {
            try
            {
                // Delete existing zip file if exists
                if (File.Exists(destinationZipFullPath))
                    File.Delete(destinationZipFullPath);

                using (ZipArchive zip = ZipFile.Open(destinationZipFullPath, ZipArchiveMode.Create))
                {
                    foreach (var file in filesToZip)
                    {
                        zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
                    }
                }

                return File.Exists(destinationZipFullPath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return false;
            }
        }
    }
}