using Android.Media;
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
    public partial class CertificatePage : ContentPage
    {
        MediaRecorder recorder = null;
        MediaPlayer player = null;

        string mPersonNumber = string.Empty;

        static string fileName = string.Empty;
        static string filePath = string.Empty;



        public CertificatePage(string personNumber)
        {
            mPersonNumber = personNumber;
            fileName = mPersonNumber + ".3gp";
            filePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath + fileName;

            recorder = new MediaRecorder();
            player = new MediaPlayer();

            InitializeComponent();
        }

        public void StartPlayer()
        {
            if (player == null)
            {
                player = new MediaPlayer();
            }
            else
            {
                player.Reset();
            }

            player.SetDataSource(filePath);
            player.Prepare();
            player.Start();  
        }

        private void BtnPlay_Clicked(object sender, EventArgs e)
        {
            StartPlayer();
        }

        private void BtnRecord_Pressed(object sender, EventArgs e)
        {
            try
            {
                // Button 색을 바꾼다.
                BtnRecord.BackgroundColor = Color.Blue;

                if (File.Exists(filePath))
                    File.Delete(filePath);

                //Java.IO.File myFile = new Java.IO.File(filePath);
                //myFile.CreateNewFile();

                if (recorder == null)
                    recorder = new MediaRecorder(); // Initial state.
                else
                    recorder.Reset();

                recorder.SetAudioSource(AudioSource.Mic);
                recorder.SetOutputFormat(OutputFormat.ThreeGpp);
                recorder.SetAudioEncoder(AudioEncoder.AmrNb); // Initialized state.
                recorder.SetOutputFile(filePath); // DataSourceConfigured state.
                recorder.Prepare(); // Prepared state
                recorder.Start(); // Recording state.

            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.StackTrace);
            }
        }

        private void BtnRecord_Released(object sender, EventArgs e)
        {
            try
            {
                BtnRecord.BackgroundColor = Color.Default;
                if (recorder != null)
                {
                    recorder.Stop();
                    recorder.Release();
                    recorder = null;
                }
            }
            catch(Exception ex)
            {
                Console.Out.WriteLine(ex.StackTrace);
            }
        }

        private async void BtnOK_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignPage(mPersonNumber), false);
        }
    }
}