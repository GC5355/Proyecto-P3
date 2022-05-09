using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Repositorios
{
    public class Conexion
    {
        public static string ObtenerStringConexion()
        {
            string strCon = "";

            ConfigurationBuilder cb = new ConfigurationBuilder();
            cb.AddJsonFile("appsettings.json");
            IConfiguration configuracion = cb.Build();

            strCon = configuracion.GetConnectionString("Conexion1");

            return strCon;
        }

        public static SqlConnection ObtenerConexion()
        {
            string strCon = ObtenerStringConexion();
            return new SqlConnection(strCon);
        }

        public static void AbrirConexion(SqlConnection con)
        {
            if (con != null && con.State != ConnectionState.Open)
            {
                con.Open();
            }
        }

        public static void CerrarYDisposeConexion(SqlConnection con)
        {
            CerrarConexion(con);
            con.Dispose();
        }

        public static void CerrarConexion(SqlConnection con)
        {
            if (con != null && con.State != ConnectionState.Closed)
            {
                con.Close();
            }
        }
    }
}
