using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Yahas.Models;

namespace Yahas.ViewModels
{
	public class VMresumenProductos
	{
		public async Task<double> SumarCapitalProductos(string idcategoria, DateTime fi, DateTime ff)
		{
			double Suma = 0;
			try
			{
				var obj = (await Constantes.firebase
					.Child("Kardex")
					.OrderByKey()
					.OnceAsync<Mkardex>()).Where(a => a.Key != "models" && a.Object.Idcategoria == idcategoria &&
					Convert.ToDateTime(a.Object.Fecha) >= fi && Convert.ToDateTime(a.Object.Fecha) <= ff && a.Object.TipoKardex =="ENTRADA");


				foreach (var item in obj)
				{
					Suma += Convert.ToDouble(item.Object.Unidades) * Convert.ToDouble(item.Object.PrecioCompra);
				}
			}
			catch (Exception)
			{

			}
			return Suma;
		}
		public async Task<double> SumarProductosInvertidos(string idcategoria, DateTime fi, DateTime ff)
		{
			double Suma = 0;
			try
			{
				var obj = (await Constantes.firebase
					.Child("Kardex")
					.OrderByKey()
					.OnceAsync<Mkardex>()).Where(a => a.Key != "models" && a.Object.Idcategoria == idcategoria &&
					Convert.ToDateTime(a.Object.Fecha) >= fi && Convert.ToDateTime(a.Object.Fecha) <= ff && a.Object.TipoKardex == "ENTRADA");


				foreach (var item in obj)
				{
					Suma += Convert.ToDouble(item.Object.Unidades);
				}
			}
			catch (Exception)
			{

			}
			return Suma;
		}
		public async Task<double> SumarProductosVendidos(string idcategoria, DateTime fi, DateTime ff)
		{
			double Suma = 0;
			try
			{
				var obj = (await Constantes.firebase
					.Child("Kardex")
					.OrderByKey()
					.OnceAsync<Mkardex>()).Where(a => a.Key != "models" && a.Object.Idcategoria == idcategoria &&
					Convert.ToDateTime(a.Object.Fecha) >= fi && Convert.ToDateTime(a.Object.Fecha) <= ff && a.Object.TipoKardex == "SALIDA");


				foreach (var item in obj)
				{
					Suma += Convert.ToDouble(item.Object.Unidades);
				}
			}
			catch (Exception)
			{

			}
			return Suma;
		}
		public async Task<double> SumarVentasBrutas(string idcategoria, DateTime fi, DateTime ff)
		{
			double Suma = 0;
			try
			{
				var obj = (await Constantes.firebase
					.Child("Kardex")
					.OrderByKey()
					.OnceAsync<Mkardex>()).Where(a => a.Key !="models" && a.Object.Idcategoria == idcategoria &&
					Convert.ToDateTime(a.Object.Fecha) >= fi && Convert.ToDateTime(a.Object.Fecha) <= ff && a.Object.TipoKardex == "SALIDA");


				foreach (var item in obj)
				{
					Suma += Convert.ToDouble(item.Object.PrecioVenta )* Convert.ToDouble(item.Object.Unidades);
				}
			}
			catch (Exception)
			{

			}
			return Suma;
		}
		public async Task<double> SumarEgresos(string idcategoria, DateTime fi, DateTime ff)
		{
			double Suma = 0;
			try
			{
				var obj = (await Constantes.firebase
					.Child("Egresos")
					.OrderByKey()
					.OnceAsync<Megresos>()).Where(a => a.Key != "models" && a.Object.Idcategoria == idcategoria &&
					Convert.ToDateTime(a.Object.Fecha) >= fi && Convert.ToDateTime(a.Object.Fecha) <= ff );


				foreach (var item in obj)
				{
					Suma += Convert.ToDouble(item.Object.Monto);
				}
			}
			catch (Exception)
			{

			}
			return Suma;
		}
		public async Task<double> SumarGanancias(string idcategoria, DateTime fi, DateTime ff)
		{
			double Suma = 0;
			try
			{
				var obj = (await Constantes.firebase
					.Child("Kardex")
					.OrderByKey()
					.OnceAsync<Mkardex>()).Where(a => a.Key != "models" && a.Object.Idcategoria == idcategoria &&
					Convert.ToDateTime(a.Object.Fecha) >= fi && Convert.ToDateTime(a.Object.Fecha) <= ff && a.Object.TipoKardex == "SALIDA");


				foreach (var item in obj)
				{
					Suma += Convert.ToDouble(item.Object.Ganancia);
				}
			}
			catch (Exception)
			{

			}


			//Obtengo el monto total de egresos en el rango de fechas 
			double egresos = await SumarEgresos(idcategoria, fi, ff);
			Suma = Suma - egresos;
			if (Suma < 0)
				Suma = 0;

			return Suma;
		}
		public async Task<double> CalcularStockExistente(string idcategoria, DateTime fi, DateTime ff)
		{
			double Suma = 0;
			try
			{
				var obj = (await Constantes.firebase
					.Child("Categorias")
					.OrderByKey()
					.OnceAsync<Mcategorias>()).Where(a => a.Key != "models" && a.Key == idcategoria);


				foreach (var item in obj)
				{
					Suma += Convert.ToDouble(item.Object.Stock);
				}
			}
			catch (Exception)
			{

			}

			return Suma;
		}
		public async Task<List<MresumenGeneral>> ListarResumen(string idcategoria, DateTime fi, DateTime ff)
		{

			var listResumen = new List<MresumenGeneral>();
			try
			{
				double capitalInv = await SumarCapitalProductos(idcategoria, fi, ff);
				double stockExistente = await CalcularStockExistente(idcategoria, fi, ff);
				double cantProductoInver = await SumarProductosInvertidos(idcategoria, fi, ff);
				double cantProductoVendidos = await SumarProductosVendidos(idcategoria, fi, ff);
				double VentasBrutas = await SumarVentasBrutas(idcategoria, fi, ff);
				double egresos = await SumarEgresos(idcategoria, fi, ff);
				double ganancias = await SumarGanancias(idcategoria, fi, ff);


				//Capital Invertido
				var modelsCapital = new MresumenGeneral()
				{
					Nombre = "Capital Invertido",
					Monto = "RD$ " + Constantes.AsignarComa(capitalInv),
					Icono = "https://i.ibb.co/4KXPnQm/capital.png",
					Color = "White"
				};

				//StockExistente
				var modelsStock = new MresumenGeneral()
				{
					Nombre = "Stock Existente",
					Monto = Constantes.AsignarComa(stockExistente),
					Icono = "https://i.ibb.co/DCTcMtB/almacenes.png",
					Color = "White"
				};

				//Cantidad Productos
				var modelsproductosInv = new MresumenGeneral()
				{
					Nombre = "Cantidad Productos Inv.",
					Monto = Constantes.AsignarComa(cantProductoInver),
					Icono = "https://i.ibb.co/prJJ56x/envase.png",
					Color = "White"
				};

				//Cantidad Productos Vendidos
				var modelsproductosVendidos = new MresumenGeneral()
				{
					Nombre = "Cantidad Productos Vendidos",
					Monto =  Constantes.AsignarComa(cantProductoVendidos),
					Icono = "https://i.ibb.co/Z8Cf1XY/mercado-callejero.png",
					Color = "White"
				};

				//Ventas Brutas
				var modelsVentasBruta = new MresumenGeneral()
				{
					Nombre = "Ventas Brutas",
					Monto = "RD$ " + Constantes.AsignarComa(VentasBrutas),
					Icono = "https://i.ibb.co/wSWJ59y/metodo-de-pago-1.png",
					Color = "White"
				};

				//Ventas Brutas
				var modelegresos = new MresumenGeneral()
				{
					Nombre = "Gastos Totales",
					Monto = "RD$ " + Constantes.AsignarComa(egresos),
					Icono = "https://i.ibb.co/RhfsL7d/advertencia.png",
					Color = "White"
				};

				//Ganancias 
				var modelsGanancias = new MresumenGeneral()
				{
					Nombre = "Ganancias",
					Monto = "RD$ " + Constantes.AsignarComa(ganancias),
					Icono = "https://i.ibb.co/QNfXh4W/ganador.png",
					Color = "White"
				};

				listResumen.Add(modelsStock);
				listResumen.Add(modelsproductosInv);
				listResumen.Add(modelsCapital);
				listResumen.Add(modelsproductosVendidos);
				listResumen.Add(modelsVentasBruta);
				listResumen.Add(modelsGanancias);
				listResumen.Add(modelegresos);

			}
			catch (Exception ex )
			{
			}
			

			return listResumen;
		}
	} 
}
