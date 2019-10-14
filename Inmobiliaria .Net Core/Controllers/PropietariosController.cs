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
	public class PropietariosController : Controller
	{
		private readonly IRepositorio<Propietario> repositorio;

		public PropietariosController(IRepositorio<Propietario> repositorio)
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

		
		public ActionResult Create()
		{
			return View();
		}

		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Propietario propietario)
		{
			ViewBag.propietarios = repositorio.ObtenerTodos();

			foreach (var item in (IList<Propietario>)ViewBag.propietarios)
			{
				if (item.Email == propietario.Email || item.Dni == propietario.Dni)
				{
					ViewBag.Mensaje = "Email o DNI existente";
					return View();
				}
			}

			try
			{
				TempData["Nombre"] = propietario.Nombre;
				if (ModelState.IsValid)
				{
					propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: propietario.Clave,
						salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 1000,
						numBytesRequested: 256 / 8));
					repositorio.Alta(propietario);
					TempData["Id"] = "Nuevo propietario ingresado";
					return RedirectToAction(nameof(Index));
				}
				else
					return View();
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View();
			}
		}

		//[HttpPost]
		//public JsonResult Buscar(string s)
		//{
		//	var res = repositorio.ObtenerTodos().Where(x => x.Nombre.Contains(s) || x.Apellido.Contains(s));
		//	return new JsonResult(res);
		//}

		[Authorize(Policy = "Administrador")]
		public ActionResult Edit(int id)
		{
			var p = repositorio.ObtenerPorId(id);
			return View(p);
		}

		[Authorize(Policy = "Administrador")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			Propietario p = null;
			try
			{
				p = repositorio.ObtenerPorId(id);
				p.Nombre = collection["Nombre"];
				p.Apellido = collection["Apellido"];
				p.Dni = collection["Dni"];
				p.Email = collection["Email"];
				p.Telefono = collection["Telefono"];
				repositorio.Modificacion(p);
				TempData["Id"] = "Has modificado los datos";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View(p);
			}
		}

		[Authorize(Policy = "Administrador")]
		public ActionResult Delete(int id)
		{
			var p = repositorio.ObtenerPorId(id);
			return View(p);
		}

		[Authorize(Policy = "Administrador")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, Propietario entidad)
		{
			try
			{
				repositorio.Baja(id);
				TempData["Id"] = "El propietario ha sido eliminado";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.Error = "Propietario con inmuebles activos";
				ViewBag.StackTrate = ex.StackTrace;
				return View(entidad);
			}
		}
	}
}