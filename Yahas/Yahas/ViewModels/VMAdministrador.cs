using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Yahas.ViewModels
{
	public class VMAdministrador 
	{
		public async Task<string> SubirImagenStorage(string Idusuario, Stream image)
		{
			var rutafoto = await new FirebaseStorage(Constantes.storage)
				.Child("Administrador")
				.Child(Idusuario + ".jpg")
				.PutAsync(image);

			return rutafoto;
		}
	}
}
