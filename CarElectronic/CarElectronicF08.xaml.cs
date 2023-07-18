using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarElectronic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarElectronicF08 : ContentPage
    {
        private string url = "http://192.168.56.1:83/wsPrueba/api/General";
        public CarElectronicF08()
        {
            InitializeComponent();
        }

        private void btnIngresar_Clicked(object sender, EventArgs e)
        {
            try
            {
           
                WebClient digitales = new WebClient();
                var mdlTLRM08 = new System.Collections.Specialized.NameValueCollection

                {

                    { "iddigitaliza", "0" },
                    {"idorden", txtIdOrden.Text },
                    //{"iddefinicion1", txtIdDefinicion1.Text },
                    //{"iddefinicion2", txtIdDefinicion2.Text },
                    {"valdefin1", txtValdefin1.Text},    
                    {"valdefin2", txtValdefin2.Text },
                    {"pathdigitaliza", txtPathDigitaliza.Text },
                    {"codtipodigitaliza", txtCodtipoDigitaliza.Text },
                  
                };

                digitales.Headers[HttpRequestHeader.ContentType] = "application/json";
                //armar nuestro json propioro es decir define la tabla y envia el modelo 
                var data = new
                {
                    iStrIdProceso = "digitales",
                    iOrden = ToDictionary(mdlTLRM08)
                };

                string json = JsonConvert.SerializeObject(data);
                byte[] byTLRM08 = Encoding.UTF8.GetBytes(json);

                byte[] response = digitales.UploadData(url, "POST", byTLRM08);

                //recibe la respuesta del web service 
                string responseString = Encoding.UTF8.GetString(response);

                //Navigation.PushAsync(new MainPage());
                DisplayAlert("Alerta", "Ingreso :" + responseString, "Cerrar");


            }
            catch (Exception ex)
            {
                DisplayAlert("Alerta", ex.Message, "Cerrar");
            }
        }
        private static Dictionary<string, string> ToDictionary(NameValueCollection collection)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var key in collection.AllKeys)
            {
                var value = collection[key];
                dictionary.Add(key, value);
            }

            return dictionary;
        }
    }
}