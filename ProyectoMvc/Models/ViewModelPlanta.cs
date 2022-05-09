using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.EntidadesNegocio;
using Microsoft.AspNetCore.Http;


namespace ProyectoMvc.Models
{
    public class ViewModelPlanta
    {
        public Planta Planta { get; set; }
        public FichaCuidado FichaCuidado { get; set; }

        public NombreVulgar NombreVulgar { get; set; }

        public List<NombreVulgar> ListaNombres { get; set; }

        public IEnumerable<Planta> listaPlantas { get; set; }
        public IEnumerable<Tipo> Tipos { get; set; }

        public IFormFile Imagen { get; set; }

        public int IdTipoSeleccionado { get; set; }


        public string IngresoTextoBusqueda { get; set; }

        public string IngresoAlturaBusquedaMin { get; set; }
        public string IngresoAlturaBusquedaMax { get; set; }
        public string PlantaDescripcionMin { get; set; }
        public string PlantaDescripcionMax { get; set; }

    }
}
