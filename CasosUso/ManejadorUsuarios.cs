using System;
using System.Collections.Generic;
using System.Text;
using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorio;

namespace CasosUso
{
    public class ManejadorUsuarios : IManejadorUsuarios
    {

        public IRepositorioUsuario miRepositorio { get; set; }

        public bool InicioSesion(string email, string contrasenia)
        {

            bool inicio = miRepositorio.Login(email, contrasenia);
            return inicio;
        }

        public ManejadorUsuarios(IRepositorioUsuario miRepo)
        {
            miRepositorio = miRepo;
        }
        public bool ValidarAltaUsuario(Usuario miUsuario)
        {
            bool esValido = false;

            if (miUsuario.SoyValido())
            {

                esValido = miRepositorio.Add(miUsuario);

            }

            return esValido;

        }

    }
}
