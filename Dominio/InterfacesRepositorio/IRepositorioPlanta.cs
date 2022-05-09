using System;
using System.Collections.Generic;
using System.Text;
using Dominio.EntidadesNegocio;

namespace Dominio.InterfacesRepositorio
{
    public interface IRepositorioPlanta : IRepositorio<Planta>
    {
        List<NombreVulgar> BuscarNombresVulgaresPlanta(int idPlanta);
        FichaCuidado BuscarFichaCuidado(int idPlanta);

        public IEnumerable<Planta> ListarPlantasPorTipo(int idTipo);

        public bool BuscarNombreCientifico(string NombreCientifico);

        public IEnumerable<Planta> BuscarPlantaAlturaMinima(decimal alturaIngresada);

        public IEnumerable<Planta> BuscarPlantaAlturaMaxima(decimal alturaIngresada);

        public IEnumerable<Planta> BuscarPlantaPorAmbiente(int AmbientePlanta);
    }
}

