using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.EntidadesNegocio
{
    public class NombreVulgar : IValidable
    {
         public int Id { get; set; }
        public string Nombre { get; set; }

        public bool SoyValido()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return  Nombre + " ";
        }
    }
}
