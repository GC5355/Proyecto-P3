using System;
using System.Collections.Generic;
using System.Text;
using Dominio.InterfacesRepositorio;
using Dominio.EntidadesNegocio;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Repositorios
{
    public class RepositoriosTipoADO : IRepositorioTipo
    {

        //implementacion de alta de tipo
        public bool Add(Tipo miTipo)
        {
            bool miRetorno = false;

            if (miTipo != null)
            {
                SqlConnection miConexion = Conexion.ObtenerConexion();
                string miSql = "INSERT INTO Tipo VALUES(@nombre, @descripcion); SELECT CAST(SCOPE_IDENTITY() AS INT);";
                SqlCommand miComand = new SqlCommand(miSql, miConexion);

                miComand.Parameters.AddWithValue("@nombre", miTipo.Nombre);
                miComand.Parameters.AddWithValue("@descripcion", miTipo.Descripcion);

                if (miTipo.Nombre != null && miTipo.Descripcion != null && miTipo.SoyValido() && BuscarTipoPorNombre(miTipo.Nombre) == null)
                {

                    try
                    {
                        Conexion.AbrirConexion(miConexion);
                        int idTipo = (int)miComand.ExecuteScalar();
                        miTipo.Id = idTipo;
                        miRetorno = true;
                        Conexion.CerrarConexion(miConexion);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        Conexion.CerrarConexion(miConexion);
                    }
                }
            }
          

            return miRetorno;
        }
        //

        //implementacion de listar todos los tipos de planta

        public List<Tipo> FindAll()
        {
            List<Tipo> misTipos = new List<Tipo>();

            SqlConnection miConexion = Conexion.ObtenerConexion();

            string miSql = "SELECT * FROM Tipo;";
            SqlCommand miCommand = new SqlCommand(miSql, miConexion);

            try
            {
                Conexion.AbrirConexion(miConexion);
                SqlDataReader reader = miCommand.ExecuteReader();

                while (reader.Read())
                {
                    Tipo tipoAgregar = new Tipo()
                    {


                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Descripcion = reader.GetString(2),


                    };
                    misTipos.Add(tipoAgregar);
                }

                Conexion.CerrarConexion(miConexion);
            }

            catch
            {
                throw;
            }
            finally
            {
                Conexion.CerrarConexion(miConexion);
            }

            return misTipos;

        }
        //ver si reemplazar esto

        public Tipo FindById(int id)
        {
            Tipo buscado = null;

            SqlConnection con = Conexion.ObtenerConexion();

            string sql = "SELECT * FROM Tipo WHERE Id=@id;";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@id", id);

            try
            {
                Conexion.AbrirConexion(con);
                SqlDataReader reader = com.ExecuteReader();

                if (reader.Read())
                {
                    buscado = new Tipo()
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Descripcion = reader.GetString(2),

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

            return buscado;
        }

        public Tipo BuscarTipoPorNombre(string nombre)
        {
            Tipo nombreBuscado = null;

            if(nombre != null)
            {
                SqlConnection miConexion = Conexion.ObtenerConexion();

                string miSql = "SELECT * FROM Tipo WHERE Nombre=@nombre;";
                SqlCommand miCommand = new SqlCommand(miSql, miConexion);
                miCommand.Parameters.AddWithValue("@nombre", nombre);

                try
                {
                    Conexion.AbrirConexion(miConexion);
                    SqlDataReader reader = miCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        nombreBuscado = new Tipo()
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Descripcion = reader.GetString(2),
                        };
                    }

                    Conexion.CerrarConexion(miConexion);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Conexion.CerrarConexion(miConexion);
                }
            }

           

            return nombreBuscado;
        }
        //

        //para elimnar un tipo

        public bool VerificarTipoEnUso(int miTipo)
        {
            bool tipoEnUso = false;

            SqlConnection miConexion = Conexion.ObtenerConexion();

            string miSql = "SELECT * FROM Planta WHERE IdTipo=@miTipo;";
            SqlCommand miCommand = new SqlCommand(miSql, miConexion);
            miCommand.Parameters.AddWithValue("@miTipo", miTipo);

            try
            {
                Conexion.AbrirConexion(miConexion);
                SqlDataReader reader = miCommand.ExecuteReader();

                if (reader.Read())
                {
                    tipoEnUso = true;

                }

                Conexion.CerrarConexion(miConexion);
            }
            catch
            {
                throw;
            }
            finally
            {
                Conexion.CerrarConexion(miConexion);
            }

            return tipoEnUso;
        }


        public bool Remove(int id)
        {
            bool ok = false;

            SqlConnection miConexion = Conexion.ObtenerConexion();

            string miSql = "DELETE FROM Tipo WHERE Id=@id;";
            SqlCommand com = new SqlCommand(miSql, miConexion);
            com.Parameters.AddWithValue("@id", id);

            try
            {
                Conexion.AbrirConexion(miConexion);
                int filasAfectadas = com.ExecuteNonQuery();
                ok = filasAfectadas == 1;
                Conexion.CerrarConexion(miConexion);
            }
            catch
            {
                throw;
            }
            finally
            {
                Conexion.CerrarConexion(miConexion);
            }

            return ok;
        }

        public bool ModificarDescripcion(string nombre, string descripcion, int maximo, int minimo)
        {
            bool retorno = false;

            SqlConnection miConexion = Conexion.ObtenerConexion();

            Tipo miTipo = BuscarTipoPorNombre(nombre);





            if (miTipo != null && miTipo.Nombre == miTipo.Nombre)
            {
                string miSql = "UPDATE Tipo SET Descripcion=@descripcion WHERE Nombre=@nombre;";
                SqlCommand com = new SqlCommand(miSql, miConexion);

                com.Parameters.AddWithValue("@id", miTipo.Id);
                com.Parameters.AddWithValue("@nombre", miTipo.Nombre);
                com.Parameters.AddWithValue("@descripcion", descripcion);
                miTipo.Descripcion = descripcion;

                if (miTipo.SoyValidoDos(maximo, minimo))
                {
                    try
                    {
                        Conexion.AbrirConexion(miConexion);
                        int filasAfectadas = com.ExecuteNonQuery();
                        retorno = filasAfectadas == 1;
                        Conexion.CerrarConexion(miConexion);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        Conexion.CerrarConexion(miConexion);
                    }
                }
            }

            return retorno;
        }
        public bool Update(Dominio.EntidadesNegocio.Tipo obj)
        {
            throw new NotImplementedException();
        }

    }
}
