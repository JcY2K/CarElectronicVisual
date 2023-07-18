using CarElectronic;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class CarElectronicF09 : Rg.Plugins.Popup.Pages.PopupPage
    {
        private string urlParametros = "http://192.168.100.44:83/wsPrueba/api/TLRP01";
        private string urlTLRM08 = "http://192.168.100.44:83/wsPrueba/api/TLRM08";
        private int gintOrden = 0;

        public CarElectronicF09(int iCodOrden)
        {
            InitializeComponent();
            gintOrden = iCodOrden;
            obtenerDatos();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BackgroundColor =Color.White;   
        }
        private async void  obtenerDatos()
        {
            //VHDSPAUGENERAL
 
            await getJsonVhGeneral(1, urlParametros + "?iNmonico=VHDSPAUGENERAL");

        }
        public async Task getJsonVhGeneral(int idPicker, string url)
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
                        string value2 = "";
                        

                        List<CheckListItem> datosCheckList = new List<CheckListItem>();

                        foreach (JsonElement element in arrayElement.EnumerateArray())
                        {
                            if (element.TryGetProperty("VALSTRING", out JsonElement property1))
                            {
                                if (property1.ValueKind == JsonValueKind.String)
                                {
                                    value1 = property1.GetString();

                                }
                            }

                            if (element.TryGetProperty("SERIE", out JsonElement property2))
                            {
                                if (property2.ValueKind == JsonValueKind.String)
                                {
                                    value2 = property2.GetString();

                                }
                            }

                            CheckListItem item = new CheckListItem
                            {
                                IsChecked = false, // Set the initial checkbox state as needed
                                Text = value2 + "-" + value1
                            };
                            datosCheckList.Add(item);
                        }//foreach
                        chkLstOpt.ItemsSource = datosCheckList;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alerta", ex.Message, "cerrar");
            }
        }
        public class CheckListItem
        {
            public bool IsChecked { get; set; }
            public string Text { get; set; }
        }
        private void chkLstOpt_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void btnExisteDef_Clicked(object sender, EventArgs e)
        {
            try
            {
                WebClient detCabOrden = new WebClient();

                foreach (var item in chkLstOpt.ItemsSource)
                {
                    if (item is CheckListItem itemClass && itemClass.IsChecked)
                    {
                        string[] datParam = itemClass.Text.Split('-');
                        if (datParam.Length >0)
                        {
                            NameValueCollection mdlTLRM08 = new System.Collections.Specialized.NameValueCollection();
                            
                            mdlTLRM08.Add("IDDIGITALIZA", "0");
                            mdlTLRM08.Add("IDORDEN", gintOrden.ToString());
                            mdlTLRM08.Add("IDDEFINICION1", "VHDSPAUGENERAL");
                            mdlTLRM08.Add("IDDEFINICION2", "");
                            mdlTLRM08.Add("VALDEFIN1", datParam[0]);
                            mdlTLRM08.Add("VALDEFIN2", "");
                            mdlTLRM08.Add("COMENTARIO", "");
                            mdlTLRM08.Add("PATHDIGITALIZA", "");
                            mdlTLRM08.Add("CODTIPODIGITALIZA", "");
                        

                            detCabOrden.Headers[HttpRequestHeader.ContentType] = "application/json";
                            //armar nuestro json propioro es decir define la tabla y envia el modelo 
                            var dictionary = new Dictionary<string, string>();
                            foreach (string key in mdlTLRM08.AllKeys)
                            {
                                dictionary[key] = mdlTLRM08[key];
                            }

                            string json = JsonConvert.SerializeObject(dictionary);

                            byte[] byTLRM08 = Encoding.UTF8.GetBytes(json);

                            byte[] response = detCabOrden.UploadData(urlTLRM08, "POST", byTLRM08);

                            //recibe la respuesta del web service 
                            string responseString = Encoding.UTF8.GetString(response);
                            /*
                            string[] cadRpta = responseString.Split('|');
                            if (cadRpta.Length > 0)
                            {
                                if (cadRpta[0] == "ok")
                                {
                                    gintCodOrden = Convert.ToInt32(cadRpta[1]);
                                }
                            }*/
                            //Navigation.PushAsync(new MainPage());
                            await DisplayAlert("Alerta", "Ingreso :" + responseString, "Cerrar");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alerta", ex.Message, "Cerrar");
            }

            // List<string>  existeLista = new List<string>();
            // 
            // 
            // foreach (var item in chkLstOpt.ItemsSource)
            // {
            //     if (item is CheckListItem itemClass && itemClass.IsChecked)
            //     {
            //         existeLista.Add(itemClass.Text);
            //     }
            // }

            //listaRetorna?.Invoke(this, existeLista);
            // Close the floating screen
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}