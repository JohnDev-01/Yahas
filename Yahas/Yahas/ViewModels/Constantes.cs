using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yahas.ViewModels
{
	public class Constantes
	{
		public static FirebaseClient firebase = new FirebaseClient("https://yahas-5fbb7-default-rtdb.firebaseio.com/");
		public static string webApy = "AIzaSyBKdhuaZrz4JbK3WKdFWTUKPWD6ydD1Fwg";
		public static string storage = "yahas-5fbb7.appspot.com";


		public static string AsignarComa(double valor)
		{
			return String.Format("{0:#,##0.##}", valor);
		}
	}
}
