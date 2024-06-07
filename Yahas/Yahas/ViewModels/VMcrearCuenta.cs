using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Yahas.ViewModels
{
	public  class VMcrearCuenta
	{
		public async Task CrearCuenta(string email, string pass)
		{
			var authprovider = new FirebaseAuthProvider(new FirebaseConfig(Constantes.webApy));

			await authprovider.CreateUserWithEmailAndPasswordAsync(email, pass);
		}
		public async Task<bool> Validarcuenta(string email, string pass)
		{
			try
			{
				var authprovider = new FirebaseAuthProvider(new FirebaseConfig(Constantes.webApy));
				var auth = await authprovider.SignInWithEmailAndPasswordAsync(email, pass);
				var serializartoken = JsonConvert.SerializeObject(auth);
				Preferences.Set("MyFirebaseRefreshToken", serializartoken);
				return true;
			}
			catch (Exception)
			{
				return false;

			}

		}
	}
}
