using Dropbox.Api.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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


        public static string folderPathforSave = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, "jtbPromiseDownload");
        DirectoryInfo di = new DirectoryInfo(folderPathforSave);
        string selectedFileName = string.Empty;

        public SearchDocPage ()
		{
			InitializeComponent ();
            DocView.ItemsSource = searchFileViewModel.Files;

            CheckFileList();

            CreateFolder(folderPathforSave);
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


        public async Task DownloadFile(string filename)
        {

            if (filename == string.Empty)
            {
                await DisplayAlert("Warning", "파일을 선택하고 다운로드하세요.", "OK");
                return;
            }

            // 폴더 삭제 사용자 선택 항목 추가
            //if (new DirectoryInfo(folderPathforSave).Exists == true)
            //{
            //    var answer = await DisplayAlert("", "Download/jtbPromise/ 폴더에 다른 계약서가 존재합니다. 삭제하고 진행합니까?", "Yes", "No");
            //    if (answer)
            //    {
            //        DeleteFolder(folderPathforSave);
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}

            // 시작전 폴더 삭제후 다시 만들고 진행
            DeleteFolder(folderPathforSave);
            CreateFolder(folderPathforSave);
            try
            {
                string folderName = GetPhoneNumber();

                cDropbox = new CDropBox();
                strAuthenticationURL = cDropbox.GeneratedAuthenticationURL();
                strAccessToken = cDropbox.GenerateAccessToken();

                if (cDropbox.FolderExists("/Dropbox/jtbPromise/" + folderName) == true)
                {
                    //IList<Metadata> IliFiles = await cDropbox.ListFiles("/Dropbox/jtbPromise/" + folderName);
                       
                    await cDropbox.Download("/Dropbox/jtbPromise/" + folderName, filename, di.FullName, filename + ".zip");
                }
            }
                
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        async private void BtnDownload_Clicked(object sender, EventArgs e)
        {
            if (AppConstants.ShowAds)
            {
                await DependencyService.Get<IAdmobInterstitialAds>().Display(AppConstants.InterstitialAdId);
            }
            Debug.WriteLine("Continue button click implementation");

            await DownloadFile(selectedFileName);

            await Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);
        }

        private void DocView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedFileName = (e.SelectedItem as CSearchFile).FileName;
            //await Navigation.PushAsync(new DownloadPage(e.SelectedItem as CSearchFile), false);
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
            if (dInfo.Exists == true)
            {
                dInfo.Delete(true);
            }
        }
    }
}