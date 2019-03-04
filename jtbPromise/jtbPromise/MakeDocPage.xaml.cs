//using iTextSharp.text;
//using iTextSharp.text.pdf;
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

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            // jtbPromise 폴더 서버로 보내고 삭제하기
            var answer = await DisplayAlert("", "저장하시겠습니까?", "Yes", "No");
            try
            {
                if (answer)
                {
                    // 서버로 보내기

                    
                    cDropbox = new CDropBox();
                    strAuthenticationURL = cDropbox.GeneratedAuthenticationURL();
                    strAccessToken = cDropbox.GenerateAccessToken();

                    if (cDropbox.FolderExists("/Dropbox/jtbPromise") == false)
                    {
                        cDropbox.CreateFolder("/Dropbox/jtbPromise");
                    }

                    List<string> liFiles = new List<string>();

                    
                    foreach (var s in di.GetFiles())
                    {
                        cDropbox.Upload("/Dropbox/jtbPromise", s.Name, s.FullName);
                    }
                    

                    // 삭제하기
                    DeleteFolrder(MakeDocPage.folderPathforSave);
                }
                else
                {

                }
            }
            catch(Exception)
            {

            }
            
        }


        //async public void ListingFileInDropBox()
        //{
        //    using (DropboxClient client = new DropboxClient(_accessKey))
        //    {
        //        try
        //        {
        //            bool more = true;
        //            var list = await client.Files.ListFolderAsync("");
        //            while (more)
        //            {
        //                foreach (var item in list.Entries.Where(i => i.IsFile))
        //                {
        //                    // Process the file
        //                }
        //                more = list.HasMore;
        //                if (more)
        //                {
        //                    list = await client.Files.ListFolderContinueAsync(list.Cursor);
        //                }
        //            }
        //        }
        //        catch
        //        {
        //            // Process the exception
        //        }
        //    }
        //}

        //public async Task DownloadFileInDropbox(string filename)
        //{
        //    using (DropboxClient client = new DropboxClient(_accessKey))
        //    {
        //        IDownloadResponse<FileMetadata> resp =
        //            await client.Files.DownloadAsync(filename);
        //        Stream ds = await resp.GetContentAsStreamAsync();
        //        await ds.CopyToAsync(s);
        //        ds.Dispose();
        //    }
        //}

        //public async Task UploadFileInDropbox(string filename)
        //{
        //    using (DropboxClient client = new DropboxClient(_accessKey))
        //    {
        //        var UploadResponse = await client.Files.UploadAsync(filename);

        //    }
        //}

        //public bool Upload(string UploadfolderPath, string UploadfileName, string SourceFilePath)
        //{
        //    try
        //    {
        //        using (var stream = new MemoryStream(File.ReadAllBytes(SourceFilePath)))
        //        {
        //            var response = DBClient.Files.UploadAsync(UploadfolderPath + "/" + UploadfileName, WriteMode.Overwrite.Instance, body: stream);
        //            var rest = response.Result; //Added to wait for the result from Async method  
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}

        //public async Task DeleteFileInDropbox(string filename)
        //{
        //    using (DropboxClient client = new DropboxClient(_accessKey))
        //    {
        //        await client.Files.DeleteAsync(filename);
        //    }
        //}

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