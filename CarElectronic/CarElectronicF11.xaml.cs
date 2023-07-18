using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarElectronic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarElectronicF11 : ContentPage
    {
        private string urlDetOrden = "http://192.168.100.44:83/wsPrueba/api/TLRM06";
        private readonly HttpClient detOrden = new HttpClient();
        private ObservableCollection<mdlTLRM06> postTlrm06;
        private int gintCodOrden = 0;

        public CarElectronicF11(int iCodOrden)
        {
            InitializeComponent();
            gintCodOrden = iCodOrden;
            iniProceso();
        }

        public async void iniProceso()
        {
            var contenido = await detOrden.GetStringAsync(urlDetOrden+ "?iCodOrden=" + gintCodOrden);
            if (contenido != null)
            {
                //primero a lista y luego a observable collection 
                List<mdlTLRM06> datosPost = JsonConvert.DeserializeObject<List<mdlTLRM06>>(contenido);
                postTlrm06 = new ObservableCollection<mdlTLRM06>(datosPost);
                listaDetOrden.ItemsSource = postTlrm06;

            }
        }
        private void listaDetOrden_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void btnNvServicio_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CarElectronicF12(gintCodOrden));
        }
    }
}