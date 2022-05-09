using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.EntidadesNegocio
{
    public class Item : IValidable
    {
        public Planta Planta { get; set; }
        public int Cantidad { get; set; }
        public Decimal PrecioCongelado { get; set; }

        public bool SoyValido()
        {
            throw new NotImplementedException();
        }
    }
}
