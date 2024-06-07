using Firebase.Auth;
using Firebase.Database.Query;
using Firebase.Storage;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Yahas.Models;

namespace Yahas.ViewModels
{
	public class VMnegocios
	{
		public VMnegocios()
		{


		}
		public async Task InsertarNegocios(Mnegocios models)
		{
			await Constantes.firebase.Child("Negocios")
				.PostAsync(models);
		}
		public async Task<Mnegocios> MostrarInformacionNegocio()
		{
			var idusuario = await ObtenerIdusuario();
			var obj = (await Constantes.firebase
					.Child("Negocios")
					.OrderByKey()
					.OnceAsync<Mnegocios>()).Where(a => a.Object.idusuario == idusuario);
			var models = new Mnegocios();
			foreach (var item in obj)
			{
				models = item.Object;
			}
			return models;
		}
		public async Task EditarInformacion(Mnegocios models)
		{
			try
			{
				var idusuario = await ObtenerIdusuario();
				var obj = (await Constantes.firebase
						.Child("Negocios")
						.OrderByKey()
						.OnceAsync<Mnegocios>()).Where(a => a.Object.idusuario == idusuario).FirstOrDefault();
				obj.Object.nombre = models.nombre;
				obj.Object.foto = models.foto;


				await Constantes.firebase
					.Child("Negocios")
					.Child(obj.Key)
					.PutAsync(obj.Object);
			}
			catch (Exception ex)
			{
			}

		}
		public async Task<string> EditarFotoPerfilStorage(MediaFile file)
		{
			string idusuario = await ObtenerIdusuario();
			string rutafoto = "";
			try
			{
				await new FirebaseStorage(Constantes.storage)
					.Child("Administrador")
					.Child(idusuario + ".jpg")
					.DeleteAsync();
			}
			catch (Exception ex )
			{
			}

			var funcion = new VMAdministrador();
			rutafoto = await funcion.SubirImagenStorage(idusuario, file.GetStream());
			return rutafoto;
		}
		public async Task<string> ObtenerIdusuario()
		{
			string idusuario;
			try
			{
				var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constantes.webApy));
				var guardarId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
				var RefrescarContenido = await authProvider.RefreshAuthAsync(guardarId);
				Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefrescarContenido));
				idusuario = guardarId.User.LocalId;
			}
			catch (Exception)
			{
				idusuario = "";
			}
			return idusuario;

		}
	}
}