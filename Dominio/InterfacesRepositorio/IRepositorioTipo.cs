using System;
using System.Collections.Generic;
using System.Text;
using Dominio.EntidadesNegocio;

namespace Dominio.InterfacesRepositorio
{
    public interface IRepositorioTipo : IRepositorio<Tipo>
    {

        Tipo BuscarTipoPorNombre(string email);

        bool VerificarTipoEnUso(int miTipo);

        bool ModificarDescripcion(string nombre, string descripcion, int caracteresMinimos, int caracteresMaximos);

    }
}
