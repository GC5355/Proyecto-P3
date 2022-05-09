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
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoMvc.Controllers
{
    public class PlantasController : Controller
    {
        public IManejadorPlantas ManejadorPlantas { get; set; }


        public IWebHostEnvironment WebHostEnvironment { get; set; }

        public PlantasController(IManejadorPlantas manejadorPlantas, IWebHostEnvironment webHostEnvironment)
        {
            ManejadorPlantas = manejadorPlantas;
            WebHostEnvironment = webHostEnvironment;
        }


        // GET: PlantasController
        public ActionResult Index()
        {
            IEnumerable<Planta> plantas = ManejadorPlantas.ListarPlantas();
            return View(plantas);
        }

    

        // GET: PlantasController/Create
        public ActionResult Create()
        {
            ViewModelPlanta vm = new ViewModelPlanta();
            vm.Tipos = ManejadorPlantas.ListarTiposDePlantas();
           // vm.AmbientePlanta = Planta.Ambiente.mixta;

            return View(vm);
        }

        // POST: PlantasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewModelPlanta vm, int AmbientePlanta)
        {
            try
            {
                if (vm.Imagen != null)
                {
                    vm.Tipos = ManejadorPlantas.ListarTiposDePlantas();
                    vm.Planta.AmbientePlanta = (Planta.Ambiente)AmbientePlanta;

                    string extension = Path.GetExtension(vm.Imagen.FileName);
                    string nomArchivo = vm.Planta.NombreCientifico;
                    nomArchivo = nomArchivo.Replace(' ', '_');
                    nomArchivo = nomArchivo + "_001";
                    nomArchivo = nomArchivo + extension;
                    vm.Planta.Foto = nomArchivo;

                    string cadena = vm.NombreVulgar.Nombre;
                    List<NombreVulgar> listAux = ExtraerNombresVulgares(cadena);

                    vm.Planta.NombresVulgares = listAux;
                    vm.Planta.Cuidado = vm.FichaCuidado;

                    bool ok = ManejadorPlantas.RegistrarNuevaPlanta(vm.Planta, vm.IdTipoSeleccionado, Int32.Parse(vm.PlantaDescripcionMin), Int32.Parse(vm.PlantaDescripcionMax));

                    if (ok)
                    {
                        if (vm.Imagen.ContentType == ("image/jpeg") || vm.Imagen.ContentType == ("image/png"))
                        {
                            string rutaRaiz = WebHostEnvironment.WebRootPath;
                            string rutaImagenes = Path.Combine(rutaRaiz, "imagenes");
                            string rutaArchivo = Path.Combine(rutaImagenes, nomArchivo);

                            FileStream stream = new FileStream(rutaArchivo, FileMode.Create);
                            vm.Imagen.CopyTo(stream);
                            vm.Tipos = ManejadorPlantas.ListarTiposDePlantas();

                            ViewBag.Msj = "Alta de planta correcta";
                            return View(vm);


                        }
                        else
                        {
                            vm.Tipos = ManejadorPlantas.ListarTiposDePlantas();
                            ViewBag.Error("Debe subir una imagen con extension .jpg o .png");
                            return View(vm);
                        }

                    }
                    else
                    {
                        vm.Tipos = ManejadorPlantas.ListarTiposDePlantas();
                        ViewBag.Msj = "NO SE PUDO HACER EL ALTA";
                        return View(vm);
                    }

                }
                else
                {
                    vm.Tipos = ManejadorPlantas.ListarTiposDePlantas();
                    ViewBag.Msj = "NO SE PUDO HACER EL ALTA";
                    return View(vm);
                }
            }
            catch (Exception ex)
            {
                return View();
            }

        }
               
        
        public List<NombreVulgar> ExtraerNombresVulgares(string cadena)
        {
            // Dada un string, crea una lista de nombres. El caracter que separa cada nombre es /.

            string palabra = "";
            bool ok = false;

            List<NombreVulgar> listAux = new List<NombreVulgar>();

            for (int i = 0; i < cadena.Length; i++)
            {
                if (cadena[i].ToString() != "/")
                {
                    palabra += cadena[i];
                }
                else if (cadena[i].ToString() == "/")
                {
                    NombreVulgar nom = new NombreVulgar()
                    {
                        Nombre = palabra
                    };
                    listAux.Add(nom);
                    palabra = "";
                }

            }

            NombreVulgar n = new NombreVulgar()
            {
                Nombre = palabra
            };
            listAux.Add(n);

            if (listAux.Count != 0)
            {
                ok = true;
            }
            return listAux;
        }




        // GET: Buscar por tipo
        public ActionResult BusquedaPlantas()
        {
            ViewModelPlanta vm = new ViewModelPlanta();
            vm.Tipos = ManejadorPlantas.ListarTiposDePlantas();
            vm.listaPlantas = new List<Planta>();
            return View(vm);
        }

        // POST: Buscar por tipo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BusquedaPlantas(ViewModelPlanta vm, int AmbientePlanta)
        {
            // Dependiendo que variable este llegando por parametro, va a entrar en el if correspondiente para hacer la busqueda.

            try
            {
                vm.Tipos = ManejadorPlantas.ListarTiposDePlantas();
                if (vm.IdTipoSeleccionado != 0)
                {
                    // Busqueda por TIPO
                    IEnumerable<Planta> listaPlantasBuscadas = ManejadorPlantas.ListarPlantasPorTipo(vm.IdTipoSeleccionado);
                    vm.listaPlantas = listaPlantasBuscadas;

                }else if(vm.IngresoTextoBusqueda != "" && vm.IngresoTextoBusqueda != null)
                {
                    // Busqueda por TEXTO
                    IEnumerable < Planta > listaPlantasBuscadas = ManejadorPlantas.BuscarPlantaPorTexto(vm.IngresoTextoBusqueda);
                    vm.listaPlantas = listaPlantasBuscadas;

                }
                else if (vm.IngresoAlturaBusquedaMin != null)
                {
                    //Busqueda por ALTURA MINIMA 
                    IEnumerable<Planta> listaPlantasBuscadas = ManejadorPlantas.BuscarPlantaAlturaMinima(Int32.Parse(vm.IngresoAlturaBusquedaMin));
                    vm.listaPlantas = listaPlantasBuscadas;
                    
                }
                else if (vm.IngresoAlturaBusquedaMax != null)
                {
                    //Busqueda por ALTURA MAX 
                    IEnumerable<Planta> listaPlantasBuscadas = ManejadorPlantas.BuscarPlantaAlturaMaxima(Int32.Parse(vm.IngresoAlturaBusquedaMax));
                    vm.listaPlantas = listaPlantasBuscadas;
                 
                }else if(AmbientePlanta != 0)
                {
                    //Busqueda por AMBIENTE
                    IEnumerable<Planta> listaPlantasBuscadas = ManejadorPlantas.BuscarPlantaPorAmbiente(AmbientePlanta);
                    vm.listaPlantas = listaPlantasBuscadas;

                }

                vm.Tipos = ManejadorPlantas.ListarTiposDePlantas();
                return View(vm);

            }
            catch
            {
                vm.Tipos = ManejadorPlantas.ListarTiposDePlantas();
                return View(vm);
            }
        }


        // GET: MostrarFichaCuidados
        public ActionResult MostrarFichaCuidados()
        {
            FichaCuidado ficha = new FichaCuidado()
            {
                FrecuenciaRiego = 6,
                Id = 2
            };
            
            return View(ficha);
        }

    }
}
