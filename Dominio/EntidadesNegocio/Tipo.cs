using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.EntidadesNegocio
{
    public class Tipo : IValidable
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }





        public bool SoyValido()
        {

            bool valido = true;

            if (Nombre != null)
            {
                int i = 0;
                while (i < Nombre.Length && valido)
                {
                    if (char.IsLetter(Nombre[i]) || Nombre[i].ToString() == " ")
                    {
                        valido = true;
                    }
                    else
                    {
                        valido = false;
                    }
                    i++;
                }

                Nombre = Nombre.Trim();
            }


            return valido;
        }

        public bool SoyValidoDos(int TipoDescripcionMax, int TipoDescripcionMin)
        {   // Valida el ingreso de la descripcion, le llega por parametros los valores max y min.
            bool valido = false;

            if (Descripcion != null && Descripcion.Length > TipoDescripcionMin && Descripcion.Length < TipoDescripcionMax)
            {
                valido = true;
            }

            return valido;
        }


        public override string ToString()
        {
            return "Id: " + Id + " Nombre: " + Nombre + " Descripcion: " + Descripcion;
        }

    }
}
