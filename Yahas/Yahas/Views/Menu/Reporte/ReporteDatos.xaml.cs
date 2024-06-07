using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yahas.ViewModels;

namespace Yahas.Views.Menu.Reporte
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReporteDatos : ContentPage
	{
		public ReporteDatos()
		{
			InitializeComponent();
		}
		protected override async void OnAppearing()
		{
			UserDialogs.Instance.ShowLoading("Mostrando...");
			await MostrarNegociosInfo();
			await MostrarCategorias();
			await MostrarIdCategoria();
			await MostrarInformacionReporte();
			UserDialogs.Instance.HideLoading();
		}
		string Idcategoria;
		private async Task MostrarNegociosInfo()
		{

			var viewModels = new VMnegocios();
			var models = await viewModels.MostrarInformacionNegocio();
			lblNombre.Text = "Hola, " + models.nombre + "!";
			ImagenIcono.Source = models.foto;

		}
		private async Task MostrarCategorias()
		{
			var viewModels = new VMcategorias();
			var list = await viewModels.MostrarCategoriasPorIdusuario();
			pickerCategorias.ItemsSource = null;

			foreach (var item in list)
			{
				pickerCategorias.Items.Remove(item.Nombre);
				pickerCategorias.Items.Add(item.Nombre);
				pickerCategorias.SelectedItem = item.Nombre;
			}

		}
		private async Task MostrarIdCategoria()
		{
			try
			{
				var viewModels = new VMcategorias();
				var list = await viewModels.MostrarCategoriasPorNombre(pickerCategorias.SelectedItem.ToString());
				Idcategoria = list.Idcategorias;

			}
			catch (Exception ex)
			{

			}
			
		}
		private async Task MostrarInformacionReporte()
		{
			var vmReport = new VMresumenProductos();
			if (Idcategoria != null && Idcategoria != "")
			{
				var list = await vmReport.ListarResumen(Idcategoria, dateFi.Date, dateFf.Date);
				collectionReport.ItemsSource = list;
			}

		}
		private async void pickerCategorias_SelectedIndexChanged(object sender, EventArgs e)
		{
			await MostrarIdCategoria();
			await MostrarInformacionReporte();
		}
		private void ValidarFechasSeleccionada()
		{
			if (dateFi.Date > dateFf.Date)
			{
				dateFf.Date = dateFi.Date;
			}
		}
		private async void dateFf_DateSelected(object sender, DateChangedEventArgs e)
		{
			ValidarFechasSeleccionada();
			await MostrarInformacionReporte();
		}

		private async void dateFi_DateSelected(object sender, DateChangedEventArgs e)
		{
			ValidarFechasSeleccionada();
			await MostrarInformacionReporte();
		}
	}
}