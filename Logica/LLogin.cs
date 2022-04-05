using Data;
using Entidad;
using Entidad.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LLogin : LBase<usuario>
    {
        EUsuario e = new EUsuario();
        public EUsuario IniciarSesion(ELogin objLogin)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var login = (from x in esquema.usuario
                                 where x.estado != (int)Estado.Eliminado &&
                                 x.nickname == objLogin.Nickname &&
                                 x.pass == objLogin.Pass
                                 select x).FirstOrDefault();

                    if (login == null)
                    {
                        throw new BussinessException("El Correo o Contraseña son incorrectos.");
                    }
                    else
                    {
                        EUsuario log = new EUsuario();
                        log.Id = login.id;
                        log.Nombre = login.nombre;
                        log.Nickname = login.nickname;
                        log.Tipo = login.tipo;
                        log.Estado = login.estado;

                        return log;
                    }
                }
            }
            catch (BussinessException ex)
            {
                throw new BussinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
