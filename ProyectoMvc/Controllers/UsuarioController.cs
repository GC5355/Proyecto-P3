using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProyectoMvc.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CasosUso;
using Dominio.EntidadesNegocio;
using Repositorios;
using Microsoft.AspNetCore.Http;

namespace ProyectoMvc.Controllers
{
    public class UsuarioController : Controller
    {
        public IManejadorUsuarios ManejadorUsuarios { get; set; }

        public UsuarioController(IManejadorUsuarios miManejador)
        {
            ManejadorUsuarios = miManejador;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(IFormCollection datosInicio)
        {
            string Email = datosInicio["Email"];
            string Contrasenia = datosInicio["Contrasenia"];


            bool inicioCorrecto = ManejadorUsuarios.InicioSesion(Email, Contrasenia);
            if (inicioCorrecto)
            {
                HttpContext.Session.SetString("UsuarioLogueado", Email);
                return RedirectToAction("AltaTipo", "Tipo");
            }

            else
            {
                ViewBag.Mensaje = "Datos Incorrectos";
            }

            return View("Index");

        }

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logout(string x)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
