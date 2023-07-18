using Newtonsoft.Json;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel.Design;

namespace CarElectronic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarElectronicF06 : ContentPage
    {
        private string urlVehiculo = "http://192.168.100.44:83/wsPrueba/api/TLRM04";
        private string urlCliente = "http://192.168.100.44:83/wsPrueba/api/General";
        private string urlParametros = "http://192.168.100.44:83/wsPrueba/api/TLRP01";

        private int giCodCliente = 0;
        private string gsMarca = "";
        private string gsModelo = "";
        private string gsTipCombus = "";

        //?iStrIdProceso='cliente'

        private readonly HttpClient getCliente = new HttpClient();
        public CarElectronicF06()
        {
            InitializeComponent();
            cargaDatGeneral();
        }

        private void btnIngresar_Clicked(object sender, EventArgs e)
        {
            try
            {
                DateTime selecttedDate = FecCompra.Date;
                string formattedDate = selecttedDate.ToString("yyyyMMdd");

                WebClient vehiculo = new WebClient();
                var mdlTLRM04 = new System.Collections.Specialized.NameValueCollection

                {
                    { "idVehiculo", "0" },
                    {"idcliente",giCodCliente.ToString() },
                    { "codMarca", gsMarca},
                    { "codModelo", gsModelo},
                    { "ANIOFABRICA", txtAñoFabrica.Text },
                    { "paisOrigen", txtPaisOrigen.Text },
                    { "cilindraje", txtCilindraje.Text },
                    { "color", txtColor.Text },
                    { "CODTIPOCOMBUSTIBLE", gsTipCombus},
                    { "snAsegurado", txtSnAsegurado.Text },
                    { "fechaCompra", formattedDate },
                    { "placa", txtPlaca.Text },

                };

                vehiculo.Headers[HttpRequestHeader.ContentType] = "application/json";
                //armar nuestro json propioro es decir define la tabla y envia el modelo 
                var data = new
                {
                    iStrIdProceso = "vehiculo",
                    IVehiculo = ToDictionary(mdlTLRM04)
                };

                string json = JsonConvert.SerializeObject(data);
                byte[] byTLRM04 = Encoding.UTF8.GetBytes(json);

                byte[] response = vehiculo.UploadData(urlVehiculo, "POST", byTLRM04);

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

        //private void txtCodModelo_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtCodModelo.Text.Trim()))
        //    {
        //        DisplayAlert("Alerta", "Debe ingresar un Modelo", "Cerrar");
        //    }
        //
        //}
        //aaaa
        //20230718 private void txtCodMarca_TextChanged(object sender, TextChangedEventArgs e)
        //20230718 {
        //20230718     if (string.IsNullOrEmpty(txtCodMarca.Text.Trim()))
        //20230718     {
        //20230718         DisplayAlert("Alerta", "Debe ingresar una Marca", "Cerrar");
        //20230718     }
        //20230718 }
        public async Task getJsonGeneral(int idProceso, string url,int iWhPick)
        {
            HttpClient generalHttp = new HttpClient();
            try
            {
                var contenido = await generalHttp.GetStringAsync(url);
                if (contenido != null)
                {
                    // dynamic deserializedObject = System.Text.Json.JsonSerializer.Deserialize<dynamic>(contenido);
                    JsonDocument deserializedObject = JsonDocument.Parse(contenido);

                    JsonElement arrayElement = deserializedObject.RootElement;

                    if (arrayElement.ValueKind == JsonValueKind.Array)
                    {
                        string value1 = "";
                        int value2 = 0;
                        string value3 = "";
                        foreach (JsonElement element in arrayElement.EnumerateArray())
                        {
                            switch (idProceso)
                            {
                                case 1:                            // Access specific properties within each array element
                                    if (element.TryGetProperty("NOMBRECORTO", out JsonElement property1))
                                    {
                                        if (property1.ValueKind == JsonValueKind.String)
                                        {
                                            value1 = property1.GetString();

                                        }
                                    }

                                    if (element.TryGetProperty("IDCLIENTE", out JsonElement property2))
                                    {
                                        if (property2.ValueKind == JsonValueKind.Number)
                                        {
                                            value2 = property2.GetInt32();

                                        }
                                    }

                                    pckClientes.Items.Add(value2.ToString() + " - " + value1.ToString());
                                    break;
                                case 2:
                                    // Access specific properties within each array element
                                    if (element.TryGetProperty("VALSTRING", out JsonElement property5))
                                    {
                                        if (property5.ValueKind == JsonValueKind.String)
                                        {
                                            value1 = property5.GetString();

                                        }
                                    }

                                    if (element.TryGetProperty("SERIE", out JsonElement property6))
                                    {
                                        if (property6.ValueKind == JsonValueKind.String)
                                        {
                                            value3 = property6.GetString();

                                        }
                                    }
                                    if (iWhPick == 1)
                                    {
                                        pckMarcas.Items.Add(value3.ToString() + " - " + value1.ToString());
                                    }
                                    else if (iWhPick == 2)
                                    {
                                        pckModelo.Items.Add(value3.ToString() + " - " + value1.ToString());
                                    }
                                    else 
                                    {
                                        pckTipCombustible.Items.Add(value3.ToString() + " - " + value1.ToString());
                                    }
                                    
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alerta", ex.Message, "cerrar");
            }
        }
        private async void cargaDatGeneral()
        {
            await getJsonGeneral(1,urlCliente,0);
            await getJsonGeneral(2, urlParametros + "?iNmonico=VHMARCAS",1);
            await getJsonGeneral(2, urlParametros + "?iNmonico=VHTIPOCOMBUSTIBLE",3);
        }

        private void pckClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] datCliSelect;
            datCliSelect = pckClientes.SelectedItem.ToString().Split('-');
            giCodCliente = Convert.ToInt32(datCliSelect[0]);
        }

        private async void pckMarcas_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] datMarca;
            datMarca = pckMarcas.SelectedItem.ToString().Split('-');
            gsMarca = datMarca[0];

            await getJsonGeneral(2, urlParametros + "?iNmonico=VHMODELOS&iCodSecuencia=" + gsMarca,2);
        }
    

        private void pckModelo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] datModelo;
            datModelo = pckModelo.SelectedItem.ToString().Split('-');
            gsModelo = datModelo[0].ToString();
        }

        private void pckTipCombustible_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] tipCombustible;
            tipCombustible = pckTipCombustible.SelectedItem.ToString().Split('-');
            gsTipCombus = tipCombustible[0];
        }
    }
}