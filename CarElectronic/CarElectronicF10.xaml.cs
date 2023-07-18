using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CarElectronic.CarElectronicF09;

namespace CarElectronic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarElectronicF10 : Rg.Plugins.Popup.Pages.PopupPage
    {
        private string urlParametros = "http://192.168.100.44:83/wsPrueba/api/TLRP01";
        private List<string> listMotNovedad = new List<string>();
        public CarElectronicF10()
        {
            InitializeComponent();
            obtenerDatos();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BackgroundColor = Color.White;
        }
        private void chkLstCarroceria_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
        private async void obtenerDatos()
        {
            await getJsonNovCarroceria(2, urlParametros + "?iNmonico=VHNDVCARROCERIA");
            await getJsonNovCarroceria(1, urlParametros + "?iNmonico=VHUBICACARROCERIA");
        }
        public async Task getJsonNovCarroceria(int idProceso, string url)
        {
            HttpClient generalHttp = new HttpClient();

            try
            {
                var contenido = await generalHttp.GetStringAsync(url);
                if (contenido != null)
                {
                    // dynamic deserializedObject = System.Text.Json.JsonSerializer.Deserialize<dynamic>(contenido);
                    JsonDocument deserializedObject = JsonDocument.Parse(contenido);
                    JsonElement rowArray = deserializedObject.RootElement;

                    string value1 = "";
                    string value2 = "";
                    switch (idProceso)
                    {
                        case 1:
                            var grid = new Grid();
                            // Define the grid column definitions
                            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // text column
                            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // picker column
                            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // text column

                            int i = 0;
                            foreach (JsonElement rowElement in rowArray.EnumerateArray())
                            {
                                //jior
                                if (rowElement.TryGetProperty("VALSTRING", out JsonElement property1))
                                {
                                    if (property1.ValueKind == JsonValueKind.String)
                                    {
                                        value1 = property1.GetString();

                                    }
                                }

                                if (rowElement.TryGetProperty("SERIE", out JsonElement property2))
                                {
                                    if (property2.ValueKind == JsonValueKind.String)
                                    {
                                        value2 = property2.GetString();

                                    }
                                }
                                //jior
                                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                                // Create text label
                                var textLabel = new Label
                                {
                                    Text = value2 + "-" + value1,
                                    VerticalOptions = LayoutOptions.Center
                                };
                                Grid.SetColumn(textLabel, 0);
                                Grid.SetRow(textLabel, i);

                                // Create picker
                                var picker = new Picker
                                {
                                    ItemsSource = listMotNovedad
                                };
                                picker.SelectedIndexChanged += Picker_SelectedIndexChanged;
                                Grid.SetColumn(picker, 1);
                                Grid.SetRow(picker, i);

                                // Create entry
                                var entry = new Entry
                                {
                                    Keyboard = Keyboard.Text, // Adjust the keyboard type as needed
                                    Placeholder = "Ingrese novedad"
                                };
                                Grid.SetColumn(entry, 2);
                                Grid.SetRow(entry, i);

                                // Add controls to the grid
                                grid.Children.Add(textLabel);
                                grid.Children.Add(picker);
                                grid.Children.Add(entry);

                                i = i + 1;
                            }
                            var button = new Button
                            {
                                Text = "Guardar",
                                VerticalOptions = LayoutOptions.End,
                                HorizontalOptions = LayoutOptions.Center,
                                Margin = new Thickness(0, 10)
                            };
                            button.Clicked += Button_Clicked;
                            Content = new StackLayout
                            {
                                Children = { grid,button }
                            };
                            break;
                        case 2:
                            foreach (JsonElement rowElement in rowArray.EnumerateArray())
                            {
                                //jior
                                if (rowElement.TryGetProperty("VALSTRING", out JsonElement property1))
                                {
                                    if (property1.ValueKind == JsonValueKind.String)
                                    {
                                        value1 = property1.GetString();

                                    }
                                }

                                if (rowElement.TryGetProperty("SERIE", out JsonElement property2))
                                {
                                    if (property2.ValueKind == JsonValueKind.String)
                                    {
                                        value2 = property2.GetString();

                                    }
                                }
                                listMotNovedad.Add(value2 + "-" + value1);
                            }
                            break;
                    }    
                }
            
            }
    
            catch (Exception ex)
            {
               await DisplayAlert("Alerta", ex.Message, "cerrar");
            }
        }
        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var selectedItem = (string)picker.SelectedItem;
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            // Handle button click event
            DisplayAlert("alerta", "ojo butoon" , "cierre");
        }
    }
}