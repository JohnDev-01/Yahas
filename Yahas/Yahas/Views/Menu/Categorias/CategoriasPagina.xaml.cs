using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yahas.ViewModels;

namespace Yahas.Views.Menu.Categorias
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoriasPagina : ContentPage
	{
		public CategoriasPagina ()
		{
			InitializeComponent ();
		}
		protected override async void OnAppearing()
		{
			var viewModels = new VMcategorias();
			var list = await viewModels.MostrarCategoriasPorIdusuario();
			collectionCategorias.ItemsSource = list;
		}
		private async void btnAgregar_Clicked(object sender, EventArgs e)
		{
			AdministrarCategorias.TipoGuadado = "NUEVO";
			await Navigation.PushAsync(new AdministrarCategorias());
        }

		private async void btnEditar_Clicked(object sender, EventArgs e)
		{
			string Idcategoria = ((ImageButton)sender).CommandParameter.ToString();
			AdministrarCategorias.TipoGuadado = "EDITAR";
			AdministrarCategorias.Idcategoria = Idcategoria;
			await Navigation.PushAsync(new AdministrarCategorias());
		}
	}
}