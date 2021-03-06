﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=391641

namespace ESAIP_Quiz
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            // this.LoadImage();
            this.GetData();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /*  void LoadImage()
          {
              Uri myUri = new Uri("http://thecatapi.com/api/images/get?format=src&type=jpg", UriKind.Absolute);
              BitmapImage bmi = new BitmapImage();
              bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
              bmi.UriSource = myUri;
              image.Source = bmi;
          }*/
        /// <summary>
        /// Récupération des données en JSON d'une API et affichage dans textbox 
        /// </summary>
        async void GetData()
        {
            try
            {
                // Remplacer url par celle de l'API 
                Uri geturi = new Uri("http://samples.openweathermap.org/data/2.5/weather?q=London,uk&appid=63b4550a02896e55ab571f88e01add13"); //replace your url  
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                System.Net.Http.HttpResponseMessage responseGet = await client.GetAsync(geturi);
                string response = await responseGet.Content.ReadAsStringAsync();
                // En Fonction du json a récuper les paramètres de l'objet ne seront pas les mêmes 
                // Dans ce cas il faut modifier lefichier WeatherObject 
                var weatherdata = JsonConvert.DeserializeObject<WeatherObject>(response);
                if (weatherdata != null)
                {
                    txt1.Text = response;
                    reponse1.Content = "The city is " + weatherdata.name.ToString();
                    reponse2.Content = "The weather is" + weatherdata.weather[0].description.ToString();
                    reponse3.Content = "The temperature is " + weatherdata.main.temp.ToString();
                }
            }
            catch (Exception ex)
            {
                var dialog = new Windows.UI.Popups.MessageDialog(ex.Message);
                await dialog.ShowAsync();
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: préparer la page pour affichage ici.
        }
        /// <summary>
        /// Lorsque le bouton est sélectionner on envoie des données json sur l'api
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Valider_Click(object sender, RoutedEventArgs e)
        {     
            var httpClient = new System.Net.Http.HttpClient();
            try
            {
                // Remplacer par l'url de l'api, où se trouve la liste des quiz
                string resourceAddress = "http://localhost:80/quiz";
                // Mettre en forme la reponse souhaité
                Reponse r=new Reponse{summary="Drapeau de f1",description="C'est une description",title="C'est un titre"};
                string postBody = JsonConvert.SerializeObject(r);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                System.Net.Http.HttpResponseMessage wcfResponse = await httpClient.PostAsync(resourceAddress, new StringContent(postBody, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                var dialog = new Windows.UI.Popups.MessageDialog(ex.Message);
                await dialog.ShowAsync();
            }
        }
    }
}

