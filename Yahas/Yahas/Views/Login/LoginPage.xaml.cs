using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yahas.ViewModels;
using Yahas.Views.Menu;

namespace Yahas.Views.Login
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();
			MostrarUsuarioRecordado();
		}
		private async Task ValidarDatos()
		{

			var func = new VMcrearCuenta();
			var estate = await func.Validarcuenta(txtCorreo.Text, txtContra.Text);

			UserDialogs.Instance.HideLoading();
			if (estate == true)
			{
				Application.Current.MainPage = new NavigationPage(new Contenedor());
			}
			else
			{
				await DisplayAlert("Alerta:", "Datos invalidos, verifica que tus datos sean los correctos", "OK");
			}


		}
		private async void btnIniciarSesion_Clicked(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(txtCorreo.Text))
			{
				if (!string.IsNullOrEmpty(txtContra.Text))
				{

					UserDialogs.Instance.ShowLoading("Iniciando sesion...");
					ValidateSwich();
					RecordarUsuarioInscrito();
					await ValidarDatos();
				}
				else
				{
					await DisplayAlert("Valida:", "Ingresa una contraseña valida", "OK");
				}
			}
			else
			{
				await DisplayAlert("Valida:", "Ingresa un correo valido", "OK");
			}
		}
		private void ValidateSwich()
		{
			if (swichRecordarUser.IsToggled == true)
			{
				estadoRecordarCuenta = true;
			}
			else
			{
				estadoRecordarCuenta = false;
			}
		}
		private async void btnCrearCuenta_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new CrearCuenta());
        }

		private void swichRecordarUser_Toggled(object sender, ToggledEventArgs e)
		{

        }
		bool estadoRecordarCuenta;
		private void MostrarUsuarioRecordado()
		{
			var value = Preferences.Get("EstadoRecordarCuenta", "");
			if (value == "True")
			{
				var correo = Preferences.Get("Correo", "");
				var pass = Preferences.Get("Contra", "");
				txtContra.Text = pass;
				txtCorreo.Text = correo;
				swichRecordarUser.IsToggled = true;
			}
			else
			{
				swichRecordarUser.IsToggled = false;
			}
		}
		private void RecordarUsuarioInscrito()
		{
			var correo = txtCorreo.Text;
			var contra = txtContra.Text;

			Preferences.Set("EstadoRecordarCuenta", estadoRecordarCuenta.ToString());
			Preferences.Set("Correo", correo);
			Preferences.Set("Contra", contra);
		}

	}
}