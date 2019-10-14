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
    public class InquilinosController : Controller
    {
		private readonly IRepositorio<Inquilino> repositorio;

		public InquilinosController(IRepositorio<Inquilino> repositorio)
		{
			this.repositorio = repositorio;
		}

        public ActionResult Index()
        {
			var lista = repositorio.ObtenerTodos();
			if (TempData.ContainsKey("Id"))
				ViewBag.Id = TempData["Id"];
			if (TempData.ContainsKey("Mensaje"))
				ViewBag.Mensaje = TempData["Mensaje"];
			return View(lista);
        }

		[Authorize(Policy = "Administrador")]
		public ActionResult Create()
        {
            return View();
        }

		[Authorize(Policy = "Administrador")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino)
        {
			ViewBag.inquilinos = repositorio.ObtenerTodos();

			foreach (var item in (IList<Inquilino>)ViewBag.inquilinos)
			{
				if (item.Dni == inquilino.Dni)
				{
					ViewBag.Mensaje = "Inquilino existente";
					return View();
				}
			}

			try
			{
				if (ModelState.IsValid)
				{
					repositorio.Alta(inquilino);
					TempData["Id"] = "Nuevo inquilino ingresado";
					return RedirectToAction(nameof(Index));
				}
				else
					return View();	
			}
			catch(Exception ex)
			{
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View();
			}
					
        }

		[Authorize(Policy = "Administrador")]
		public ActionResult Edit(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            return View(entidad);
        }

		[Authorize(Policy = "Administrador")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inquilino i = null;
            try
            {
                i = repositorio.ObtenerPorId(id);
                i.Nombre = collection["Nombre"];
                i.Apellido = collection["Apellido"];
                i.Dni = collection["Dni"];
                i.Direccion = collection["Direccion"];
                i.Telefono = collection["Telefono"];
                repositorio.Modificacion(i);
				TempData["Id"] = "Has modificado los datos";
				return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(i);
            }
        }

		[Authorize(Policy = "Administrador")]
		public ActionResult Delete(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            return View(entidad);
        }

		[Authorize(Policy = "Administrador")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inquilino inquilino)
        {
            try
            {
                repositorio.Baja(id);
				TempData["Id"] = "El Inquilino ha sido eliminado";
				return RedirectToAction(nameof(Index));
			}
            catch (Exception ex)
            {
                ViewBag.Error = "Inquilino con alquiler vigente";
                ViewBag.StackTrate = ex.StackTrace;
				return View();
			}
        }
    }
}