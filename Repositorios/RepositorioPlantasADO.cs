using System;
using System.Collections.Generic;
using System.Text;
using Dominio.InterfacesRepositorio;
using Dominio.EntidadesNegocio;
using Microsoft.Data.SqlClient;


namespace Repositorios
{
    public class RepositorioPlantasADO : IRepositorioPlanta
    {

        public bool Add(Planta obj)
        {
            bool ok = false;

            SqlConnection con = Conexion.ObtenerConexion();

            string sqlPlanta = "INSERT INTO Planta VALUES(@idTIpo, @nombreCientifico, @alturaMax, @foto, @ambientePlanta, @descr);" +
                 "SELECT CAST(SCOPE_IDENTITY() AS INT);";

            string sqlFichaCuidado = "INSERT INTO FichaCuidado VALUES(@frecuenciaRiego, @unidadTiempo, @temp, @iluminacion, @idPlanta)" +
                 "SELECT CAST(SCOPE_IDENTITY() AS INT);";

            string sqlNombreVulgar = "INSERT INTO NombresVulgares VALUES (@nombre, @idPlanta)" +
                 "SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlCommand com = new SqlCommand(sqlPlanta, con);


            com.Parameters.AddWithValue("@idTIpo", obj.Tipo.Id);
            com.Parameters.AddWithValue("@nombreCientifico", obj.NombreCientifico);
            com.Parameters.AddWithValue("@alturaMax", obj.AlturaMax);
            com.Parameters.AddWithValue("@foto", obj.Foto);
            com.Parameters.AddWithValue("@ambientePlanta", obj.AmbientePlanta);
            com.Parameters.AddWithValue("@descr", obj.Descripcion);


            SqlTransaction tran = null;

            
                try
                {
                    Conexion.AbrirConexion(con);
                    tran = con.BeginTransaction();
                    com.Transaction = tran;

                    // INSERT en Planta
                    int idPlanta = (int)com.ExecuteScalar();
                    obj.Id = idPlanta;
                    com.Parameters.Clear();

                    // INSERT en FichaCuidado
                    com.Parameters.AddWithValue("@frecuenciaRiego", obj.Cuidado.FrecuenciaRiego);
                    com.Parameters.AddWithValue("@unidadTiempo", obj.Cuidado.UnidadTiempo);
                    com.Parameters.AddWithValue("@temp", obj.Cuidado.Temperatura);
                    com.Parameters.AddWithValue("@iluminacion", obj.Cuidado.Iluminacion);
                    com.Parameters.AddWithValue("@idPlanta", obj.Id);
                    com.CommandText = sqlFichaCuidado;


                    int idCuidado = (int)com.ExecuteScalar();
                    obj.Cuidado.Id = idCuidado;
                    com.Parameters.Clear();

                    //INSERT lista de nombres en tabla NombresVulgares

                    foreach (NombreVulgar nv in obj.NombresVulgares)
                    {
                        com.Parameters.AddWithValue("@nombre", nv.Nombre);
                        com.Parameters.AddWithValue("@idPlanta", obj.Id);
                        com.CommandText = sqlNombreVulgar;

                        int idNomVulgar = (int)com.ExecuteScalar();
                        nv.Id = idNomVulgar;

                        com.Parameters.Clear();
                    }

                    tran.Commit();
                    ok = true;
                    Conexion.CerrarConexion(con);
                }
                catch (Exception e)
                {
                    if (tran != null) tran.Rollback();
                    throw;
                }
                finally
                {
                    Conexion.CerrarYDisposeConexion(con);
                }

                
            
          return ok;
        }

        

