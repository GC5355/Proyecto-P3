using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.EntidadesNegocio
{
    public class Usuario : IValidable
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool SoyValido()
        {
            bool devuelvo = false;
            bool mayuscula = false;
            bool minuscula = false;
            bool numero = false;

            if (Password.Length >= 6 && Email != "" &&Email !=null)

                for (int i = 0; i < Password.Length; i++)
                {
                    if (char.IsUpper(Password[i]))
                    {
                        mayuscula = true;
                    }
                    if (char.IsLower(Password[i]))
                    {
                        minuscula = true;
                    }
                    if (char.IsDigit(Password[i]))
                    {
                        numero = true;
                    }
                    if (mayuscula && minuscula && numero)
                    {
                        devuelvo = true;
                    }

                }

            return devuelvo;
        }

        public override string ToString()
        {
            return "Id: " + Id + " | " + "Email: " + Email + " | " + "Password: " + Password;
        }
    }
}
