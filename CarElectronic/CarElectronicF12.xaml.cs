using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class CarElectronicF12 : ContentPage
    {
        private string urlParametros = "http://192.168.100.44:83/wsPrueba/api/TLRP01";
        private string urlTLRM07= "http://192.168.100.44:83/wsPrueba/api/TLRM07";
        private string urlDetOrden = "http://192.168.100.44:83/wsPrueba/api/TLRM06";

        private string gsCodTipoServicio = "";
        private int giIdServicio = 0;
        private int gintCodOrden = 0;
        public CarElectronicF12(int iCodOrden)
        {
            InitializeComponent();
            gintCodOrden=iCodOrden;
            obtenerDatosGen();
        }



        private void btnIngresar_Clicked(object sender, EventArgs e)
        {
            try
            {
                WebClient detOrden = new WebClient();
                var mdlTLRM06 = new System.Collections.Specialized.NameValueCollection
                {

                    { "IDDETORDEN", "0" },
                    {"IDORDEN",gintCodOrden.ToString()},
                    {"IDTRBSERV", gsCodTipoServicio },
                    {"IDSERVICIO", giIdServicio.ToString()},
                    {"PRECIO", txtPrecio.Text.ToString()},
                    {"CANTIDAD", txtCantidad.Text.ToString() },
                    {"COMENTARIO", txtComentario.Text },
                    {"DIRECCIONDETORDEN", txtDirDetOrd.Text },
                    {"DIRLATITUD", txtLatitud.Text},
                    {"DIRLONGUITUD", txtLonguitud.Text },

                };

                detOrden.Headers[HttpRequestHeader.ContentType] = "application/json";
                //armar nuestro json propioro es decir define la tabla y envia el modelo 
                var data = new Dictionary<string, string>();
                foreach (string key in mdlTLRM06.AllKeys)
                {
                    data[key] = mdlTLRM06[key];
                }


                string json = JsonConvert.SerializeObject(data);
                byte[] byTLRM06 = Encoding.UTF8.GetBytes(json);

                byte[] response = detOrden.UploadData(urlDetOrden, "POST", byTLRM06);

                //recibe la respuesta del web service 
                string responseString = Encoding.UTF8.GetString(response);

                DisplayAlert("Alerta", "Proceso OK", "Cerrar");
            }
            catch (Exception ex)
            {
                DisplayAlert("Alerta", ex.Message, "Cerrar");
            }
        }

        public async void obtenerDatosGen()
        {
            await getJsonGeneral(1, urlParametros + "?iNmonico=VHTRABAJOSERVICIO");
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
                        int value2 = 0;
                        string value3 = "";
                        double value4 = 0;
                        foreach (JsonElement element in arrayElement.EnumerateArray())
                        {
                            switch (idPicker)
                            {
                                case 1:                            // Access specific properties within each array element
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
                                            value3 = property2.GetString();

                                        }
                                    }

                                    pckTipoServicio.Items.Add(value3.ToString() + " - " + value1.ToString());
                                    break;
                                case 2:
                                    if (element.TryGetProperty("IDSERVICIO", out JsonElement property3))
                                    {
                                        if (property3.ValueKind == JsonValueKind.Number)
                                        {
                                            value2 = property3.GetInt32();

                                        }
                                    }

                                    if (element.TryGetProperty("NMONICOSERVICIO", out JsonElement property4))
                                    {
                                        if (property4.ValueKind == JsonValueKind.String)
                                        {
                                            value1 = property4.GetString();

                                        }
                                    }
                                    if (element.TryGetProperty("COSTOVENTA", out JsonElement property5))
                                    {
                                        if (property5.ValueKind == JsonValueKind.Number)
                                        {
                                            if (property5.TryGetDouble(out double value5))
                                            {
                                                value4 = value5;
                                            }

                                        }
                                    }
                                    pckServicio.Items.Add(value2.ToString()+"-"+value1.ToString()+"-"+value4.ToString());
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

        private async void pckTipoServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] datTipoServicio;
            datTipoServicio = pckTipoServicio.SelectedItem.ToString().Split('-');
            gsCodTipoServicio = datTipoServicio[0];

            await getJsonGeneral(2, urlTLRM07 + "?iCodTipoSrv=" + gsCodTipoServicio);
        }

        private void pckServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] datServicio;
            datServicio = pckServicio.SelectedItem.ToString().Split('-');

            giIdServicio = Convert.ToInt32(datServicio[0]);
            double costoVenta = Convert.ToDouble( datServicio[2]);
            txtPrecio.Text = costoVenta.ToString();
        }
    }
}