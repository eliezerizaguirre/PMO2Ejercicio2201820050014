using Proyecto2.clases;
using Proyecto2.model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proyecto2.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ubicaciones : ContentPage
    {
        List<UbicacionModel> service;
        Crud crud = new Crud();
        byte[] image;
        public Ubicaciones()
        {
            InitializeComponent();
      
           
            OnAppearing();

        }
        protected async override void OnAppearing()
        {

            base.OnAppearing();

            try
            {
                var ubicacionList = await crud.getReadUbicacion();


                if (ubicacionList != null)
                {
                    lista.ItemsSource = ubicacionList;
                    lista.IsRefreshing = false;
                }

            }
            catch (SQLiteException e)
            {
                await DisplayAlert("Lista", "no hay registros", "ok");

            }



        }

        private async void ShowMapa_Clicked(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(id.Text)))
            {
                await DisplayAlert("Alerta", "Seleccione una ubicacion", "ok");
                return;
            }
            bool show = await DisplayAlert("Lista de Lugares cercanos", "Ir a foresquare api", "yes", "no");
            if (show)
            {

                await Navigation.PushAsync(new ForsquareApiList(Convert.ToDouble(latitud.Text), Convert.ToDouble(longitud.Text)));
            }



        }

        private async void lista_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {


            var ubicacion = new UbicacionModel();
            var objeto = (UbicacionModel)e.SelectedItem;
            if (!string.IsNullOrEmpty(objeto.id.ToString()))
            {
                var listaSeleccionada = await crud.getUbicacionId(objeto.id);
                if (listaSeleccionada != null)
                {
                    id.Text = (listaSeleccionada.id).ToString();
                    descripcion.Text = listaSeleccionada.descripcion;
                    latitud.Text = (listaSeleccionada.latitud).ToString();
                    longitud.Text = listaSeleccionada.longitud.ToString();
                    image = listaSeleccionada.fotografia;

                }

            }

        }

        private async void Eliminar_Clicked(object sender, EventArgs e)
        {
            var ubicacion = await crud.getUbicacionId(Convert.ToInt32(id.Text));
            if ((string.IsNullOrEmpty(id.Text)))
            {
                await DisplayAlert("Alerta", "Seleccione una ubicacion para eliminar", "ok");
                return;
            }
            bool show = await DisplayAlert("Ver Mapa", "Eliminar esta ubicacion: " + descripcion.Text, "yes", "no");
            if (show)
            {

                if (ubicacion != null)
                {
                    await crud.Delete(ubicacion);
                    await DisplayAlert("Delete", "Datos Eliminados", "ok");
                    OnAppearing();
                }
                else
                {
                    await DisplayAlert("Warning", "No ha seleccionado ubicacion para borrar", "Ok");
                }


                OnAppearing();

            }

        }

        private async void actualizar_Clicked(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(id.Text)))
            {
                await DisplayAlert("Alerta", "Seleccione una ubicacion", "ok");
                return;
            }
            bool show = await DisplayAlert("Update", "Actualizar Datos", "yes", "no");
            if (show)
            {
                var getLista = new Lista
                {
                    idlista = Convert.ToInt32(id.Text),
                    latitudLista = Convert.ToDouble(latitud.Text),
                    longitudLista = Convert.ToDouble(longitud.Text),
                    descripcionLista = descripcion.Text,
                    imagenlista = image,
                 
                };
                var getMapa = new Actualizar();
                getMapa.BindingContext = getLista;

                await Navigation.PushAsync(getMapa);
            }


        }
    }
}