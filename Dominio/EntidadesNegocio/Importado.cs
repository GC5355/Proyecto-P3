using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.EntidadesNegocio
{
   public class Importado : Compra 
    {
        public static int Impuesto { get; set; }
        public static int TasaArancel { get; set; }
        public int ImpuestoCongelado { get; set; }
        public bool EsAmericaSur { get; set; }
        public int TasaArancelCongelado { get; set; }
        public string DescripcionMedidasSanitarias { get; set; }


        public bool SoyValido()
        {
            throw new NotImplementedException();
        }
    }
}
