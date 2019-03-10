using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using System.Collections.Generic;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using System.Threading.Tasks;
using System.IO;

namespace jtbPromise.Droid
{
    [Activity(Label = "jtbPromise", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            // 권한이 없어 권한허용을 물어볼 항목들
            List<string> permissions = new List<string>();

            // 권한이 있는지 확인할 항목들
            List<string> checkPermissions = new List<string>();
            checkPermissions.Add(Manifest.Permission.AccessNetworkState);
            checkPermissions.Add(Manifest.Permission.Internet);
            checkPermissions.Add(Manifest.Permission.WriteSms);
            checkPermissions.Add(Manifest.Permission.BroadcastSms);
            checkPermissions.Add(Manifest.Permission.BroadcastWapPush);
            checkPermissions.Add(Manifest.Permission.ReceiveBootCompleted);
            checkPermissions.Add(Manifest.Permission.ReceiveMms);
            checkPermissions.Add(Manifest.Permission.ReceiveSms);
            checkPermissions.Add(Manifest.Permission.SendSms);
            checkPermissions.Add(Manifest.Permission.WriteExternalStorage);
            checkPermissions.Add(Manifest.Permission.ReadExternalStorage);
            checkPermissions.Add(Manifest.Permission.ReadSms);
            checkPermissions.Add(Manifest.Permission.RecordAudio);  // 오디오 권한 추가
            checkPermissions.Add(Manifest.Permission.ReadPhoneNumbers);
            checkPermissions.Add(Manifest.Permission.ReadPhoneState);

            // 권한이 있는지 확인하고 없다면 체크할 목록에 추가합니다.
            foreach (var checkPermission in checkPermissions)
            {
                if (ContextCompat.CheckSelfPermission(this, checkPermission) != (int)Permission.Granted)
                {
                    permissions.Add(checkPermission);
                }
            }

            // 권한없는 항목에 대해서 추가 허용을 할지 물어보는 팝업을 띄웁니다.
            ActivityCompat.RequestPermissions(this, permissions.ToArray(), 1);


            string fontName = "GodoB.ttf";
            //string fontName = "SourceHanSerifK-Regular.otf";
            string fontPath = Path.Combine(CacheDir.AbsolutePath,  fontName);
            using (var asset = Assets.Open(fontName))
            using (var dest = File.Open(fontPath, FileMode.Create))
            {
                asset.CopyTo(dest);
            }
            string customFontPath = fontPath;
        }


        public override void OnBackPressed()
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);

            builder.SetPositiveButton("확인", (senderAlert, args) => {
                Finish();
            });

            builder.SetNegativeButton("취소", (senderAlert, args) => {
                return;
            });

            Android.App.AlertDialog alterDialog = builder.Create();
            alterDialog.SetTitle("알림");
            alterDialog.SetMessage("프로그램을 종료 하시겠습니까?");
            alterDialog.Show();
        }
    }
}