        public List<Planta> FindAll()
        {
            List<Planta> plantas = new List<Planta>();

            SqlConnection con = Conexion.ObtenerConexion();

            string sql = "select P.*, FC.Id, FC.FrecuenciaRiego, FC.Iluminacion, FC.Temperatura, FC.UnidadTiempo, T.Id, T.Nombre, T.Descripcion from Planta AS P LEFT JOIN FichaCuidado AS FC ON P.Id = FC.IdPlanta LEFT JOIN Tipo AS T ON P.IdTipo = T.Id";


            SqlCommand com = new SqlCommand(sql, con);

            try
            {
                Conexion.AbrirConexion(con);
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    Planta p = CrearPlanta(reader);
                    p.Cuidado = CrearCuidado(reader);
                    p.Tipo = CrearTipo(reader);
                    p.NombresVulgares = BuscarNombresVulgaresPlanta(p.Id);                      // Porque pasa eso??
                    plantas.Add(p);
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

            return plantas;
        }



        private Tipo CrearTipo(SqlDataReader reader)
        {
            return new Tipo()
            {
                Id = reader.GetInt32(12),
                Nombre = reader.GetString(13),
                Descripcion = reader.GetString(14)
            };
        }

        private FichaCuidado CrearCuidado(SqlDataReader reader)
        {
            return new FichaCuidado()
            {
                Id = reader.GetInt32(7),
                FrecuenciaRiego = reader.GetInt32(8),
                Iluminacion = (FichaCuidado.TipoIluminacion)Enum.Parse(typeof(FichaCuidado.TipoIluminacion), reader.GetString(9)),
                Temperatura = reader.GetInt32(10),
                UnidadTiempo = reader.GetInt32(11)
            };
        }


        private Planta CrearPlanta(SqlDataReader reader)
        {
            return new Planta()
            {
                Id = reader.GetInt32(0),
                NombreCientifico = reader.GetString(2),
                AlturaMax = reader.GetDecimal(3),
                Foto = reader.GetString(4),

                AmbientePlanta = (Planta.Ambiente)Enum.Parse(typeof(Planta.Ambiente), reader.GetString(5)),
                Descripcion = reader.GetString(6)
            };
        }




        public Dominio.EntidadesNegocio.Planta FindById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Dominio.EntidadesNegocio.Planta obj)
        {
            throw new NotImplementedException();
        }

        public bool BuscarNombreCientifico(string nombreCientifico)
        {
            bool esta= false;
            SqlConnection con = Conexion.ObtenerConexion();

            string sql = "SELECT * FROM Planta Where NombreCientifico ="  + " '" + nombreCientifico + "' ";

            SqlCommand com = new SqlCommand(sql, con);

            try
            {
                Conexion.AbrirConexion(con);
                SqlDataReader reader = com.ExecuteReader();

                if (reader.Read())
                {
                    esta = true;
                  
                }

                Conexion.CerrarConexion(con);
            }
            catch(Exception e)
            {
                throw;
            }
            finally
            {
                Conexion.CerrarConexion(con);
            }

            return esta;
        }

        public List<NombreVulgar> BuscarNombresVulgaresPlanta(int id)
        {
            List<NombreVulgar> nombresVulgares = new List<NombreVulgar>();

            SqlConnection con = Conexion.ObtenerConexion();

            string sql = "SELECT * FROM NombresVulgares WHERE IdPlanta=" + id;
            SqlCommand com = new SqlCommand(sql, con);

            try
            {
                Conexion.AbrirConexion(con);
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    NombreVulgar nom = new NombreVulgar()
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1)
                    };

                    nombresVulgares.Add(nom);
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

            return nombresVulgares;
        }

        List<NombreVulgar> IRepositorioPlanta.BuscarNombresVulgaresPlanta(int idPlanta)
        {
            List<NombreVulgar> nombresVulgares = new List<NombreVulgar>();

            SqlConnection con = Conexion.ObtenerConexion();

            string sql = "SELECT * FROM NombresVulgares WHERE IdPlanta=" + idPlanta;
            SqlCommand com = new SqlCommand(sql, con);

            try
            {
                Conexion.AbrirConexion(con);
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    NombreVulgar nom = new NombreVulgar()
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1)
                    };

                    nombresVulgares.Add(nom);
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

