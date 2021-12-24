using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Proyecto2.clases;
using Proyecto2.model;
using Proyecto2.views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Proyecto2
{
    public partial class MainPage : ContentPage
    {
        List<UbicacionModel> service;
      
        double dlatitud, dlongitud;
        byte[] image;
        public MainPage()
        {
            InitializeComponent();
            


        }

        protected override void OnAppearing()
        {

            base.OnAppearing();
            locationGPS();
        }

        public async void locationGPS()
        {

            var location = CrossGeolocator.Current;
            location.DesiredAccuracy = 50;

            if (!location.IsGeolocationEnabled || !location.IsGeolocationAvailable)
            {

                await DisplayAlert("Warning", " GPS no esta activo", "ok");

            }
            else
            {
                if (!location.IsListening)
                {
                    await location.StartListeningAsync(TimeSpan.FromSeconds(10), 1);


                }
                location.PositionChanged += (posicion, args) =>
                {
                    var ubicacion = args.Position;
                    latitud.Text = ubicacion.Latitude.ToString();
                    dlatitud = Convert.ToDouble(latitud.Text);
                    longitud.Text = ubicacion.Longitude.ToString();
                    dlongitud = Convert.ToDouble(longitud.Text);
                };

            }

        }

        public async void guardarUbicacion()
        {

            if (string.IsNullOrEmpty(descripcion_larga.Text))
            {
                await DisplayAlert("Alerta", "Debe describir la ubicacion", "ok");
                return;
            }
            if (imagefile.Source == null)
            {
                await DisplayAlert("Alerta", "Seleccione Imagen", "ok");
                return;
            }


            else
            {

                Crud crud = new Crud();
                Conexion conn = new Conexion();

                var ubicacion = new UbicacionModel()
                {
                    id = 0,
                    latitud = Convert.ToDouble(latitud.Text),
                    longitud = Convert.ToDouble(longitud.Text),
                    descripcion = descripcion_larga.Text,
                    fotografia = image

                };
                refresc.IsRunning = true;
                conn.Conn().CreateTable<UbicacionModel>();
                conn.Conn().Insert(ubicacion);
                await DisplayAlert("Success", "Ubicacion Guardada", "Ok");
                descripcion_larga.Text = "";
             
             

                    refresc.IsRunning = false;
                


            }


        }
    
       
        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Ubicaciones());
        }

        private async void buttoncamera_Clicked(object sender, EventArgs e)
        {
            var camera = new StoreCameraMediaOptions();
            camera.PhotoSize = PhotoSize.Full;
            camera.Name = "img";
            camera.Directory = "MiApp";


            var foto = await CrossMedia.Current.TakePhotoAsync(camera);


            if (foto != null)
            {

                imagefile.Source = ImageSource.FromStream(() => {

                    return foto.GetStream();



                });
                imagefile.IsVisible = true;
                using (MemoryStream memory = new MemoryStream())
                {

                    Stream stream = foto.GetStream();
                    stream.CopyTo(memory);
                    image = memory.ToArray();
                }
            }
        }

        private void Salvar_Clicked(object sender, EventArgs e)
        {
            guardarUbicacion();

        }
    }
}