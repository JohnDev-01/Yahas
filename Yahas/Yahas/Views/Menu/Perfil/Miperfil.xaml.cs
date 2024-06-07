using Acr.UserDialogs;
using Plugin.Media.Abstractions;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yahas.ViewModels;
using Yahas.Models;

namespace Yahas.Views.Menu.Perfil
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Miperfil : ContentPage
	{
		public Miperfil ()
		{
			InitializeComponent ();
		}
		MediaFile file;
		bool estadoModificar;
		bool estadoModificarfoto;
		string rutafoto;
		protected override async void OnAppearing()
		{
			await MostrarNegociosInfo();
			estadoModificar = false;
			estadoModificarfoto = false;
		}
		private async void bnCambiarPerfil_Clicked(object sender, EventArgs e)
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
				estadoModificar = true;
				estadoModificarfoto = true;

				ImagenPerfil.Source = null;
				ImagenPerfil.Source = ImageSource.FromStream(file.GetStream);
			}
			catch (Exception ex)
			{
				await DisplayAlert("error:", ex.Message, "ok");
			}
			
		}

		private async void btnGuardad_Clicked(object sender, EventArgs e)
		{
			if (estadoModificar == true)
			{
				UserDialogs.Instance.ShowLoading("Guardando...");

				await EditarInformacion();
				
				UserDialogs.Instance.HideLoading();
			}
        }
		private async Task MostrarNegociosInfo()
		{
			UserDialogs.Instance.ShowLoading("Mostrando...");
			var viewModels = new VMnegocios();
			var models = await viewModels.MostrarInformacionNegocio();
			txtNombre.Text = models.nombre;
			ImagenPerfil.Source = models.foto;
			rutafoto= models.foto;
			UserDialogs.Instance.HideLoading();
		}
		private async Task EditarInformacion()
		{
			var viewModels = new VMnegocios();
			if (estadoModificarfoto == true)
			{
				rutafoto = await viewModels.EditarFotoPerfilStorage(file);
			}
			var models = new Mnegocios();
			
			models.nombre = txtNombre.Text;
			models.foto = rutafoto;
			await viewModels.EditarInformacion(models);
		}
		private void txtNombre_TextChanged(object sender, TextChangedEventArgs e)
		{
			estadoModificar = true;
		}
	}
}