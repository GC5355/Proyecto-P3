using System;
using System.Collections.Generic;
using System.Text;
using Dominio.EntidadesNegocio;

namespace CasosUso
{
   public interface IManejadorTipos
    {

        public bool AltaTipo(Tipo miTipo, int TipoDescripcionMax, int TipoDescripcionMin);

        IEnumerable<Tipo> ListarTodosLostipos();

        Tipo BuscarTipoPorNombre(string nombreBuscado);

        IEnumerable<Planta> TraerTodasLasPlantas();//Para la operacion crud eliminacion de tipo,se "traen" todas las plantas para verificar
        //que el tipo no este en uso

        bool VerificarTipoEnUso(int miTipo);

        bool EliminarTipo(int id);

        public bool ModificarDescripcion(string nombre, string descripcion, int maximo,int minimo);


    }
}
