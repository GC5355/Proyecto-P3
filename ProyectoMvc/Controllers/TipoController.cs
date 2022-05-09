using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasosUso;
using Dominio.EntidadesNegocio;
using ProyectoMvc.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;


namespace ProyectoMvc.Controllers
{
    public class TipoController : Controller
    {
        public IManejadorTipos ManejadorTipos { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; set; }

        public TipoController(IManejadorTipos manejaTipos, IWebHostEnvironment whenv)
        {
            ManejadorTipos = manejaTipos;
            WebHostEnvironment = whenv;
        }
        // GET: TipoController

        //Listar todos los tipos
        public ActionResult Index()
        {
            IEnumerable<Tipo> mistipos = ManejadorTipos.ListarTodosLostipos();
            List<Tipo> miLista = mistipos.ToList();
            if (miLista.Count == 0)
            {
                ViewBag.Mensaje = "No existen tipos disponibles";
            }
            return View(mistipos);
        }


        //implementacion de busqueda por nombre de tipo
        public IActionResult IndexBusqueda()
        {
            return View();
        }


        public IActionResult BusquedaPorTipo(IFormCollection misDatos)
        {
            string Nombre = misDatos["Nombre"];

            Tipo miTipo = ManejadorTipos.BuscarTipoPorNombre(Nombre);
            if (miTipo != null)
            {
                ViewBag.Mensaje = miTipo.ToString();
            }
            else
            {
                ViewBag.Mensaje = "No existen tipos con este nombre";
            }

            return View("IndexBusqueda");
        }

        //


        //Implementacion de alta tipo
        // GET: TipoController/Create
        public ActionResult AltaTipo()
        {
            ViewModelTipo miModelo = new ViewModelTipo();
            return View(miModelo);
        }

        // POST: TipoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AltaTipo(ViewModelTipo miModelo)
        {
            bool funcionaPorFavor = ManejadorTipos.AltaTipo(miModelo.Tipo, Int32.Parse(miModelo.TipoDescripcionMax), Int32.Parse(miModelo.TipoDescripcionMin));

            try
            {

                if (funcionaPorFavor)
                {
                    ViewBag.Mensaje = "Alta de tipo correcta";
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo hacer el alta";
                }
            }
            catch
            {
                return View();
            }
            return View();


        }


        //Implementacion de elminacion de tipo 
        public IActionResult IndexEliminar()
        {

            return View();
        }

        public IActionResult ConfirmarEliminacion(int idVerificacionTipo)
        {
            bool existe = ManejadorTipos.VerificarTipoEnUso(idVerificacionTipo);
            if (existe)
            {
                ViewBag.Mensaje = "No se puede eliminar, el tipo esta asociado en al menos una planta";
            }
            else
            {
                ManejadorTipos.EliminarTipo(idVerificacionTipo);
                ViewBag.Mensaje = "Se elimino correctamente el tipo";
            }

            return View("ConfirmarEliminacion");

        }

        public IActionResult EliminarTipo(IFormCollection misDatos)
        {
            string Nombre = misDatos["Nombre"];

            Tipo miTipo = ManejadorTipos.BuscarTipoPorNombre(Nombre);

            if (miTipo != null)
            {


                ViewBag.MuestroDatos = miTipo.ToString();//guardo los datos para mostrar en la confirmacion de eliminacion
                ViewBag.IdParaVerificar = miTipo.Id;//guardo el id para verificar si el usuario confirma la eliminacion, no este en uso
            }
            else
            {
                ViewBag.Mensaje = "No existen tipos con este nombre";
                return View("IndexEliminar");
            }

            return View("ConfirmarEliminacion");
        }



        public ActionResult ModificarDescripcion()
        {

            return View();
        }

        public ActionResult ModificarDescripcionTipo(IFormCollection misDatos)
        {
            string nombre = misDatos["Nombre"];
            Tipo miTipo = ManejadorTipos.BuscarTipoPorNombre(nombre);

            try
            {

                if (miTipo != null)
                {

                    ViewBag.Mensaje = miTipo;

                }
                else

                {
                    ViewBag.Alerta = "No existen tipos con ese nombre";
                }

                return View("ModificarDescripcion");

            }
            catch
            {
                return View();
            }


        }

        public ActionResult ConfirmarModificacion(IFormCollection misDatos)
        {
            string nombre = misDatos["Nombre"];
            string descripcion = misDatos["Descripcion"];
            string minimo = misDatos["TipoDescripcionMin"];
            string maximo = misDatos["TipoDescripcionMax"];

            bool ok = ManejadorTipos.ModificarDescripcion(nombre, descripcion, Int32.Parse(maximo), Int32.Parse(minimo));



            if (ok)
            {
                ViewBag.Confirmar = "Descripcion Actualizada";
            }

            return View("ModificarDescripcion");

        }




    }
}
