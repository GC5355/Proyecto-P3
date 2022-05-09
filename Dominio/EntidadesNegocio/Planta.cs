using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.EntidadesNegocio
{
    public class Planta : IValidable
    {
        public int Id { get; set; }
        public Tipo Tipo { get; set; }
        public string NombreCientifico { get; set; }
        public List<NombreVulgar> NombresVulgares { get; set; }
        public decimal AlturaMax { get; set; }
        public string Foto { get; set; }
        public Ambiente AmbientePlanta { get; set; }
        public FichaCuidado Cuidado { get; set; }
        public string Descripcion { get; set; }


        public enum Ambiente
        {
            interior = 1,
            exterior = 2,
            mixta = 3
        }


        public override string ToString()
        {
            return "Id: " + Id + " | Nombre cientifico: " + NombreCientifico;
        }



        public bool SoyValido()             
        {
            bool valido = false;

            if (this.Tipo.SoyValido())
            {
                if (NombreCientifico != null && NombresVulgares != null && AlturaMax > 0 && Foto != "" && Descripcion != null && Cuidado != null)
                {
                    NombreCientifico = NombreCientifico.Trim();
                    valido = true;
                }
            }
            return valido;
        }
        public bool SoyValidoDosPlanta(int PlantaDescripcionMin, int  PlantaDescripcionMax)
        {
            // Valida el ingreso de la descripcion, le llega por parametros los valores max y min.
            bool valido = false;

            if (Descripcion.Length > PlantaDescripcionMin && Descripcion.Length < PlantaDescripcionMax)
            {
                valido = true;
            }

            return valido;
        }
    }
}
