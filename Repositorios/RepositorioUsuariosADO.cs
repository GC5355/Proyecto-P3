using System;
using System.Collections.Generic;
using System.Text;
using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorio;
using Microsoft.Data.SqlClient;

namespace Repositorios
{
    public class RepositorioUsuariosADO : IRepositorioUsuario

    {
        public bool Add(Usuario miUsuario)
        {

            bool miRetorno = false;

            SqlConnection con = Conexion.ObtenerConexion();

            if (BuscarUsuarioXEmail(miUsuario.Email) != null)//se verifica que el email no este en uso
            {

                string miSql = "INSERT INTO Usuario VALUES(@email, @contrasenia);" +
                "SELECT CAST(SCOPE_IDENTITY() AS INT);";


            SqlCommand com = new SqlCommand(miSql, con);

            com.Parameters.AddWithValue("@email", miUsuario.Email);
            com.Parameters.AddWithValue("@contrasenia", miUsuario.Password);

           

                try
                {
                    Conexion.AbrirConexion(con);
                    int filasAfectadas = com.ExecuteNonQuery();
                    miRetorno = filasAfectadas == 1;
                    Conexion.CerrarConexion(con);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Conexion.CerrarConexion(con);
                }
            }

            return miRetorno;
        }

        public bool Login(string email, string password)
        {
            bool miRetorno = false;
            Usuario miUsuario = BuscarUsuarioXEmail(email);
            if (miUsuario != null && miUsuario.Password == password)
            {
                miRetorno = true;
            }
            return miRetorno;
        }

        public Usuario BuscarUsuarioXEmail(string email)
        {
            Usuario miUsuario = null;

            SqlConnection con = Conexion.ObtenerConexion();

            string sql = "SELECT * FROM Usuario WHERE Email=@email;";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@email", email);

            try
            {
                Conexion.AbrirConexion(con);
                SqlDataReader reader = com.ExecuteReader();

                if (reader.Read())
                {
                    miUsuario = new Usuario()
                    {

                        Email = reader.GetString(1),
                        Password = reader.GetString(2),
                    };
                }

                Conexion.CerrarConexion(con);
            }
            catch
            {
                throw;
            }
            finally
            {
                Conexion.CerrarConexion(con);
            }

            return miUsuario;


        }

        public List<Usuario> FindAll()
        {
            throw new NotImplementedException();
        }

        public Usuario FindById(int id)
        {
            throw new NotImplementedException();
        }



        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Usuario obj)
        {
            throw new NotImplementedException();
        }


    }
}
