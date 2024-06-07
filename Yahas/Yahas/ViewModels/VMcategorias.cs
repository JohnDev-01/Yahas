using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yahas.Models;

namespace Yahas.ViewModels
{
	public class VMcategorias
	{
		public async Task<Mcategorias> MostrarCategoriasPorId(string Idcategoria)
		{
			var viewModels = new VMnegocios();
			var idusuario = await viewModels.ObtenerIdusuario();
			var list = new List<Mcategorias>();

			var obj = (await Constantes.firebase
						.Child("Categorias")
						.OrderByKey()
						.OnceAsync<Mcategorias>()).Where(a => a.Key != "Models" && a.Key==Idcategoria).FirstOrDefault();
			
				var models = new Mcategorias();
				models = obj.Object;
				models.Idcategorias = obj.Key;
			
			return models;
		}
		public async Task<Mcategorias> MostrarCategoriasPorNombre(string Nombre)
		{
			var viewModels = new VMnegocios();
			var idusuario = await viewModels.ObtenerIdusuario();
			var list = new List<Mcategorias>();

			var obj = (await Constantes.firebase
						.Child("Categorias")
						.OrderByKey()
						.OnceAsync<Mcategorias>()).Where(a => a.Key != "Models" && a.Object.Idusuario==idusuario
						&&a.Object.Nombre ==Nombre).FirstOrDefault();
			
				var models = new Mcategorias();
				models = obj.Object;
				models.Idcategorias = obj.Key;
			
			return models;
		}
		public async Task<List<Mcategorias>> MostrarCategoriasPorIdusuario()
		{
			var viewModels = new VMnegocios();
			var idusuario = await viewModels.ObtenerIdusuario();
			var list = new List<Mcategorias>();

			var obj = (await Constantes.firebase
						.Child("Categorias")
						.OrderByKey()
						.OnceAsync<Mcategorias>()).Where(a => a.Key != "Models" && a.Object.Idusuario == idusuario);
			foreach (var item in obj)
			{
				var models = new Mcategorias();
				models = item.Object;
				models.Stock = "Stock: "+item.Object.Stock;
				models.Idcategorias = item.Key;
				list.Add(models);
			}
			return list;
		}
		public async Task InsertarNuevaCategoria(Mcategorias models)
		{
			try
			{
				await Constantes.firebase
					.Child("Categorias")
					.PostAsync(models);
			}
			catch (Exception ex)
			{

			}
		}
		public async Task EditarNombreCategorias(string idcategoria, string Nombre)
		{
			var obj = (await Constantes.firebase
						.Child("Categorias")
						.OrderByKey()
						.OnceAsync<Mcategorias>()).Where(a => a.Key == idcategoria).FirstOrDefault();

			obj.Object.Nombre = Nombre;

			await Constantes.firebase
						.Child("Categorias")
						.Child(obj.Key)
						.PutAsync(obj.Object);
		}
		public async Task EditarUnidadesCategorias(string idcategoria, string Unidades)
		{
			var obj = (await Constantes.firebase
						.Child("Categorias")
						.OrderByKey()
						.OnceAsync<Mcategorias>()).Where(a => a.Key == idcategoria).FirstOrDefault();

			obj.Object.Stock = Unidades;

			await Constantes.firebase
						.Child("Categorias")
						.Child(obj.Key)
						.PutAsync(obj.Object);
		}
		public async Task EditarPreciosCategorias(string idcategoria, string precioCompra,string precioventa)
		{
			var obj = (await Constantes.firebase
						.Child("Categorias")
						.OrderByKey()
						.OnceAsync<Mcategorias>()).Where(a => a.Key == idcategoria).FirstOrDefault();

			obj.Object.PrecioCompra = precioCompra;
			obj.Object.PrecioVenta = precioventa;

			await Constantes.firebase
						.Child("Categorias")
						.Child(obj.Key)
						.PutAsync(obj.Object);
		}
		public async Task ActualizarCantidadCategoria(string Idcategoria, string tipoKardex, string Unidades)
		{
			var modelsCategorias = await MostrarCategoriasPorId(Idcategoria);
			double cantidadStock = Convert.ToDouble(modelsCategorias.Stock);

			double stockExistencia=0;
			if(tipoKardex == "ENTRADA")
			{
				stockExistencia = cantidadStock + Convert.ToDouble(Unidades);
			}
			if (tipoKardex == "SALIDA")
			{
				if ( cantidadStock <= 0)
				{
					stockExistencia = 0;
				}
				else
				{
					stockExistencia = cantidadStock - Convert.ToDouble(Unidades);
				}
			}




			var obj = (await Constantes.firebase
						.Child("Categorias")
						.OrderByKey()
						.OnceAsync<Mcategorias>()).Where(a => a.Key == Idcategoria).FirstOrDefault();

			obj.Object.Stock = stockExistencia.ToString();
			
			await Constantes.firebase
						.Child("Categorias")
						.Child(obj.Key)
						.PutAsync(obj.Object);

		}
	}
}
