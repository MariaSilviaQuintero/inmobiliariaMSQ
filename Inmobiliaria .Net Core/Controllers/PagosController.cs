using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_.Net_Core.Controllers
{
    public class PagosController : Controller
    {
        private readonly IRepositorio<Pago> repoPago;
        private readonly IRepositorio<Alquiler> repoAlquiler;

        public PagosController(IRepositorio<Pago> repoPago, IRepositorio<Alquiler> repoAlquiler)
        {
            this.repoPago = repoPago;
            this.repoAlquiler = repoAlquiler;
        }

        public ActionResult Index()
        {
            var lista = repoPago.ObtenerTodos();
			if (TempData.ContainsKey("Id"))
				ViewBag.Id = TempData["Id"];
			if (TempData.ContainsKey("Mensaje"))
				ViewBag.Mensaje = TempData["Mensaje"];
			return View(lista);
        }

		[Authorize(Policy = "Administrador")]
		public ActionResult Create()
		{
			ViewBag.alquiler = repoAlquiler.ObtenerTodos();
            return View();
		}

		[Authorize(Policy = "Administrador")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Pago pago)
		{
			try
			{
			    repoPago.Alta(pago);
				TempData["Id"] = "Nuevo pago ingresado";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.pago = repoPago.ObtenerTodos();
				ViewBag.alquiler = repoAlquiler.ObtenerTodos();
				ViewBag.Error = "Contrato de alquiler inexistente";
				ViewBag.StackTrate = ex.StackTrace;
				return View();
				
			}
		}

		[Authorize(Policy = "Administrador")]
		public ActionResult Delete(int id)
		{
			var entidad = repoPago.ObtenerPorId(id);
			return View(entidad);
		}

		[Authorize(Policy = "Administrador")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, Pago pago)
		{
			try
			{
				repoPago.Baja(id);
				TempData["Id"] = "El pago ha sido eliminado";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View();
			}
		}

		[Authorize(Policy = "Administrador")]
		public ActionResult Edit(int id)
		{
			var entidad = repoPago.ObtenerPorId(id);
			return View(entidad);
		}

		[Authorize(Policy = "Administrador")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Pago pago)
		{
			try
			{
				pago.IdPago = id;
				repoPago.Modificacion(pago);
				TempData["Id"] = "Has modificado los datos";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View();
			}
		}

	}
}