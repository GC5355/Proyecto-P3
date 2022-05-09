using System;
using System.Collections.Generic;
using System.Text;
using Dominio.EntidadesNegocio;
using Repositorios;
using Dominio.InterfacesRepositorio;

namespace CasosUso
{
    public class ManejadorTipos : IManejadorTipos
    {
        public IRepositorioTipo RepoDeTipo { get; set; }
        public IRepositorioPlanta RepoDePlanta { get; set; }
        public ManejadorTipos(IRepositorioTipo repositorioTipo, IRepositorioPlanta repositorioPlanta)

        {
            RepoDeTipo = repositorioTipo;
            RepoDePlanta = repositorioPlanta;
        }

        public Tipo BuscarTipoPorNombre(string nombreBuscado)
        {
            return RepoDeTipo.BuscarTipoPorNombre(nombreBuscado);

        }


        public IEnumerable<Tipo> ListarTodosLostipos()
        {
            return RepoDeTipo.FindAll();
        }
        //metodo para implementar el alta de un tipo de planta
        public bool AltaTipo(Tipo miTipo, int TipoDescripcionMax, int TipoDescripcionMin)
        {
            bool ok = false;

            if (miTipo.SoyValido() && miTipo.SoyValidoDos(TipoDescripcionMax, TipoDescripcionMin)) 
            {
                ok = RepoDeTipo.Add(miTipo);

            }

            return ok;
        }

        public IEnumerable<Planta> TraerTodasLasPlantas()
        {
            return RepoDePlanta.FindAll();
        }

        public bool VerificarTipoEnUso(int miTipo)
        {
            return RepoDeTipo.VerificarTipoEnUso(miTipo);
        }

        public bool EliminarTipo(int id)
        {
            return RepoDeTipo.Remove(id);
        }

        public bool ModificarDescripcion(string nombre, string descripcion, int maximo, int minimo)
        {
            return RepoDeTipo.ModificarDescripcion(nombre, descripcion, maximo, minimo);
        }

    }
}
