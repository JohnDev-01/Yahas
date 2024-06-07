using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yahas.ViewModels;

namespace Yahas.Views.Menu.Egresos
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistorialEgresos : ContentPage
	{
		public HistorialEgresos ()
		{
			InitializeComponent ();
		}
		string Idcategoria;
		protected override async void OnAppearing()
		{
			await MostrarCategorias();
			await MostrarIdCategoria();
			await MostrarRegistroGasto();
		}
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
		private async Task MostrarRegistroGasto()
		{
			var viewModels = new VMegresos();
			var listEgresos = await viewModels.MostrarGastosRegistrados(dateFi.Date, dateFf.Date, Idcategoria);
			collectionRegistros.ItemsSource = listEgresos;
		}

		private async void dateFi_DateSelected(object sender, DateChangedEventArgs e)
		{
		 ValidarFechasSeleccionada();
			await MostrarRegistroGasto();
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
			await MostrarRegistroGasto();

		}

		private async void btnEliminar_Clicked(object sender, EventArgs e)
		{
			
			bool estate = await DisplayAlert("Eliminando:", "Estas seguro de querer eliminar el gasto seleccionado?", "SI","NO");
			if (estate == true)
			{
				string Idegreso = ((ImageButton)sender).CommandParameter.ToString();
				var viewModels = new VMegresos();
				await viewModels.EliminarEgreso(Idegreso);
				await MostrarRegistroGasto();
			}
        }

		private async void pickerCategorias_SelectedIndexChanged(object sender, EventArgs e)
		{
			await MostrarIdCategoria();
			await MostrarRegistroGasto();
		}
    }
}