using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_.Net_Core.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly IRepositorioInmueble repoInmueble;
        private readonly IRepositorio<Propietario> repoPropietario;

        public InmueblesController(IRepositorioInmueble repoInmueble, IRepositorio<Propietario> repoPropietario)
        {
            this.repoInmueble = repoInmueble;
            this.repoPropietario = repoPropietario;
        }

        public ActionResult Index()
        {
            var lista = repoInmueble.ObtenerTodos();
			if (TempData.ContainsKey("Id"))
				ViewBag.Id = TempData["Id"];
			if (TempData.ContainsKey("Mensaje"))
				ViewBag.Mensaje = TempData["Mensaje"];
			return View(lista);
        }

		[Authorize(Policy = "Administrador")]
		public ActionResult Create()
		{
			ViewBag.Propietarios = repoPropietario.ObtenerTodos();
			return View();
		}

		[Authorize(Policy = "Administrador")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Inmueble entidad)
		{
            try
            {
                //if (ModelState.IsValid)
                //{
					repoInmueble.Alta(entidad);
                    TempData["Id"] = "Nuevo Inmueble agregado";
					return RedirectToAction(nameof(Index));
                //}
                //else
                //{
                //    ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                //    return View();
                //}
            }
            catch
            {
			
                ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                ViewBag.Error = "Propietario inexistente";
                //ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }

		[Authorize(Policy = "Administrador")]
		public ActionResult Delete(int id)
        {
            var entidad = repoInmueble.ObtenerPorId(id);
            return View(entidad);
        }

		[Authorize(Policy = "Administrador")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inmueble entidad)
        {
            try
            {
				repoInmueble.Baja(id);
                TempData["Id"] = "El Inmueble ha sido eliminado";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = "No es posible eliminar, contrato de alquiler vigente";
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }

		[Authorize(Policy = "Administrador")]
		public ActionResult Edit(int id)
		{
			var entidad = repoInmueble.ObtenerPorId(id);
			ViewBag.Propietarios = repoPropietario.ObtenerTodos();
			return View(entidad);
		}

		[Authorize(Policy = "Administrador")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Inmueble entidad)
		{
			try
			{
				entidad.IdInmueble = id;
				repoInmueble.Modificacion(entidad);
				TempData["Id"] = "Has modificado los datos";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.Propietarios = repoPropietario.ObtenerTodos();
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View();
			}
		}
	}
}