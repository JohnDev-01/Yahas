using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Yahas.Views.Intro
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IntroPage : ContentPage
	{
		public IntroPage()
		{
			InitializeComponent();
			Animar();
		}
		private async void Animar()
		{
			await ImagenIcono.FadeTo(1, 1000);
			Application.Current.MainPage = new NavigationPage(new Login.LoginPage());
		}
	}
}