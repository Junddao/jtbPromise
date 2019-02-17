//using iTextSharp.text;
//using iTextSharp.text.pdf;
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

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            // jtbPromise 폴더 서버로 보내고 삭제하기
            var answer = await DisplayAlert("", "저장하시겠습니까?", "Yes", "No");
            if(answer)
            {
                // 서버로 보내기


                // 삭제하기
                DeleteFolrder(MakeDocPage.folderPathforSave);
            }
            else
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