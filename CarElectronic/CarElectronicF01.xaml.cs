using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarElectronic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarElectronicF01 : ContentPage
    {
        public CarElectronicF01()
        {
            InitializeComponent();           
            lblFechaApp.Text= App.GetFechaHoraAcceso();
        }

        private  void btnIngresar_Clicked(object sender, EventArgs e)
        {
            string codUsuario = "1";
            string pass = "1";
            if (string.IsNullOrEmpty(txtUsuario.Text) || string.IsNullOrEmpty(txtClave.Text))
            {
                DisplayAlert("Alerta", "Usuario/Clave no ingresado", "Cerrar");
            }else if (string.Equals(txtUsuario.Text.ToString(),codUsuario,StringComparison.InvariantCultureIgnoreCase) &&
                            string.Equals(txtClave.Text.ToString(), pass, StringComparison.InvariantCultureIgnoreCase))
            {
                /*await Navigation.PushModalAsync(new CarElectronicF02(codUsuario)  
                {
                    BindingContext = this as CarElectronicF01
                });*/
                Navigation.PushAsync(new CarElectronicF02(codUsuario));
            }
            else { DisplayAlert("Alerta", "Usuario/Clave incorrecto", "Cerrar"); }

        }

        private void btnSolRegistro_Clicked(object sender, EventArgs e)
        {

        }
    }
}