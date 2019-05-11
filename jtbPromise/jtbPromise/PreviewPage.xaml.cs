using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;
using SkiaSharp.Views.Forms;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace jtbPromise
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreviewPage : ContentPage
    {

        //public PreviewPage()
        //{
        //    InitializeComponent();
        //}
        string mTitle = string.Empty;
        string mContent = string.Empty;
        string mFirstName = string.Empty;
        string mSecondName = string.Empty;

        string customFontPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "..", "cache", "GodoB.ttf");
        //string customFontPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "..", "cache", "SourceHanSerifK-Regular.otf");

        public PreviewPage(string title, string content, string firstName, string secondName)
        {
            InitializeComponent();

            try
            {
                mTitle = title;
                mContent = content;
                mFirstName = firstName;
                mSecondName = secondName;
            }
            catch(Exception)
            {
                DisplayAlert("Warning", "서명후 확인하세요.", "OK");
            }
        }

        async void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            //var tf = SKTextEncoding.Utf32;
            // Create an SKPaint object to display the text

            SKPaint textPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Black,
                StrokeWidth = 2,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
                TextEncoding = SKTextEncoding.Utf32
            };

            using (var tf = SKTypeface.FromFile(customFontPath))
            {
                textPaint.Typeface = tf;
            }

            try
            {
                #region Title Draw
                // Adjust TextSize property so text is 95% of screen width
                float textWidthTitle = textPaint.MeasureText(mTitle);
                //textPaint.TextSize = 0.3f * info.Width * textPaint.TextSize / textWidthTitle;
                textPaint.TextSize = 50f;

                // Find the text bounds
                SKRect textBoundsTitle = new SKRect();
                textPaint.MeasureText(mTitle, ref textBoundsTitle);

                // Calculate offsets to center the text on the screen
                float xTextTitle = info.Width / 2 - textBoundsTitle.MidX;
                float yTextTitle = textBoundsTitle.Height + 50;

                // And draw the text
                canvas.DrawText(mTitle, xTextTitle, yTextTitle, textPaint);

                #endregion


                #region Content Draw
                //갑, 을 이름 Contents 에 추가
                mContent = txt_split(mContent, 20);
                string NameInContents = "(갑)" + mFirstName + "은/는 (을)" + mSecondName + "에게 \n";
                mContent = NameInContents + mContent;
                

                float textWidthContent = textPaint.MeasureText(mContent);
                string[] arrContents = mContent.Split('\n');
                //textPaint.TextSize = textPaint.TextSize * 0.45f;
                textPaint.TextSize = 32f;

                for (int i = 0; i < arrContents.Count(); i++)
                {
                    SKRect textBoundsContent = new SKRect();
                    textPaint.MeasureText(mContent, ref textBoundsContent);

                    float xTextContent = 10;
                    float yTextContent = (textBoundsTitle.Height * 3 + 50) + (textBoundsContent.Height + 20) * (i + 1);

                    canvas.DrawText(arrContents[i], xTextContent, yTextContent, textPaint);
                }
                #endregion


                #region First Name Draw /  Second Name Draw
                float textWidthFirstName = textPaint.MeasureText(mFirstName);

                SKRect textBoundsFirstName = new SKRect();
                textPaint.MeasureText(mFirstName, ref textBoundsFirstName);

                float xPositionTextFirstName = info.Width - (textBoundsFirstName.Width * 2) - 50;
                float yPositionTextFirstName = info.Height - (textBoundsFirstName.Height * 2) - 100;

                float textWidthSecondName = textPaint.MeasureText(mSecondName);

                SKRect textBoundsSecondName = new SKRect();
                textPaint.MeasureText(mSecondName, ref textBoundsSecondName);

                float xPositionTextSecondName = info.Width - (textBoundsSecondName.Width * 2) - 50;
                float yPositionTextSecondName = info.Height - textBoundsSecondName.Height - 50;

                float xPosition = 0f;
                if (xPositionTextFirstName < xPositionTextSecondName) xPosition = xPositionTextFirstName;
                else xPosition = xPositionTextSecondName;

                canvas.DrawText(mFirstName, xPosition, yPositionTextFirstName, textPaint);
                canvas.DrawText(mSecondName, xPosition, yPositionTextSecondName, textPaint);
                #endregion

                int rectWidth = (int)textBoundsFirstName.Height;
                int rectHeight = (int)textBoundsFirstName.Height;

                string firstPersonImageFile = System.IO.Path.Combine(MakeDocPage.folderPathforSave,"first.png");
                string secondPersonImageFile = System.IO.Path.Combine(MakeDocPage.folderPathforSave, "second.png");

                var bitmapFirstPersonSign = SKBitmap.Decode(firstPersonImageFile);
                double resize = Convert.ToDouble(bitmapFirstPersonSign.Width) / Convert.ToDouble(bitmapFirstPersonSign.Height);
                int width = Convert.ToInt16(textBoundsFirstName.Height * resize) + 50;
                int height = (int)textBoundsFirstName.Height + 50;
         
                var firstPersonResizedImage = bitmapFirstPersonSign.Resize(new SKImageInfo(width, height), SKBitmapResizeMethod.Lanczos3);
                
                canvas.DrawBitmap(firstPersonResizedImage, info.Width - rectWidth - 50, yPositionTextFirstName - rectHeight - 25);

                var bitmapSecondPersonSign = SKBitmap.Decode(secondPersonImageFile);
                var secondPersonResizedImage = bitmapSecondPersonSign.Resize(new SKImageInfo(width, height), SKBitmapResizeMethod.Lanczos3);

                canvas.DrawBitmap(secondPersonResizedImage, info.Width - rectWidth - 50, yPositionTextSecondName - rectHeight - 25);

                saveImage(surface);
            }
            catch(Exception)
            {
                await DisplayAlert("Alert", "필수 항목을 모두 입력하세요.", "OK");
            }
        }

        public string txt_split(string s_str, int str_cnt)
        {
            string r_str = "";

            string[] s_split = s_str.Split('\n');

            for (int i = 0; i < s_split.Length; i++)
            {
                if (s_split[i].Length > str_cnt)
                {
                    if (i <= 0)
                    {
                        r_str += txt_split(s_str.Insert(str_cnt, "\n"), str_cnt);

                    }
                    else
                    {
                        r_str += txt_split(s_split[i].Insert(str_cnt, "\n"), str_cnt);
                    }

                }
                else
                {
                    r_str += s_split[i] + "\n";
                }

            }
            return r_str;
        }

        async void saveImage(SKSurface _surface)
        {

            string filePath = System.IO.Path.Combine(MakeDocPage.folderPathforSave, mFirstName + "_" + mSecondName + ".png");
            try
            {
                var surface = SKSurface.Create(
                (int)canvasView.CanvasSize.Width,
                (int)canvasView.CanvasSize.Height,
                SKImageInfo.PlatformColorType,
                SKAlphaType.Premul);
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.White);

                canvas.DrawSurface(_surface, 0, 0);

                using (var pngData = surface.Snapshot().Encode(SKEncodedImageFormat.Png, 100))
                {
                    using (var stream = File.OpenWrite(filePath))
                    {
                        pngData.SaveTo(stream);
                    }
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Alert", "예상치 못한 문제가 발생했습니다.", "OK");
            }
        }


        async void BtnOK_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}