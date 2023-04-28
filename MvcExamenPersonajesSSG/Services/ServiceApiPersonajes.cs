using MvcExamenPersonajesSSG.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcExamenPersonajesSSG.Services
{
    public class ServiceApiPersonajes
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApi;

        public ServiceApiPersonajes(IConfiguration configuration)
        {
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
            this.UrlApi = configuration.GetValue<string>
                ("ApiUrls:ApiSeries");
        }


        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        #region Gets

        public async Task<List<Serie>> GetSeriesAsync()
        {
            string request = "/api/Series";
            List<Serie> series = await CallApiAsync<List<Serie>>(request);
            return series;
        }

        public async Task<List<Personaje>> GetPersonajesAsync()
        {
            string request = "/api/Personajes";
            List<Personaje> personajes = await this.CallApiAsync<List<Personaje>>(request);
            return personajes;
        }

        public async Task<List<Personaje>> GetPersonajesSerieAsync(int idserie)
        {
            string request = "/api/Personajes/PersonajesSerie/" + idserie;
            List<Personaje> personajes = await this.CallApiAsync<List<Personaje>>(request);
            return personajes;
        }

        public async Task<Serie> FindSerieAsync(int idserie)
        {
            string request = "/api/series/" + idserie;
            Serie serie = await this.CallApiAsync<Serie>(request);
            return serie;
        }

        public async Task<Personaje> FindPersonajeAsync(int idpersonaje)
        {
            string request = "/api/personajes/" + idpersonaje;
            Personaje personaje = await this.CallApiAsync<Personaje>(request);
            return personaje;
        }

        #endregion


        #region METODOS DE ACCION

        public async Task InsertPersonajeAsync(string nombre, string imagen, int idSerie)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/personajes";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Personaje personaje = new Personaje();
                personaje.IdPersonaje = 0;
                personaje.Nombre = nombre;
                personaje.Imagen = imagen;
                personaje.IdSerie = idSerie;

                string json = JsonConvert.SerializeObject(personaje);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }

        public async Task UpdatePersonajeAsync(int idpersonaje, string nombre, string imagen, int idserie)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/personajes";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                //TENEMOS QUE ENVIAR UN OBJETO JSON
                //NOS CREAMOS UN OBJETO DE LA CLASE DEPARTAMENTO
                Personaje personaje = new Personaje
                {
                    IdPersonaje = idpersonaje,
                    Nombre = nombre,
                    Imagen = imagen,
                    IdSerie = idserie
                };

                string json = JsonConvert.SerializeObject(personaje);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);
            }

        }

        public async Task DeletePersonajeAsync(int idpersonaje)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/personajes/" + idpersonaje;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();

                HttpResponseMessage response =
                    await client.DeleteAsync(request);
            }
        }

        #endregion

    }
}
