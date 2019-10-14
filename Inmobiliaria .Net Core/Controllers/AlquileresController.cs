using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_.Net_Core.Controllers
{
	public class AlquileresController : Controller
	{
		private readonly IRepositorio<Alquiler> repositorio;
		private readonly IRepositorioInmueble repoInmueble;
		private readonly IRepositorio<Inquilino> repoInquilino;

		public AlquileresController(IRepositorio<Alquiler> repositorio, IRepositorioInmueble repoInmueble, IRepositorio<Inquilino> repoInquilino)
		{
			this.repositorio = repositorio;
			this.repoInmueble = repoInmueble;
			this.repoInquilino = repoInquilino;
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
			ViewBag.inmueble = repoInmueble.BuscarDisponibles();
			ViewBag.inquilino = repoInquilino.ObtenerTodos();
			return View();
		}

		[Authorize(Policy = "Administrador")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Alquiler alquiler)
		{
			try
			{

				if (ModelState.IsValid)
				{
					repositorio.Alta(alquiler);
					TempData["Id"] = "Contrato de alquiler agregado correctamente";
					return RedirectToAction(nameof(Index));
				}
				else
				{
					ViewBag.inmueble = repoInmueble.BuscarDisponibles();
					ViewBag.inquilino = repoInquilino.ObtenerTodos();
					return View();
				}
				//}
				//else
				//{
				//	TempData["Mensaje"] = "Inmueble y/o Inquilino inexistente";
				//	return RedirectToAction(nameof(Index));
				//}
			}
			catch (Exception ex)
			{
				ViewBag.inmueble = repoInmueble.BuscarDisponibles();
				ViewBag.inquilino = repoInquilino.ObtenerTodos();
				ViewBag.Error = "Inmueble y/o Inquilino inexistente";
				ViewBag.StackTrate = ex.StackTrace;
				return View();

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
		public ActionResult Delete(int id, Alquiler entidad)
		{
			try
			{

				repositorio.Baja(id);
				TempData["Id"] = "Se eliminó correctamente";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.Error = "Hay pagos relacionados a este alquiler";
				ViewBag.StackTrate = ex.StackTrace;
				return View();
			}
		}

		[Authorize(Policy = "Administrador")]
		public ActionResult Edit(int id)
		{
			var entidad = repositorio.ObtenerPorId(id);
			ViewBag.inmueble = repoInmueble.BuscarDisponibles();
			ViewBag.inquilino = repoInquilino.ObtenerTodos();
			return View(entidad);
		}

		[Authorize(Policy = "Administrador")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Alquiler entidad)
		{
			try
			{
				entidad.IdAlquiler = id;
				repositorio.Modificacion(entidad);
				TempData["Id"] = "Datos modficados con exito!";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.inmueble = repoInmueble.BuscarDisponibles();
				ViewBag.inquilino = repoInquilino.ObtenerTodos();
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View();
			}
		}
	}
}