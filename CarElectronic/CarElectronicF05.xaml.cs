using Newtonsoft.Json;
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

    public partial class CarElectronicF05 : ContentPage
    {
        
        private string url = "http://192.168.56.1:83/wsPrueba/api/General";
        private string urlParametros = "http://192.168.100.44:83/wsPrueba/api/TLRP01";
        private string codTipoId = "";
        public CarElectronicF05()
        {
            InitializeComponent();
            cargaDatGeneral();
        }

        private void btnIngresar_Clicked(object sender, EventArgs e)
        {
            try
            {
                DateTime selectedDate = dtpFecNac.Date;
                string formattedDate = selectedDate.ToString("yyyyMMdd");

                WebClient cliente = new WebClient();
                var mdlTLRM03 = new System.Collections.Specialized.NameValueCollection
                {
                    { "idCliente", "0" },
                    { "tipoidCliente", codTipoId },
                    { "identificacion", txtIdentificacion.Text },
                    { "primer_Nombre", txtPrimNombre.Text },
                    { "segundo_Nombre", txtSegNombre.Text },
                    { "primer_Apellido", txtPrimApellido.Text },
                    { "segundo_Apellido", txtSegApellido.Text },
                    { "nombreCorto", txtNomCorto.Text },
                    { "correo", txtCorreo.Text },
                    { "celular", txtCelular.Text },
                    { "convencional", txtConvencional.Text },
                    { "fechaNacimiento", formattedDate}
                };

                cliente.Headers[HttpRequestHeader.ContentType] = "application/json";
                //armar nuestro json propioro es decir define la tabla y envia el modelo 
                var data = new
                {
                    iStrIdProceso = "cliente",
                    iCliente = ToDictionary(mdlTLRM03)
                };

                string json = JsonConvert.SerializeObject(data);
                byte[] byTLRM03 = Encoding.UTF8.GetBytes(json);

                byte[] response = cliente.UploadData(url, "POST", byTLRM03);
                
                //recibe la respuesta del web service 
                string responseString = Encoding.UTF8.GetString(response);
                if (responseString =="true")
                //Navigation.PushAsync(new MainPage());
                    DisplayAlert("Alerta", "Ingreso :" + responseString, "Cerrar");
                else
                {
                    DisplayAlert("Alerta", "ERROR:" + responseString, "Cerrar");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alerta", ex.Message, "Cerrar");
            }
        }
        //
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

        private void txtPrimApellido_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPrimApellido.Text.Trim()))
            {
                DisplayAlert("Alerta", "Debe ingresar Primer Apellido/Primer Nombre", "Cerrar");
            }
            else
            {
                if (string.IsNullOrEmpty(txtPrimNombre.Text.Trim()))
                {
                    DisplayAlert("Alerta", "Debe ingresar Primer Nombre", "Cerrar");                    
                }
                else {
                    txtNomCorto.Text = txtPrimNombre.Text.Trim() + " " + txtPrimApellido.Text.Trim();
                }
            }
        }

        private void txtPrimNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPrimNombre.Text.Trim()))
            {
                DisplayAlert("Alerta", "Debe ingresar Primer Nombre", "Cerrar");
            }
        }

        private void pckTipoId_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] datTipDoc = pckTipoId.SelectedItem.ToString().Split('-'); ;
            codTipoId = datTipDoc[0];
        }
        private async void cargaDatGeneral()
        {            
            await getJsonGeneral(1, urlParametros + "?iNmonico=TIPODOCCLIENTE");
        }
        public async Task getJsonGeneral(int idPicker, string url)
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
                        string value3 = "";
                        foreach (JsonElement element in arrayElement.EnumerateArray())
                        {
                            switch (idPicker)
                            {
                                case 1:
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

                                    pckTipoId.Items.Add(value3.ToString() + " - " + value1.ToString());
                                    break;
                            }//switch
                        }//foreach
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alerta", ex.Message, "cerrar");
            }
        }

    }
}