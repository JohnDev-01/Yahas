using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yahas.Models;
using System.Linq;

namespace Yahas.ViewModels
{
	public class VMegresos
	{
		public async Task GuardarNuevoGasto(Megresos models)
		{
			try
			{
				await Constantes.firebase
								.Child("Egresos")
								.PostAsync(models);
			}
			catch (Exception ex)
			{

			}

		}
		public async Task<List<Megresos>> MostrarGastosRegistrados(DateTime fi, DateTime ff,string Idcategoria)
		{
			var viewModelsUser = new VMnegocios();
			var IdusuarioLogueado = await viewModelsUser.ObtenerIdusuario();

			var obj = (await Constantes.firebase
				.Child("Egresos")
				.OrderByKey().
				OnceAsync<Megresos>()).Where(a => a.Key != "models" && a.Object.Idusuario == IdusuarioLogueado && a.Object.Idcategoria == Idcategoria);

			var list = new List<Megresos>();
			foreach (var item in obj)
			{
				var models = new Megresos();
				models = item.Object;
				models.IdEgreso = item.Key;
				models.Monto = "- RD$" + Constantes.AsignarComa(Convert.ToDouble(models.Monto));
				models.Fecha = Convert.ToDateTime(models.Fecha).ToString("dd/MM/yyyy");
				list.Add(models);
			}
			return list;
		}
		public async Task EliminarEgreso(string Idegreso)
		{
			try
			{
				await Constantes.firebase
					.Child("Egresos")
					.Child(Idegreso)
					.DeleteAsync();
			}
			catch (Exception ex )
			{
			}
		}
	}
}
