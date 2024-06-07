using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yahas.ViewModels;

namespace Yahas.Views.Menu.Kardex
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistorialKardex : ContentPage
	{
		public HistorialKardex()
		{
			InitializeComponent();
		}
		protected override async void OnAppearing()
		{
			await MostrarCategorias();
		}
		string Idcategoria;
		private async Task MostrarCategorias()
		{
			var viewModels = new VMcategorias();
			var list = await viewModels.MostrarCategoriasPorIdusuario();
			pickerCategorias.ItemsSource = null;
			foreach (var item in list)
			{
				pickerCategorias.Items.Add(item.Nombre);
				pickerCategorias.SelectedItem = item.Nombre;
			}

		}
		private async void dateFi_DateSelected(object sender, DateChangedEventArgs e)
		{
			ValidarFechasSeleccionada();
			await MostrarRegistroKardex();
		}

		private async void dateFf_DateSelected(object sender, DateChangedEventArgs e)
		{
			ValidarFechasSeleccionada();
			await MostrarRegistroKardex();
		}
		private async Task MostrarRegistroKardex()
		{
			var viewModels = new VMkardex();
			var list = await viewModels.MostrarHistorialKardex(Idcategoria, dateFi.Date, dateFf.Date);
			collectionRegistros.ItemsSource = list;
		}
		private void ValidarFechasSeleccionada()
		{
			if (dateFi.Date > dateFf.Date)
			{
				dateFf.Date = dateFi.Date;
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
		private async void pickerCategorias_SelectedIndexChanged(object sender, EventArgs e)
		{
			await MostrarIdCategoria();
			await MostrarRegistroKardex();
		}

		private async void btnEliminar_Clicked(object sender, EventArgs e)
		{
			
			var state = await DisplayAlert("Eliminando:", "Estas seguro de querer eliminar el registro seleccionado?", "SI", "NO");
			if (state == true)
			{
				try
				{
					UserDialogs.Instance.ShowLoading("Eliminando...");
					string Idregistro = ((ImageButton)sender).CommandParameter.ToString();
					var viewModels = new VMkardex();
					var viewModels2 = new VMcategorias();

					var modelsRegistro = await viewModels.MostrarHistorialKardexPorId(Idregistro);
					var modelscategoria = await viewModels2.MostrarCategoriasPorId(modelsRegistro.Idcategoria);
					#region Editar la cantidad de stock
					double stockExistente = Convert.ToDouble(modelscategoria.Stock);
					double stockListo = 0;
					if (modelsRegistro.TipoKardex == "ENTRADA")
					{
						stockListo = stockExistente - Convert.ToDouble(modelsRegistro.Unidades);
					}
					if (modelsRegistro.TipoKardex == "SALIDA")
					{
						stockListo = stockExistente + Convert.ToDouble(modelsRegistro.Unidades);
					}

					if (stockListo < 0)
						stockListo = 0;


					await viewModels2.EditarUnidadesCategorias(modelsRegistro.Idcategoria, stockListo.ToString());
					#endregion
					#region Eliminar el registro
					await viewModels.EliminarRegistroKardex(Idregistro);
					#endregion
					await MostrarIdCategoria();
					await MostrarRegistroKardex();
					UserDialogs.Instance.HideLoading();
				}
				catch (Exception ex)
				{

				}
			}
			else
			{
				return;
			}
			await MostrarCategorias();
		}
	}
}