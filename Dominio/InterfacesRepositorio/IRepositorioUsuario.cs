using System;
using System.Collections.Generic;
using System.Text;
using Dominio.EntidadesNegocio;

namespace Dominio.InterfacesRepositorio
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {
        public bool Login(string email, string pass);

        public Usuario BuscarUsuarioXEmail(string email);
    }
}
