using Plugin.Geolocator;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Proyecto2.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mapa : ContentPage
    {
        double latitud, longitud;
        string label, city, country;
        public Mapa(double lat, double longi, string lab, string ciudad, string pais)
        {
            InitializeComponent();

            latitud = lat;
            longitud = longi;
            label = lab;
            city = ciudad;
            country = pais;
        }

        public async Task NavigateToBuilding25()
        {
            var location = new Xamarin.Essentials.Location(latitud, -longitud);
            var options = new MapLaunchOptions { Name = label, NavigationMode = NavigationMode.Driving };

            await Xamarin.Essentials.Map.OpenAsync(location, options);
        }

        private async void GOOGLE_Clicked(object sender, EventArgs e)
        {
            await NavigateToBuilding25();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var location = CrossGeolocator.Current;
            location.DesiredAccuracy = 50;

            if (!location.IsGeolocationEnabled || !location.IsGeolocationAvailable)
            {

                await DisplayAlert("Warning", " GPS no esta activo", "ok");
                return;
            }
            if (!location.IsListening)
            {
                await location.StartListeningAsync(TimeSpan.FromSeconds(10), 1);

                return;
            }

            Pin pin = new Pin
            {
                Label = label,
                Address = "ubicacion",
                Type = PinType.Place,
                Position = new Position(latitud, longitud)
            };
            m.Pins.Add(pin);
            m.MoveToRegion(mapSpan: MapSpan.FromCenterAndRadius(new Position(latitud, longitud), Distance.FromKilometers(1)));

        }
    }
}