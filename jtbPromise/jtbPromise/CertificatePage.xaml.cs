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

        static string filePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath + "test.3gp";

        public CertificatePage()
        {
            InitializeComponent();

            recorder = new MediaRecorder();
            player = new MediaPlayer();
        }

        private void BtnRecord_Clicked(object sender, EventArgs e)
        {
            StartRecorder();
        }

        private void BtnStop_Clicked(object sender, EventArgs e)
        {
            StopRecorder();
        }

        public void StartRecorder()
        {
            try
            {
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

        public  void StopRecorder()
        {
            if (recorder != null)
            {
                recorder.Stop();
                recorder.Release();
                recorder = null;
            }
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
    }
}