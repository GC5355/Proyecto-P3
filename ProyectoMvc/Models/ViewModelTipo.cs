using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.EntidadesNegocio;

namespace ProyectoMvc.Models
{
    public class ViewModelTipo
    {
        public Tipo Tipo { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string TipoDescripcionMin { get; set; }
        public string TipoDescripcionMax { get; set; }
    }
}
