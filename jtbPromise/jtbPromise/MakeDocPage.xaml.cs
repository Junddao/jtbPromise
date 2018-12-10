//using iTextSharp.text;
//using iTextSharp.text.pdf;
using PCLStorage;
using SkiaSharp;
using SkiaSharp.Views.Forms;
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
	public partial class MakeDocPage : ContentPage
	{

        static string folderPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;

        public MakeDocPage ()
		{
			InitializeComponent ();
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
            //Genarate();
        }

        //private async void Genarate()
        //{
        //    await PCLGenaratePdf(folderPath);
        //}

        //public async Task PCLGenaratePdf(string path)
        //{
        //    IFolder rootFolder = await FileSystem.Current.GetFolderFromPathAsync(path);
        //    IFile file = await rootFolder.CreateFileAsync("contract.pdf", CreationCollisionOption.ReplaceExisting);

        //    using (var fs = await file.OpenAsync(FileAccess.ReadAndWrite))
        //    {
        //        var document = new Document(PageSize.A4, 25, 25, 30, 30);
        //        PdfWriter writer = PdfWriter.GetInstance(document, fs);
        //        document.Open();
        //        document.Add(new Paragraph(lbTitle.Text));


        //        PdfPTable table1 = new PdfPTable(4);
        //        table1.DefaultCell.Border = 0;
        //        table1.WidthPercentage = 80;


        //        PdfPCell cellTitle = new PdfPCell();
        //        //cellTitle.Colspan = 1;
        //        cellTitle.AddElement(new Paragraph(edtTitle.Text));


        //        PdfPCell cellContent = new PdfPCell();
        //        cellContent.AddElement(new Paragraph(edtContent.Text));

        //        PdfPCell cellFirstPerson = new PdfPCell();
        //        cellFirstPerson.AddElement(new Paragraph(edtFirstPersonName.Text));


        //        PdfPCell cellSecondPerson = new PdfPCell();
        //        cellSecondPerson.AddElement(new Paragraph(edtSecondPersonName.Text));

        //        table1.AddCell(cellTitle);
        //        table1.AddCell(cellContent);
        //        table1.AddCell(cellFirstPerson);
        //        table1.AddCell(cellSecondPerson);

        //        document.Add(table1);

        //        //iTextSharp.text.Image DocImage = iTextSharp.text.Image.GetInstance(image.GetBuffer());
        //        //DocImage.ScalePercent(100f);
        //        document.Close();
        //        writer.Close();
        //    }
        //}

        public async Task<string> PCLReadFile(string path)
        {
            IFile file = await FileSystem.Current.GetFileFromPathAsync(path);
            return file.Path;
        }
    }
    
}