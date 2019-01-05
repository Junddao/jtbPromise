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
        public static string folderPathforSave = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, "jtbPromise", DateTime.Now.ToString("yyyyMMddhhmmss"));

        public MakeDocPage ()
		{
            InitializeComponent();

            CreateFolder();
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

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            
        }

        async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);

        }

        private void CreateFolder()
        {
            DirectoryInfo dInfo = new DirectoryInfo(MakeDocPage.folderPathforSave);
            if (dInfo.Exists == false)
            {
                dInfo.Create();
            }
            else
            {
                folderPathforSave = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, "jtbPromise", DateTime.Now.ToString("yyyyMMddhhmmss"));
                dInfo = new DirectoryInfo(MakeDocPage.folderPathforSave);
                dInfo.Create();
            }
        }
    }
}