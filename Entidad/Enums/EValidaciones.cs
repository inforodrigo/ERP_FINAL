using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad.Enums
{
    public class EValidaciones
    {
        protected const string ExpLtrNro = @"^[A-Za-z0-9 _][A-Za-z0-9][A-Za-z0-9 _]$";
        protected const string MsgLtrNro = "Solo Letras y Numero";

        protected const string ExpLtrNroEspGui = @"^[A-Za-z0-9 _-][A-Za-z0-9][A-Za-z0-9 _-]$";
        protected const string MsgExpLtrNroEspGui = "Solo Letras , Numero y Guion";

        protected const string ExpNumber = @"^[0-9]*$";
        protected const string MsgNumber = "Solo Numero";

        protected const string ExpNroD10 = @"\d{0,10}";
        protected const string MsgNroD10 = "Maximo 10 Digitos";

        protected const string ExpLetras = @"^[a-zA-Z _]+$";
        protected const string MsgLetras = "Solo Letras";

        protected const string ExpEmail = @"^[^@]+@[^@]+\.[a-zA-Z]{2,}$";
        protected const string MsgEmail = "No es un E-mail valido";
        protected const string MsgObligatorio = "Este Campo es Obligarorio";
    }
}
