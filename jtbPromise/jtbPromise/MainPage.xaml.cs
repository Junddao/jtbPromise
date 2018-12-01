using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace jtbPromise
{
    public partial class MainPage : ContentPage
    {

        

        public MainPage()
        {
            InitializeComponent();
            btnCreateOffLine.Clicked += BtnCreateOffLine_Clicked;
            btnSearch.Clicked += BtnSearch_Clicked;

            CheckPermissions();
        }
        void BtnCreateOffLine_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new MakeDocPage());
        }


        void BtnSearch_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new SearchDocPage());
        }

        void CheckPermissions()
        {
           

        }
    }
}
