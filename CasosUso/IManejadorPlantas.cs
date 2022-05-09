using System;
using System.Collections.Generic;
using System.Text;
using Dominio.EntidadesNegocio;


namespace CasosUso
{
   public interface IManejadorPlantas
    {
        public bool RegistrarNuevaPlanta(Planta miPlanta, int idTipoP, int PlantaDescripcionMax, int PlantaDescripcionMin);

        IEnumerable<Tipo> ListarTiposDePlantas();

        IEnumerable<Planta> ListarPlantasPorTipo(int idTipo);

        IEnumerable<FichaCuidado> ListarFichasDeCuidados();

        public IEnumerable<Planta> ListarPlantas();

       
        public List<Planta> BuscarPlantaPorTexto(string textoIngresado);

        public IEnumerable<Planta> BuscarPlantaAlturaMinima(decimal alturaIngresada);
        public IEnumerable<Planta> BuscarPlantaAlturaMaxima(decimal alturaIngresada);

        public IEnumerable<Planta> BuscarPlantaPorAmbiente(int AmbientePlanta);

    }
}
