using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yahas.Views.Intro;

namespace Yahas
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new IntroPage();
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
