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

            mTitle = title;
            mContent = content;
            mFirstName = firstName;
            mSecondName = secondName;
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
                textPaint.TextSize = 0.3f * info.Width * textPaint.TextSize / textWidthTitle;

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
                float textWidthContent = textPaint.MeasureText(mContent);
                textPaint.TextSize = textPaint.TextSize * 0.45f;

                SKRect textBoundsContent = new SKRect();
                textPaint.MeasureText(mContent, ref textBoundsContent);

                float xTextContent = info.Width / 2 - textBoundsContent.MidX;
                float yTextContent = (textBoundsTitle.Height * 2) + textBoundsContent.Height + 70;



                canvas.DrawText(mContent, xTextContent, yTextContent, textPaint);
                #endregion

                #region First Name Draw
                float textWidthFirstName = textPaint.MeasureText(mFirstName);

                SKRect textBoundsFirstName = new SKRect();
                textPaint.MeasureText(mFirstName, ref textBoundsFirstName);

                float xTextFirstName = info.Width - (textBoundsFirstName.Width * 2) - 50;
                float yTextFirstName = info.Height - (textBoundsFirstName.Height * 2) - 100;


                canvas.DrawText(mFirstName, xTextFirstName, yTextFirstName, textPaint);
                #endregion

                #region Second Name Draw
                float textWidthSecondName = textPaint.MeasureText(mSecondName);

                SKRect textBoundsSecondName = new SKRect();
                textPaint.MeasureText(mSecondName, ref textBoundsSecondName);

                float xTextSecondName = info.Width - (textBoundsSecondName.Width * 2) - 50;
                float yTextSecondName = info.Height - textBoundsSecondName.Height - 50;

                canvas.DrawText(mSecondName, xTextSecondName, yTextSecondName, textPaint);
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
                
                canvas.DrawBitmap(firstPersonResizedImage, info.Width - rectWidth - 50, yTextFirstName - rectHeight - 25);

                var bitmapSecondPersonSign = SKBitmap.Decode(secondPersonImageFile);
                var secondPersonResizedImage = bitmapSecondPersonSign.Resize(new SKImageInfo(width, height), SKBitmapResizeMethod.Lanczos3);

                canvas.DrawBitmap(secondPersonResizedImage, info.Width - rectWidth - 50, yTextSecondName - rectHeight - 25);

                saveImage(surface);
            }
            catch(Exception)
            {
                await DisplayAlert("Alert", "필수 항목을 모두 입력하세요.", "OK");
            }
        }

        async void saveImage(SKSurface _surface)
        {

            string filePath = System.IO.Path.Combine(MakeDocPage.folderPathforSave, mFirstName + "_" + mSecondName + "_" + ".png");
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