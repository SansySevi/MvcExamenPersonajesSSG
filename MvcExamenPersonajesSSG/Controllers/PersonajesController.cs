using Microsoft.AspNetCore.Mvc;
using MvcExamenPersonajesSSG.Models;
using MvcExamenPersonajesSSG.Services;

namespace MvcExamenPersonajesSSG.Controllers
{
    public class PersonajesController : Controller
    {
        private ServiceApiPersonajes service;

        public PersonajesController(ServiceApiPersonajes service)
        {
            this.service = service;
        }

        #region SERIES

        public async Task<IActionResult> SeriesServidor()
        {
            List<Serie> series = await this.service.GetSeriesAsync();
            return View(series);
        }

        public async Task<IActionResult> DetallesSerieServidor(int idserie)
        {
            Serie serie = await this.service.FindSerieAsync(idserie);
            return View(serie);
        }

        #endregion

        #region PERSONAJES

        public async Task<IActionResult> PersonajesSerieServidor(int idserie)
        {
            List<Personaje> personajes = await this.service.GetPersonajesSerieAsync(idserie);
            return View(personajes);
        }

        public async Task<IActionResult> DetallesPersonajeServidor(int idpersonaje)
        {
            Personaje personaje = await this.service.FindPersonajeAsync(idpersonaje);
            return View(personaje);
        }

        public async Task<IActionResult> CreatePersonaje()
        {
            List<Serie> series = await this.service.GetSeriesAsync();
            ViewData["SERIES"] = series;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreatePersonaje(string nombre, string imagen, int idserie)
        {
            await this.service.InsertPersonajeAsync(nombre, imagen, idserie);

            return RedirectToAction("PersonajesSerieServidor", new { idserie = idserie });
        }

        public async Task<IActionResult> UpdatePersonaje(int idpersonaje)
        {
            Personaje personaje = await this.service.FindPersonajeAsync(idpersonaje);
            List<Serie> series = await this.service.GetSeriesAsync();
            ViewData["SERIES"] = series;
            return View(personaje);
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePersonaje(int idpersonaje, string nombre, string imagen, int idserie)
        {
            await this.service.UpdatePersonajeAsync(idpersonaje, nombre,imagen, idserie);
            return RedirectToAction("PersonajesSerieServidor", new { idserie = idserie });
        }

        public async Task<IActionResult> DeletePersonaje(int idpersonaje)
        {

            Personaje personaje = await this.service.FindPersonajeAsync(idpersonaje);
            await this.service.DeletePersonajeAsync(idpersonaje);

            return RedirectToAction("PersonajesSerieServidor", new { idserie = personaje.IdSerie });
        }

        #endregion

    }
}
