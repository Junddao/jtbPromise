using System;
using System.Collections.Generic;
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


        public PreviewPage(string title, string content, string firstName, string secondName)
        {
            InitializeComponent();

            mTitle = title;
            mContent = content;
            mFirstName = firstName;
            mSecondName = secondName;
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            // Create an SKPaint object to display the text
            SKPaint textPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Black,
                StrokeWidth = 2,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round
            };


            #region Title Draw
            // Adjust TextSize property so text is 95% of screen width
            float textWidthTitle = textPaint.MeasureText(mTitle);
            textPaint.TextSize = 0.3f * info.Width * textPaint.TextSize / textWidthTitle;

            // Find the text bounds
            SKRect textBoundsTitle = new SKRect();
            textPaint.MeasureText(mTitle, ref textBoundsTitle);

            // Calculate offsets to center the text on the screen
            float xTextTitle = info.Width / 2 - textBoundsTitle.MidX;
            float yTextTitle = textBoundsTitle.Height + 10;

            // And draw the text
            canvas.DrawText(mTitle, xTextTitle, yTextTitle, textPaint);
            #endregion

            #region Content Draw
            float textWidthContent = textPaint.MeasureText(mContent);
            textPaint.TextSize = textPaint.TextSize * 0.5f;

            SKRect textBoundsContent = new SKRect();
            textPaint.MeasureText(mContent, ref textBoundsContent);

            float xTextContent = info.Width / 2 - textBoundsContent.MidX;
            float yTextContent = (textBoundsTitle.Height * 2) + textBoundsContent.Height + 30;

            canvas.DrawText(mContent, xTextContent, yTextContent, textPaint);
            #endregion

            #region First Name Draw
            float textWidthFirstName = textPaint.MeasureText(mFirstName);

            SKRect textBoundsFirstName = new SKRect();
            textPaint.MeasureText(mFirstName, ref textBoundsFirstName);

            float xTextFirstName = info.Width - textBoundsFirstName.Width - 200;
            float yTextFirstName = info.Height - (textBoundsFirstName.Height * 2) - 150;

            canvas.DrawText(mFirstName, xTextFirstName, yTextFirstName, textPaint);
            #endregion

            #region Second Name Draw
            float textWidthSecondName = textPaint.MeasureText(mSecondName);

            SKRect textBoundsSecondName = new SKRect();
            textPaint.MeasureText(mSecondName, ref textBoundsSecondName);

            float xTextSecondName = info.Width - textBoundsSecondName.Width - 200;
            float yTextSecondName = info.Height - textBoundsSecondName.Height - 150;

            canvas.DrawText(mSecondName, xTextSecondName, yTextSecondName, textPaint);
            #endregion
         
        }


        async void BtnOK_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}