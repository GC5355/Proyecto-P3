using System;
using System.Collections.Generic;
using System.Text;
using Dominio.EntidadesNegocio;

namespace CasosUso
{
    public interface IManejadorUsuarios
    {
        bool ValidarAltaUsuario(Usuario miUsuario);//no se implementa en MVC
        bool InicioSesion(string email, string password);
    }

}