            return nombresVulgares;
        }


        FichaCuidado IRepositorioPlanta.BuscarFichaCuidado(int idPlanta)
        {
            FichaCuidado ficha = null;
            SqlConnection con = Conexion.ObtenerConexion();

            string sql = "SELECT * FROM FichaCuidado WHERE IdPlanta=" + idPlanta;

            SqlCommand com = new SqlCommand(sql, con);

            try
            {
                Conexion.AbrirConexion(con);
                SqlDataReader reader = com.ExecuteReader();

                if (reader.Read())
                {
                    ficha = new FichaCuidado()
                    {
                        Id = reader.GetInt32(0),
                        FrecuenciaRiego = reader.GetInt32(1),
                        UnidadTiempo = reader.GetInt32(2),
                        Temperatura = reader.GetInt32(3),
                        Iluminacion = (FichaCuidado.TipoIluminacion)Enum.Parse(typeof(FichaCuidado.TipoIluminacion), reader.GetString(4))
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

            return ficha;
        }

        public IEnumerable<Planta> ListarPlantasPorTipo(int idTipo)
        {
            List<Planta> plantas = new List<Planta>();
            SqlConnection con = Conexion.ObtenerConexion();

            string sql = "SELECT * FROM Planta p WHERE p.IdTipo =" + idTipo;
            SqlCommand com = new SqlCommand(sql, con);

            try
            {
                Conexion.AbrirConexion(con);
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    Planta pl = new Planta()
                    {
                        Id = reader.GetInt32(0),
                        NombreCientifico = reader.GetString(2),
                        Descripcion = reader.GetString(6),
                        AmbientePlanta = (Planta.Ambiente)Enum.Parse(typeof(Planta.Ambiente), reader.GetString(reader.GetOrdinal("AmbientePlanta"))),
                        AlturaMax = reader.GetDecimal(3),
                        Foto = reader.GetString(4)
                    };
                   // pl.Tipo = CrearTipo(reader);
                    pl.NombresVulgares = BuscarNombresVulgaresPlanta(pl.Id);

                    plantas.Add(pl);
                }
                Conexion.CerrarConexion(con);
            }
            catch(Exception e)
            {
                throw;
            }
            finally
            {
                Conexion.CerrarConexion(con);
            }

            return plantas;
        }


        public IEnumerable<Planta> BuscarPlantaAlturaMinima(decimal alturaIngresada)
        {
            List<Planta> plantas = new List<Planta>();
            SqlConnection con = Conexion.ObtenerConexion();

            string sql = "SELECT * FROM Planta  WHERE AlturaMax <" + alturaIngresada;
            SqlCommand com = new SqlCommand(sql, con);

            try
            {
                Conexion.AbrirConexion(con);
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    Planta pl = new Planta()
                    {
                        Id = reader.GetInt32(0),
                        NombreCientifico = reader.GetString(2),
                        Descripcion = reader.GetString(6),
                        AmbientePlanta = (Planta.Ambiente)Enum.Parse(typeof(Planta.Ambiente), reader.GetString(reader.GetOrdinal("AmbientePlanta"))),
                        AlturaMax = reader.GetDecimal(3),
                        Foto = reader.GetString(4)
                    };
                    pl.NombresVulgares = BuscarNombresVulgaresPlanta(pl.Id);
                    plantas.Add(pl);
                }
                Conexion.CerrarConexion(con);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                Conexion.CerrarConexion(con);
            }

            return plantas;

        }


        public IEnumerable<Planta> BuscarPlantaAlturaMaxima(decimal alturaIngresada)
        {
            List<Planta> plantas = new List<Planta>();
            SqlConnection con = Conexion.ObtenerConexion();

            string sql = "SELECT * FROM Planta  WHERE AlturaMax >=" + alturaIngresada;
            SqlCommand com = new SqlCommand(sql, con);

            try
            {
                Conexion.AbrirConexion(con);
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    Planta pl = new Planta()
                    {
                        Id = reader.GetInt32(0),
                        NombreCientifico = reader.GetString(2),
                        Descripcion = reader.GetString(6),
                        AmbientePlanta = (Planta.Ambiente)Enum.Parse(typeof(Planta.Ambiente), reader.GetString(reader.GetOrdinal("AmbientePlanta"))),
                        AlturaMax = reader.GetDecimal(3),
                        Foto = reader.GetString(4)
                    };
                    pl.NombresVulgares = BuscarNombresVulgaresPlanta(pl.Id);
                    plantas.Add(pl);
                }
                Conexion.CerrarConexion(con);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                Conexion.CerrarConexion(con);
            }

            return plantas;



        }

        public IEnumerable<Planta> BuscarPlantaPorAmbiente(int AmbientePlanta)
        {
            List<Planta> plantas = new List<Planta>();
            SqlConnection con = Conexion.ObtenerConexion();
                    

            string sql = "SELECT * FROM Planta  WHERE AmbientePlanta = " + " '" + AmbientePlanta + "' ";
            SqlCommand com = new SqlCommand(sql, con);

            try
            {
                Conexion.AbrirConexion(con);
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    Planta pl = new Planta()
                    {
                        Id = reader.GetInt32(0),
                        NombreCientifico = reader.GetString(2),
                        Descripcion = reader.GetString(6),
                        AmbientePlanta = (Planta.Ambiente)Enum.Parse(typeof(Planta.Ambiente), reader.GetString(reader.GetOrdinal("AmbientePlanta"))),
                        AlturaMax = reader.GetDecimal(3),
                        Foto = reader.GetString(4)
                    };
                    pl.NombresVulgares = BuscarNombresVulgaresPlanta(pl.Id);
                    plantas.Add(pl);
                }
                Conexion.CerrarConexion(con);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                Conexion.CerrarConexion(con);
            }

            return plantas;
        }
    }
}
