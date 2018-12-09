using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using TouchTracking;
using System.IO;

namespace jtbPromise
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignPage : ContentPage
	{
        Dictionary<long, FingerPaintPolyline> inProgressPolylines = new Dictionary<long, FingerPaintPolyline>();
        List<FingerPaintPolyline> completedPolylines = new List<FingerPaintPolyline>();

        static string folderPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
        static string fileName = string.Empty;
        static string filePath = string.Empty;


        SKBitmap saveBitmap;

        string mPersonNumber = string.Empty;

        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            StrokeWidth = 2,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        public SignPage(string personNumber)
        {
            mPersonNumber = personNumber;
            fileName = personNumber + ".png";
            filePath = folderPath + fileName;

            InitializeComponent();
        }

        private void BtnClear_Clicked(object sender, EventArgs e)
        {
            completedPolylines.Clear();
            canvasView.InvalidateSurface();
        }

        void UpdateBitmap()
        {
            using (SKCanvas saveBitmapCanvas = new SKCanvas(saveBitmap))
            {
                saveBitmapCanvas.Clear();

                foreach (FingerPaintPolyline path in completedPolylines)
                {
                    saveBitmapCanvas.DrawPath(path.Path, paint);
                }

                foreach (FingerPaintPolyline path in inProgressPolylines.Values)
                {
                    saveBitmapCanvas.DrawPath(path.Path, paint);
                }
            }

            canvasView.InvalidateSurface();
        }

        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!inProgressPolylines.ContainsKey(args.Id))
                    {
                        Color strokeColor = Color.Black;
                        float strokeWidth = ConvertToPixel(new float[] { 1, 2, 5, 10, 20 }[0]);

                        FingerPaintPolyline polyline = new FingerPaintPolyline
                        {
                            StrokeColor = strokeColor,
                            StrokeWidth = strokeWidth
                        };
                        polyline.Path.MoveTo(ConvertToPixel(args.Location));

                        inProgressPolylines.Add(args.Id, polyline);
                        UpdateBitmap();
                    }
                    break;

                case TouchActionType.Moved:
                    if (inProgressPolylines.ContainsKey(args.Id))
                    {
                        FingerPaintPolyline polyline = inProgressPolylines[args.Id];
                        polyline.Path.LineTo(ConvertToPixel(args.Location));
                        UpdateBitmap();
                    }
                    break;

                case TouchActionType.Released:
                    if (inProgressPolylines.ContainsKey(args.Id))
                    {
                        completedPolylines.Add(inProgressPolylines[args.Id]);
                        inProgressPolylines.Remove(args.Id);
                        UpdateBitmap();
                    }
                    break;

                case TouchActionType.Cancelled:
                    if (inProgressPolylines.ContainsKey(args.Id))
                    {
                        inProgressPolylines.Remove(args.Id);
                        UpdateBitmap();
                    }
                    break;
            }
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            // Create bitmap the size of the display surface
            if (saveBitmap == null)
            {
                saveBitmap = new SKBitmap(info.Width, info.Height);
            }
            // Or create new bitmap for a new size of display surface
            else if (saveBitmap.Width < info.Width || saveBitmap.Height < info.Height)
            {
                SKBitmap newBitmap = new SKBitmap(Math.Max(saveBitmap.Width, info.Width),
                                                  Math.Max(saveBitmap.Height, info.Height));

                using (SKCanvas newCanvas = new SKCanvas(newBitmap))
                {
                    newCanvas.Clear();
                    newCanvas.DrawBitmap(saveBitmap, 0, 0);
                }

                saveBitmap = newBitmap;
            }

            // Render the bitmap
            canvas.Clear();
            canvas.DrawBitmap(saveBitmap, 0, 0);
        }


        SKPoint ConvertToPixel(Point pt)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                               (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
        }

        float ConvertToPixel(float fl)
        {
            return (float)(canvasView.CanvasSize.Width * fl / canvasView.Width);
        }

        private async void BtnNext_Clicked(object sender, EventArgs e)
        {
            try
            {
                var surface = SKSurface.Create(
                (int)canvasView.CanvasSize.Width,
                (int)canvasView.CanvasSize.Height,
                SKImageInfo.PlatformColorType,
                SKAlphaType.Premul);
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.White);

                foreach (FingerPaintPolyline path in completedPolylines)
                    canvas.DrawPath(path.Path, paint);

                foreach (FingerPaintPolyline path in inProgressPolylines.Values)
                    canvas.DrawPath(path.Path, paint);

                using (var pngData = surface.Snapshot().Encode(SKEncodedImageFormat.Png, 100))
                {
                    using (var stream = File.OpenWrite(filePath))
                    {
                        pngData.SaveTo(stream);
                    }
                }

                // 저장되면 OK popup
                await DisplayAlert("Save", "Complete your sign.", "OK");

                await Navigation.PopToRootAsync();
            }
            catch(Exception ex)
            {
                await DisplayAlert("Alert", "Can't save your Sign.", "OK");
            }
        }



    }
}