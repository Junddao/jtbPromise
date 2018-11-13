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
		public MakeDocPage ()
		{
			InitializeComponent ();
            btnFirstPersonCert.Clicked += BtnFirstPersonCert_Clicked;
        }

        void BtnFirstPersonCert_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CertificatePage());
        }
    }
}