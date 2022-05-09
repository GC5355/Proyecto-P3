using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.EntidadesNegocio
{
   public abstract class Compra : IValidable
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public List<Item> Items { get; set; }



        public bool SoyValido()
        {
            throw new NotImplementedException();
        }


    }
}

