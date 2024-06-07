using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Yahas.Views.Menu
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Contenedor : TabbedPage
	{
		public Contenedor()
		{
			InitializeComponent();
		}
	}
}