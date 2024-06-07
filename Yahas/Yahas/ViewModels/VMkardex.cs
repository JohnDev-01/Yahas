using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yahas.Models;
using System.Linq;

namespace Yahas.ViewModels
{
	public class VMkardex
	{
		public async Task InsertarRegistroKardex(Mkardex models)
		{
			try
			{
				await Constantes.firebase
					.Child("Kardex")
					.PostAsync(models);
			}
			catch (Exception ex)
			{

			}
		}
		public async Task<Mkardex> MostrarHistorialKardexPorId(string Idkardex)
		{
			var obj = (await Constantes.firebase.
				Child("Kardex")
				.OrderByKey()
				.OnceAsync<Mkardex>()).Where(a => a.Key == Idkardex).FirstOrDefault();

			
				var models = new Mkardex();
				models = obj.Object;
				models.Idkardex = obj.Key;
				
			return models;
		}
		public async Task<List<Mkardex>> MostrarHistorialKardex(string Idcategoria, DateTime fi, DateTime ff)
		{
			var list = new List<Mkardex>();
			try
			{
				var obj = (await Constantes.firebase.
				Child("Kardex")
				.OrderByKey()
				.OnceAsync<Mkardex>()).Where(a => a.Key != "models" &&
				a.Object.Idcategoria == Idcategoria
				&& Convert.ToDateTime(a.Object.Fecha) >= fi &&

				Convert.ToDateTime(a.Object.Fecha) <= ff);

				
				foreach (var item in obj)
				{
					var models = new Mkardex();
					models = item.Object;
					models.Idkardex = item.Key;
					if (models.TipoKardex == "ENTRADA")
					{
						models.Color = "#18893F";
						models.TipoKardex = "COMPRAS";
						models.Unidades = "+" + models.Unidades;
					}
					else
					{
						models.Color = "Red";
						models.TipoKardex = "VENTAS";
						models.Unidades = "-" + models.Unidades;
					}
					models.Fecha = Convert.ToDateTime(models.Fecha).ToString("dd/MM/yyyy");
					list.Add(models);
				}
				
			}
			catch (Exception ex)
			{

			}
			return list;
		}
		public async Task EliminarRegistroKardex(string Idregistro)
		{
			await Constantes.firebase
				.Child("Kardex")
				.Child(Idregistro)
				.DeleteAsync();
		}
	}
}
