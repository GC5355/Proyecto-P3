using Dominio.EntidadesNegocio;
using System;
using System.Collections.Generic;
using System.Text;
using Repositorios;
using Dominio.InterfacesRepositorio;


namespace CasosUso
{
    public class ManejadorPlantas : IManejadorPlantas
    {
        public IRepositorioPlanta RepoPlanta { get; set; }
        public IRepositorioTipo RepoTipo { get; set; }

        public ManejadorPlantas(IRepositorioPlanta repoPlanta, IRepositorioTipo repoTipo)
        {
            RepoPlanta = repoPlanta;
            RepoTipo = repoTipo;
        }

        public IEnumerable<FichaCuidado> ListarFichasDeCuidados()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tipo> ListarTiposDePlantas()
        {
            return RepoTipo.FindAll();
        }

        public IEnumerable<Planta> ListarPlantasPorTipo(int idTipo)
        {
            return RepoPlanta.ListarPlantasPorTipo(idTipo);
        }

        public IEnumerable<Planta> ListarPlantas()
        {
            return RepoPlanta.FindAll();
        }


        public bool RegistrarNuevaPlanta(Planta miPlanta, int idTipoP, int PlantaDescripcionMax, int PlantaDescripcionMin)
        {
            bool ok = false;
            Tipo tipo = RepoTipo.FindById(idTipoP);

            if (tipo != null)
            {
                bool estaNombreCientifico = RepoPlanta.BuscarNombreCientifico(miPlanta.NombreCientifico);
                if (!estaNombreCientifico)
                {
                    miPlanta.Tipo = tipo;

                    if (miPlanta.SoyValido() && miPlanta.SoyValidoDosPlanta(PlantaDescripcionMax, PlantaDescripcionMin))
                    {
                        ok = RepoPlanta.Add(miPlanta);
                    }
                }
            }

            return ok;
        }

        public List<Planta> BuscarPlantaPorTexto(string textoIngresado)
        {
            List<Planta> listaPlantas = RepoPlanta.FindAll();
            List<Planta> listaFiltrada = new List<Planta>();

            for (int i = 0; i < listaPlantas.Count; i++)
            {
                if (listaPlantas[i].NombreCientifico.IndexOf(textoIngresado) != -1)
                {
                    listaFiltrada.Add(listaPlantas[i]);
                    listaPlantas.RemoveAt(i);
                    i--;
                }
            }
    
             for(int i = 0; i < listaPlantas.Count; i++)
            {
                for(int j = 0; j< listaPlantas[i].NombresVulgares.Count; j++)
                {
                    if(listaPlantas[i].NombresVulgares[j].Nombre.IndexOf(textoIngresado) != -1)
                    {
                        listaFiltrada.Add(listaPlantas[i]);
                    }
                }

            }

            return listaFiltrada;
        }




        public IEnumerable<Planta> BuscarPlantaAlturaMinima(decimal alturaIngresada)
        {

            return RepoPlanta.BuscarPlantaAlturaMinima(alturaIngresada);

        }

        public IEnumerable<Planta> BuscarPlantaAlturaMaxima(decimal alturaIngresada)
        {

            return RepoPlanta.BuscarPlantaAlturaMaxima(alturaIngresada);

        }

        public IEnumerable<Planta> BuscarPlantaPorAmbiente(int AmbientePlanta)
        {
            return RepoPlanta.BuscarPlantaPorAmbiente(AmbientePlanta);
        }


    }
}
