using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarElectronic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarElectronicF07 : ContentPage
    {
        private string urlCabOrden = "http://192.168.100.44:83/wsPrueba/api/TLRM05";
        private string url = "http://192.168.100.44:83/wsPrueba/api/TLRM04";
        private string urlCliente = "http://192.168.100.44:83/wsPrueba/api/General";
        private string urlParametros = "http://192.168.100.44:83/wsPrueba/api/TLRP01";

        private int gnCodVehiculo;
        private string gCodTipoServicio;
        private string gCodCliente;
        private int gintCodOrden;
        public CarElectronicF07()
        {
            InitializeComponent();
            cargaDatGeneral();
        }

        private void btnIngresar_Clicked(object sender, EventArgs e)
        {
            try
            {
                DateTime selecttedDate = FecIngreso.Date;
                string formattedDate = selecttedDate.ToString("yyyyMMdd");

                WebClient orden = new WebClient();
                var mdlTLRM05 = new System.Collections.Specialized.NameValueCollection

                {

                    { "idOrden", "0" },
                    {"idvehiculo",gnCodVehiculo.ToString()},
                    {"idusuario", "100" },
                    {"estatus", "A" },
                    {"fecha", formattedDate},
                    {"horainicio", txtHora.Text },
                    {"kilometraje", txtKilometraje.Text },
                    {"porcentajecomb", txtPorcentajeComb.Text },
                    {"codtiposervicio", gCodTipoServicio},
                    {"observacion", txtObservacion.Text },
                    {"recomendacion", txtRecomendacion.Text },
                    {"celulartercero", txtCelular.Text },
                    {"fecha_cierre", formattedDate},

                };

                orden.Headers[HttpRequestHeader.ContentType] = "application/json";
                //armar nuestro json propioro es decir define la tabla y envia el modelo 
                var data = new
                {
                    iStrIdProceso = "orden",
                    iCabOrden = ToDictionary(mdlTLRM05)
                };

                string json = JsonConvert.SerializeObject(data);
                byte[] byTLRM05 = Encoding.UTF8.GetBytes(json);

                byte[] response = orden.UploadData(urlCabOrden, "POST", byTLRM05);

                //recibe la respuesta del web service 
                string responseString = Encoding.UTF8.GetString(response);

                char charToReplace = '\\';
                char replacementChar = ' ';

                string modifiedText = responseString.Replace(charToReplace, replacementChar);

                string[] cadRpta = modifiedText.Split('|');
                if (cadRpta.Length > 0)
                {
                    string searchWord = "True";
                    int charIndex = modifiedText.IndexOf(searchWord);

                    if (charIndex >= 0)
                    {
                        gintCodOrden = Convert.ToInt32(cadRpta[1]);
                        //Navigation.PushAsync(new MainPage());
                        DisplayAlert("Alerta", "Proceso realizado con Exito", "Cerrar");
                    }
                }
                else
                {
                    DisplayAlert("Alerta", "Error: Inserción Orden", "Cerrar");
                }
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

        private void btnConsultar_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new CarElectronicF09());
        }
        public async Task getJsonClientes(int idPicker,string url)
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
                        string value1="";
                        int value2=0;
                        string value3 = "";
                        foreach (JsonElement element in arrayElement.EnumerateArray())
                        {
                            switch (idPicker)
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
                                    if (element.TryGetProperty("PLACA", out JsonElement property3))
                                    {
                                        if (property3.ValueKind == JsonValueKind.String)
                                        {
                                            value1 = property3.GetString();

                                        }
                                    }

                                    if (element.TryGetProperty("IDVEHICULO", out JsonElement property4))
                                    {
                                        if (property4.ValueKind == JsonValueKind.Number)
                                        {
                                            value2 = property4.GetInt32();

                                        }
                                    }

                                    pckVehiculos.Items.Add(value2.ToString() + " - " + value1.ToString());
                                    break;
                                case 3:
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

                                    pckTipoServicio.Items.Add(value3.ToString() + " - " + value1.ToString());
                                    break;
                            }//switch
                        }//foreach
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alerta", ex.Message,"cerrar");
            }
        }

        private async void cargaDatGeneral()
        {
            await getJsonClientes(1,urlCliente);
            await getJsonClientes(3, urlParametros+ "?iNmonico=GETIPOSERVICIO");
        }

        private async void pckClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string [] datCliente;
            datCliente = pckClientes.SelectedItem.ToString().Split('-');
            gCodCliente = datCliente[0];
            await getJsonClientes(2, url+"?iCodVehiculo=0&iCodCliente="+datCliente[0]);
        }

        private void pckVehiculos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] datVehiculo;
            datVehiculo = pckVehiculos.SelectedItem.ToString().Split('-');
            gnCodVehiculo = Convert.ToInt32( datVehiculo[0]);
        }

        private void pckTipoServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] datTipServicio;
            datTipServicio = pckTipoServicio.SelectedItem.ToString().Split('-');
            gCodTipoServicio = datTipServicio[0];
        }

        private async void btnQTiene_Clicked(object sender, EventArgs e)
        {
            //valido var  frmExiste = new CarElectronicF09();
            //valido 
            //valido var closeButton = new Label
            //valido {
            //valido     Text = "Cierre",
            //valido     //GestureRecognizers = { closeGesture },
            //valido     HorizontalOptions = LayoutOptions.EndAndExpand,
            //valido };
            //valido 
            //valido var container = new ContentView
            //valido {
            //valido     Content = new StackLayout
            //valido     {
            //valido         Children = { frmExiste.Content, closeButton }
            //valido     }
            //valido };
            //valido 
            //valido 
            //valido var closeGesture = new TapGestureRecognizer();
            //valido closeGesture.Tapped += async (closeSender, closeE) =>
            //valido {
            //valido     mainLayout.Children.Remove(container);
            //valido };
            //valido 
            //valido closeButton.GestureRecognizers.Add(closeGesture);
            //valido 
            //valido mainLayout.Children.Add(container);
            await PopupNavigation.Instance.PushAsync(new CarElectronicF09(gintCodOrden));

           
        }
        private async void btnNovCarr_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new CarElectronicF10());
        }

        private void btnDetOrd_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CarElectronicF11(gintCodOrden));
        }
        //private async void ShowPopUp(object sender, EventArgs e)
        //{
        //    
        //}
    }
}