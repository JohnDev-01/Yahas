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
	public partial class AdministrarCategorias : ContentPage
	{
		public AdministrarCategorias ()
		{
			InitializeComponent ();
		}
		public static string TipoGuadado;
		public static string Idcategoria;
		protected override async void OnAppearing()
		{
			if (TipoGuadado == "EDITAR")
			{
				var viewModels = new VMcategorias();
				var models = await viewModels.MostrarCategoriasPorId(Idcategoria);
				txtCategoria.Text = models.Nombre;
			}
		}
		private async void btnEditar_Clicked(object sender, EventArgs e)
		{
			if (TipoGuadado == "NUEVO")
			{
				await GuardarNuevaCategoria();
			}
			else
			{
				await GuardarCategoriaEditada();
			}
        }
		private async Task GuardarCategoriaEditada()
		{
			var viewModels = new VMcategorias();
			var vmNegocios = new VMnegocios();
			string Idusuario = await vmNegocios.ObtenerIdusuario();
			await viewModels.EditarNombreCategorias(Idcategoria, txtCategoria.Text);
			await Navigation.PopAsync();
		}
		private async Task GuardarNuevaCategoria()
		{
			var viewModels = new VMcategorias();
			var vmNegocios = new VMnegocios();
			string Idusuario = await vmNegocios.ObtenerIdusuario();
			await viewModels.InsertarNuevaCategoria(new Models.Mcategorias()
			{
				Nombre = txtCategoria.Text,
				Idusuario = Idusuario,
				PrecioCompra = "0",
				PrecioVenta = "0",
				Stock = "0"
			});
			await Navigation.PopAsync();
		}
    }
}