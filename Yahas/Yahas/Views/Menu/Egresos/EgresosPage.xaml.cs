using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yahas.Models;
using Yahas.ViewModels;

namespace Yahas.Views.Menu.Egresos
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EgresosPage : ContentPage
	{
		private string Idcategoria;

		public EgresosPage ()
		{
			InitializeComponent ();
			
		}
		protected override async void OnAppearing()
		{
			LimpiarGastos();
			await MostrarCategorias();
		}
		private async void btnGuardar_Clicked(object sender, EventArgs e)
		{
			if (await ValidateEgresos() == true)
			{
				await InsertarNuevoGasto();
			}
        }
		private async Task InsertarNuevoGasto()
		{
			var viewModels = new VMegresos();
			var viewModelsNegocios = new VMnegocios();
			string IdusuarioLogeado = await viewModelsNegocios.ObtenerIdusuario();

			var modelsEgreso = new Megresos()
			{
				Concepto = txtConcepto.Text,
				Fecha = datepickerFecha.Date.ToString(),
				Idusuario = IdusuarioLogeado,
				Monto = txtImprte.Text,
				Idcategoria = Idcategoria
			};
			await viewModels.GuardarNuevoGasto(modelsEgreso);
			txtConcepto.Text = null;
			txtImprte.Text = null;
			await DisplayAlert("EGRESOS", "Registro guardado correctamente", "OK");
		}
		private async Task<bool> ValidateEgresos()
		{
			// Validacion de importe con numeros validos 
			double monto = 0;
			try
			{
			  monto = Convert.ToDouble(txtImprte.Text);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Importe:", "Importe invalido, valida!", "OK");
				return false;
			}

			if (monto >0)
			{
				if (txtConcepto.Text == "" || txtConcepto.Text == null)
					txtConcepto.Text = "(SIN CONCEPTO AGREGADO)";

				return true;

			}
			else
			{
				await DisplayAlert("Importe:", "Importe invalido, valida!", "OK");
				return false;
			}
		}
		private async void btnHistorial_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new HistorialEgresos());
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
		private async void pickerCategorias_SelectedIndexChanged(object sender, EventArgs e)
		{
			
			await MostrarIdCategoria();
        }
		private void LimpiarGastos()
		{
			txtConcepto.Text = "";
			txtImprte.Text = "";
			Idcategoria = null;
		}
    }
}