using Acr.UserDialogs;
using Firebase.Auth;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yahas.Models;
using Yahas.ViewModels;

namespace Yahas.Views.Login
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CrearCuenta : ContentPage
	{
		public CrearCuenta()
		{
			InitializeComponent();
		}
		public string idusuario;
		MediaFile file;
		private string rutafoto;

		private void btnVolver_Clicked(object sender, EventArgs e)
		{

        }

		private async void btnFoto_Clicked(object sender, EventArgs e)
		{
			await CrossMedia.Current.Initialize();
			try
			{
				file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
				{
					PhotoSize = PhotoSize.Medium
				});

				if (file == null)
				{
					return;
				}

				imgIcono.Source = ImageSource.FromStream(file.GetStream);
			}
			catch (Exception ex)
			{
				await DisplayAlert("error:", ex.Message, "ok");
			}
		}

		private async void btnCrearCuenta_Clicked(object sender, EventArgs e)
		{
			//Validation Espacios
			
			if (file != null)
			{
				if (!string.IsNullOrEmpty(txtNombre.Text) && !string.IsNullOrEmpty(txtContra.Text))
				{
					if(txtContra.Text.Count() >= 6)
					{
						UserDialogs.Instance.ShowLoading("Creando...");
						await CrearCuentaLogic();
						await IniciarSesion();
						await ObtenerIdUser();
						await SubirFoto();
						await InsertarNegocios();
						UserDialogs.Instance.HideLoading();
						await DisplayAlert("LOGIN:", "CUENTA CREADA CORRECTAMENTE, VUELVA A INICIAR LA APP!", "OK");
						Process.GetCurrentProcess().CloseMainWindow();
					}
					else
					{
						await DisplayAlert("LOGIN:", "INGRESA UNA CONTRASEÑA CON AL MENOS 6 DIGITOS", "OK");
					}

				}
				else
				{
					await DisplayAlert("LOGIN:", "INGRESA DATOS VALIDOS", "OK");
				}
			}
			else
			{
				await DisplayAlert("LOGIN:", "SELECCIONA UNA FOTO DE PERFIL PARA TU CUENTA", "OK");
			}
		}
		private async Task SubirFoto()
		{
			var funcion = new VMAdministrador();
			rutafoto = await funcion.SubirImagenStorage(idusuario, file.GetStream());
		}

		private async Task InsertarNegocios()
		{
			
			var models = new Mnegocios()
			{
				idusuario = idusuario,
				foto = rutafoto,
				nombre = txtNombre.Text
			};
			var vmnegocio = new VMnegocios();
			await vmnegocio.InsertarNegocios(models);
		}
		private void CerrarSesion()
		{
			Preferences.Remove("MyFirebaseRefreshToken");
		}
		private async Task CrearCuentaLogic()
		{
			var clas = new VMcrearCuenta();
			await clas.CrearCuenta(txtCorreo.Text, txtContra.Text);
		}
		private async Task IniciarSesion()
		{
			var func = new VMcrearCuenta();
			await func.Validarcuenta(txtCorreo.Text, txtContra.Text);
		}
		private async Task ObtenerIdUser()
		{
			try
			{
				var authprovider = new FirebaseAuthProvider(new FirebaseConfig(Constantes.webApy));
				var id = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
				var refresh = await authprovider.RefreshAuthAsync(id);
				Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(refresh));

				idusuario = id.User.LocalId;
			}
			catch (Exception ex)
			{
				await DisplayAlert("LOGIN:", "SESION EXPIRADA", "OK");
			}

		}

	}
}