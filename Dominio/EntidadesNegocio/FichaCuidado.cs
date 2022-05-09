using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.EntidadesNegocio
{
    public class FichaCuidado : IValidable
    {
        public int Id { get; set; }
        public int FrecuenciaRiego { get; set; }
        public int UnidadTiempo { get; set; }
        public int Temperatura { get; set; }

        public TipoIluminacion Iluminacion { get; set; }

  

        public enum TipoIluminacion
        {
            directa,
            indirecta,
            sombra
        }

        public bool SoyValido()
        {
            throw new NotImplementedException();
        }


    }
}
