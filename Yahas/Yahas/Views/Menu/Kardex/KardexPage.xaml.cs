using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yahas.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using Yahas.Models;
using System.Transactions;
using Acr.UserDialogs;

namespace Yahas.Views.Menu.Kardex
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class KardexPage : ContentPage
	{
		public KardexPage ()
		{
			InitializeComponent ();
		}
		string Idcategoria;
		string TipoKardex;
		protected override async void OnAppearing()
		{
			await MostrarCategorias();
			rbEntrada.IsChecked = true;
		}
		private async Task MostrarCategorias()
		{
			var viewModels = new VMcategorias();
			var list = await viewModels.MostrarCategoriasPorIdusuario();
			pickerCategorias.ItemsSource =  null ;
			
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
		private async void btnHistorial_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new HistorialKardex());
        }

		private async void btnGuardar_Clicked(object sender, EventArgs e)
		{
			bool state = await ValidarEspacios();
			if(state == true)
			{
				UserDialogs.Instance.ShowLoading("Guardando...");
				await MostrarIdCategoria();
				await EditarPreciosCategoria();
			    ValidarTipoDeKardex();
				await CalcularProximoStock();
				await InsertarRegistroKardex();
				UserDialogs.Instance.HideLoading();
			}
		}

		private async void pickerCategorias_SelectedIndexChanged(object sender, EventArgs e)
		{
			await MostrarIdCategoria();
			var viewModels = new VMcategorias();
			var models = await viewModels.MostrarCategoriasPorId(Idcategoria);
			txtPrecioCompra.Text = models.PrecioCompra;
			txtPrecioVenta.Text = models.PrecioVenta;
		}
		private async Task<bool> ValidarEspacios()
		{
			ValidarTipoDeKardex();
			bool state =false;
			if(txtCantidad.Text=="" || txtPrecioCompra.Text ==""||txtPrecioVenta.Text ==""||
				rbEntrada.IsChecked == false && rbSalida.IsChecked == false)
			{
				state = false;
				if (Convert.ToDouble(txtPrecioVenta.Text) <= Convert.ToDouble(txtPrecioCompra.Text))
				{
					await DisplayAlert("Alerta:", "El precio de venta puede que no genere ganancias, aplica un precio mayor al de compra.", "OK");
					
				}
				else
				{
					await DisplayAlert("Alerta:", "Verifica que todos los datos esten correctos", "OK");

				}
			}
			else
			{
				state = true;
			}

			return state;
		}
		private void ValidarTipoDeKardex()
		{
			if (rbEntrada.IsChecked == true)
			{
				TipoKardex = "ENTRADA";
			}
			if (rbSalida.IsChecked == true)
			{
				TipoKardex = "SALIDA";
			}
		}
		private async Task EditarPreciosCategoria()
		{
			var viewModels = new VMcategorias();
			await viewModels.EditarPreciosCategorias(Idcategoria, txtPrecioCompra.Text, txtPrecioVenta.Text);
		}
		private async Task<bool> ValidarUnidadesDeSalidaKardex()
		{
			var viewModels = new VMcategorias();
			var models = await viewModels.MostrarCategoriasPorId(Idcategoria);
			double cantidades = Convert.ToDouble( models.Stock);
			if (Convert.ToDouble(txtCantidad.Text) > cantidades && TipoKardex == "SALIDA")
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		private async Task CalcularProximoStock()
		{
			var state = await ValidarUnidadesDeSalidaKardex();
			if (state == true)
			{
				var viewModels = new VMcategorias();
				await viewModels.ActualizarCantidadCategoria(Idcategoria, TipoKardex, txtCantidad.Text);
			}
			else
			{
				await DisplayAlert("AVISO:", "Estas regitrando una salida mayor a la cantidad disponible, verifica.", "OK");
				return;
			}
		}
		private async Task InsertarRegistroKardex()
		{
			#region Calculo de ganancia  
			double ganancia = 0;
			if (TipoKardex == "SALIDA")
			{
				ganancia = (Convert.ToDouble(txtPrecioVenta.Text) - Convert.ToDouble(txtPrecioCompra.Text)) * Convert.ToDouble(txtCantidad.Text); 
			}
			if(TipoKardex == "ENTRADA")
			{
				ganancia = 0;
			}
			#endregion
			#region Identificar Idusuario
			var viewmodels = new VMnegocios();
			string idusuario = await viewmodels.ObtenerIdusuario();
			#endregion

			var models = new Mkardex()
			{
				Fecha = datepickerFecha.Date.ToString(),
				Ganancia = ganancia.ToString(),
				Idcategoria = Idcategoria,
				Idusuario = idusuario,
				TipoKardex = TipoKardex,
				Unidades = txtCantidad.Text,
				PrecioCompra = txtPrecioCompra.Text,
				PrecioVenta = txtPrecioVenta.Text
			};
			var vmKardex = new VMkardex();
			await vmKardex.InsertarRegistroKardex(models);
			LimpiarRegistros();
		}
		private void rbEntrada_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			ValidarTipoDeKardex();
		}

		private void rbSalida_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			ValidarTipoDeKardex();
		}
		private void LimpiarRegistros()
		{
			txtCantidad.Text = "";

		}
	}
}	