using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarElectronic
{
    public partial class App : Application
    {
        public static string DateFormat = "yyyy-MM-dd HH':'mm':'ss";

        public string fechaApp;

        public App()
        {
            InitializeComponent();
            //incluir activacion de navegacion 
            MainPage = new NavigationPage( new CarElectronicF01());

        }

        protected override void OnStart()
        {
            base.OnStart();            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        public static string GetFechaHoraAcceso()
        {
            return DateTime.Now.ToString(DateFormat);
        }
    }
}
