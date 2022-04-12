using Data;
using Entidad;
using Entidad.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logica
{
    public class LEmpresa : LBase<empresa>
    {
        public EEmpresa Covertir(empresa E)
        {
            EEmpresa emp = new EEmpresa();
            emp.Id = E.id;
            emp.Nombre = E.nombre;
            emp.Nit = E.nit;
            emp.Sigla = E.sigla;
            emp.Telefono = (int)E.telefono;
            emp.Correo = E.correo;
            emp.Direccion = E.direccion;
            emp.Niveles = E.niveles;
            emp.idUsuario = E.idUsuario;
            emp.Usuario = E.usuario.nombre;
            emp.Estado = E.estado;

            return emp;
        }

        public empresa Obtener_empresa(dbErpEntities esquema, long id)
        {
            return (from x in esquema.empresa
                    where x.id == id
                    select x).FirstOrDefault();
        }
        public List<EEmpresa> Obtener(int ordenar)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    List<EEmpresa> empresas = new List<EEmpresa>();
                    if (ordenar == 0)
                    {
                        var resultado = (from x in esquema.empresa
                                         where x.estado != (int)Estado.Eliminado
                                         select x).ToList();

                        foreach (var item in resultado)
                        {
                            EEmpresa e = new EEmpresa();
                            e.Id = item.id;
                            e.Nombre = item.nombre;
                            e.Nit = item.nit;
                            e.Sigla = item.sigla;
                            e.Telefono = (int)item.telefono;
                            e.Correo = item.correo;
                            e.Direccion = item.direccion;
                            e.Niveles = item.niveles;
                            e.idUsuario = item.idUsuario;
                            e.Usuario = item.usuario.nombre;
                            e.Estado = item.estado;
                            empresas.Add(e);
                        }

                    }
                    else
                    {
                        var resultado = (from x in esquema.empresa
                                         where x.estado != (int)Estado.Eliminado
                                         orderby x.id descending
                                         select x).ToList();


                        foreach (var item in resultado)
                        {
                            EEmpresa e = new EEmpresa();
                            e.Id = item.id;
                            e.Nombre = item.nombre;
                            e.Nit = item.nit;
                            e.Sigla = item.sigla;
                            e.Telefono = (int)item.telefono;
                            e.Correo = item.correo;
                            e.Direccion = item.direccion;
                            e.Niveles = item.niveles;
                            e.idUsuario = item.idUsuario;
                            e.Usuario = item.usuario.nombre;
                            e.Estado = item.estado;
                            empresas.Add(e);
                        }

                    }




                    return empresas;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EEmpresa ObtenerPorid(int id)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.empresa
                                     where x.estado != (int)Estado.Eliminado
                                     where x.id == id
                                     select x).FirstOrDefault();

                    EEmpresa emp = new EEmpresa();
                    emp.Id = resultado.id;
                    emp.Nombre = resultado.nombre;
                    emp.Nit = resultado.nit;
                    emp.Sigla = resultado.sigla;
                    emp.Telefono = (int)resultado.telefono;
                    emp.Correo = resultado.correo;
                    emp.Direccion = resultado.direccion;
                    emp.Niveles = resultado.niveles;
                    emp.idUsuario = resultado.idUsuario;
                    emp.Usuario = resultado.usuario.nombre;
                    emp.Estado = resultado.estado;

                    return emp;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EEmpresa ObtenerPorNit(string nit)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.empresa
                                     where x.estado != (int)Estado.Eliminado
                                     where x.nit == nit
                                     select x).FirstOrDefault();

                    EEmpresa emp = new EEmpresa();
                    emp.Id = resultado.id;
                    emp.Nombre = resultado.nombre;
                    emp.Nit = resultado.nit;
                    emp.Sigla = resultado.sigla;
                    emp.Telefono = (int)resultado.telefono;
                    emp.Correo = resultado.correo;
                    emp.Direccion = resultado.direccion;
                    emp.Niveles = resultado.niveles;
                    emp.idUsuario = resultado.idUsuario;
                    emp.Usuario = resultado.usuario.nombre;
                    emp.Estado = resultado.estado;

                    return emp;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<EMoneda> ObtenerMonedas(int id)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    List<EMoneda> monedas = new List<EMoneda>();

                    var resultado = (from x in esquema.moneda
                                     where x.idUsuario == id
                                     select x).ToList();

                    foreach (var item in resultado)
                    {
                        EMoneda mon = new EMoneda();
                        mon.Id = item.id;
                        mon.Nombre = item.nombre;
                        monedas.Add(mon);
                    }


                    return monedas;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int VerificarMonedas(int id)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.empresaMoneda
                                     where x.idEmpresa == id
                                     select x).ToList();
                    var result = 0;
                    if (resultado.Count > 1)
                    {
                        result = 2;
                    }
                    else
                    {
                        result = 1;
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EEmpresa Agregar(EEmpresa objempresa, EEmpresaMoneda objEmoneda)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var ExisteNombre = (from x in esquema.empresa
                                        where x.estado != (int)Estado.Eliminado &&
                                        x.nombre.ToUpper() == objempresa.Nombre.ToUpper()
                                        select x).ToList();

                    if (ExisteNombre.Count > 0)
                    {
                        throw new BussinessException("El nombre de la empresa ya ha sido registrado.");
                    }


                    var Existenit = (from x in esquema.empresa
                                     where x.estado != (int)Estado.Eliminado &&
                                     x.nit == objempresa.Nit
                                     select x).ToList();

                    if (Existenit.Count > 0)
                    {
                        throw new BussinessException("El Nit ya ha sido registrado.");
                    }

                    var Existesigla = (from x in esquema.empresa
                                       where x.estado != (int)Estado.Eliminado &&
                                       x.sigla == objempresa.Sigla
                                       select x).ToList();

                    if (Existesigla.Count > 0)
                    {
                        throw new BussinessException("Las siglas ya han sido registradas.");
                    }

                    string email = objempresa.Correo;
                    var res = 0;
                    if (email != "")
                    {

                        String expresion = @"^[^@]+@[^@]+\.[a-zA-Z]{2,}$";
                        if (Regex.IsMatch(email, expresion))
                        {
                            if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                            {
                                res = 1;
                            }
                            else
                            {
                                res = 2;
                            }
                        }
                        else
                        {
                            res = 2;
                        }
                    }

                    if (res == 2)
                    {
                        throw new BussinessException("El Correo no tiene el formato correcto.");
                    }



                    empresa empresa = new empresa();
                    empresa.nombre = objempresa.Nombre;
                    empresa.nit = objempresa.Nit;
                    empresa.sigla = objempresa.Sigla;
                    empresa.telefono = objempresa.Telefono;
                    empresa.correo = objempresa.Correo;
                    empresa.direccion = objempresa.Direccion;
                    empresa.niveles = objempresa.Niveles;
                    empresa.idUsuario = objempresa.idUsuario;
                    empresa.estado = 0;
                    esquema.empresa.Add(empresa);
                    esquema.SaveChanges();

                    EEmpresa emp = ObtenerPorNit(objempresa.Nit);

                    empresaMoneda emoneda = new empresaMoneda();
                    emoneda.activo = 1;
                    emoneda.fechaRegistro = DateTime.Now;
                    emoneda.idEmpresa = emp.Id;
                    emoneda.idMonedaPrincipal = objEmoneda.idMonedaPrincipal;
                    emoneda.idUsuario = objEmoneda.idUsuario;
                    esquema.empresaMoneda.Add(emoneda);
                    esquema.SaveChanges();

                    AgregarCuentaActivo(emp.Id, objempresa.Niveles, objempresa.idUsuario);
                    AgregarCuentaPasivo(emp.Id, objempresa.Niveles, objempresa.idUsuario);
                    AgregarCuentaPatrimonio(emp.Id, objempresa.Niveles, objempresa.idUsuario);
                    AgregarCuentaIngresos(emp.Id, objempresa.Niveles, objempresa.idUsuario);
                    AgregarCuentaEgresos(emp.Id, objempresa.Niveles, objempresa.idUsuario);
                    AgregarCuentaCostos(emp.Id, objempresa.Niveles, objempresa.idUsuario);
                    AgregarCuentaGastos(emp.Id, objempresa.Niveles, objempresa.idUsuario);

                    return objempresa;
                }
            }
            catch (BussinessException ex)
            {
                throw new BussinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public EEmpresa Editar(EEmpresa objempresa)
        {
            try
            {
                using (var esquema = GetEsquema())
                {

                    var ExisteNombre = (from x in esquema.empresa
                                        where x.estado != (int)Estado.Eliminado &&
                                        x.id != objempresa.Id &&
                                        x.nombre.ToUpper() == objempresa.Nombre.ToUpper()
                                        select x).ToList();

                    if (ExisteNombre.Count > 0)
                    {
                        throw new BussinessException("El nombre de la empresa ya ha sido registrado.");
                    }


                    var Existenit = (from x in esquema.empresa
                                     where x.estado != (int)Estado.Eliminado &&
                                     x.nit == objempresa.Nit
                                     && x.id != objempresa.Id
                                     select x).ToList();

                    if (Existenit.Count > 0)
                    {
                        throw new BussinessException("El Nit ya ha sido registrado.");
                    }

                    var Existesigla = (from x in esquema.empresa
                                       where x.estado != (int)Estado.Eliminado &&
                                       x.sigla == objempresa.Sigla
                                       && x.id != objempresa.Id
                                       select x).ToList();

                    if (Existesigla.Count > 0)
                    {
                        throw new BussinessException("Las siglas ya han sido registradas.");
                    }

                    string email = objempresa.Correo;
                    var res = 0;
                    if (email != "")
                    {

                        String expresion = @"^[^@]+@[^@]+\.[a-zA-Z]{2,}$";
                        if (Regex.IsMatch(email, expresion))
                        {
                            if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                            {
                                res = 1;
                            }
                            else
                            {
                                res = 2;
                            }
                        }
                        else
                        {
                            res = 2;
                        }
                    }

                    if (res == 2)
                    {
                        throw new BussinessException("El Correo no tiene el formato correcto.");
                    }


                    empresa empresa = Obtener_empresa(esquema, objempresa.Id);
                    empresa.nombre = objempresa.Nombre;
                    empresa.nit = objempresa.Nit;
                    empresa.sigla = objempresa.Sigla;
                    empresa.telefono = objempresa.Telefono;
                    empresa.correo = objempresa.Correo;
                    empresa.direccion = objempresa.Direccion;
                    esquema.SaveChanges();

                    return objempresa;
                }
            }
            catch (BussinessException ex)
            {
                throw new BussinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Eliminar(int id)
        {
            using (var esquema = GetEsquema())
            {
                var empresa = (from x in esquema.empresa
                               where x.id == id
                               select x).FirstOrDefault();

                try
                {
                    empresa.estado = 2;
                    esquema.SaveChanges();
                    return true;
                }
                catch (BussinessException ex)
                {
                    return false;
                }

            }
        }

        public bool AgregarCuentaActivo(int idEmpresa, int nivel, int idUsuario)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var codigo = "";
                    switch (nivel)
                    {

                        case 3:
                            codigo = "1.0.0";
                            break;
                        case 4:
                            codigo = "1.0.0.0";
                            break;
                        case 5:
                            codigo = "1.0.0.0.0";
                            break;
                        case 6:
                            codigo = "1.0.0.0.0.0";
                            break;
                        case 7:
                            codigo = "1.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }

                    cuenta cuenta = new cuenta();
                    cuenta.codigo = codigo;
                    cuenta.nombre = "Activo";
                    cuenta.nivel = 1;
                    cuenta.tipocuenta = 1;
                    cuenta.estado = 0;
                    cuenta.idUsuario = idUsuario;
                    cuenta.idEmpresa = idEmpresa;
                    esquema.cuenta.Add(cuenta);
                    esquema.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool AgregarCuentaPasivo(int idEmpresa,int nivel, int idUsuario)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var codigo = "";
                    switch (nivel)
                    {

                        case 3:
                            codigo = "2.0.0";
                            break;
                        case 4:
                            codigo = "2.0.0.0";
                            break;
                        case 5:
                            codigo = "2.0.0.0.0";
                            break;
                        case 6:
                            codigo = "2.0.0.0.0.0";
                            break;
                        case 7:
                            codigo = "2.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }

                    cuenta cuenta = new cuenta();
                    cuenta.codigo = codigo;
                    cuenta.nombre = "Pasivo";
                    cuenta.nivel = 1;
                    cuenta.tipocuenta = 1;
                    cuenta.estado = 0;
                    cuenta.idUsuario = idUsuario;
                    cuenta.idEmpresa = idEmpresa;
                    esquema.cuenta.Add(cuenta);
                    esquema.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool AgregarCuentaPatrimonio(int idEmpresa, int nivel, int idUsuario)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var codigo = "";
                    switch (nivel)
                    {

                        case 3:
                            codigo = "3.0.0";
                            break;
                        case 4:
                            codigo = "3.0.0.0";
                            break;
                        case 5:
                            codigo = "3.0.0.0.0";
                            break;
                        case 6:
                            codigo = "3.0.0.0.0.0";
                            break;
                        case 7:
                            codigo = "3.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }

                    cuenta cuenta = new cuenta();
                    cuenta.codigo = codigo;
                    cuenta.nombre = "Patrimonio";
                    cuenta.nivel = 1;
                    cuenta.tipocuenta = 1;
                    cuenta.estado = 0;
                    cuenta.idUsuario = idUsuario;
                    cuenta.idEmpresa = idEmpresa;
                    esquema.cuenta.Add(cuenta);
                    esquema.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool AgregarCuentaIngresos(int idEmpresa, int nivel, int idUsuario)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var codigo = "";
                    switch (nivel)
                    {

                        case 3:
                            codigo = "4.0.0";
                            break;
                        case 4:
                            codigo = "4.0.0.0";
                            break;
                        case 5:
                            codigo = "4.0.0.0.0";
                            break;
                        case 6:
                            codigo = "4.0.0.0.0.0";
                            break;
                        case 7:
                            codigo = "4.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }

                    cuenta cuenta = new cuenta();
                    cuenta.codigo = codigo;
                    cuenta.nombre = "Ingresos";
                    cuenta.nivel = 1;
                    cuenta.tipocuenta = 1;
                    cuenta.estado = 0;
                    cuenta.idUsuario = idUsuario;
                    cuenta.idEmpresa = idEmpresa;
                    esquema.cuenta.Add(cuenta);
                    esquema.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool AgregarCuentaEgresos(int idEmpresa, int nivel, int idUsuario)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var codigo = "";
                    switch (nivel)
                    {

                        case 3:
                            codigo = "5.0.0";
                            break;
                        case 4:
                            codigo = "5.0.0.0";
                            break;
                        case 5:
                            codigo = "5.0.0.0.0";
                            break;
                        case 6:
                            codigo = "5.0.0.0.0.0";
                            break;
                        case 7:
                            codigo = "5.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }

                    cuenta cuenta = new cuenta();
                    cuenta.codigo = codigo;
                    cuenta.nombre = "Egresos";
                    cuenta.nivel = 1;
                    cuenta.tipocuenta = 1;
                    cuenta.estado = 0;
                    cuenta.idUsuario = idUsuario;
                    cuenta.idEmpresa = idEmpresa;
                    esquema.cuenta.Add(cuenta);
                    esquema.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool AgregarCuentaCostos(int idEmpresa, int nivel, int idUsuario)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var cuentapadre = (from x in esquema.cuenta
                                                    where x.idEmpresa == idEmpresa
                                                    select x).FirstOrDefault(); 
                    var codigo = "";
                    switch (nivel)
                    {

                        case 3:
                            codigo = "5.1.0";
                            cuentapadre = (from x in esquema.cuenta
                                           where x.idEmpresa == idEmpresa &&
                                           x.codigo == "5.0.0"
                                           select x).FirstOrDefault();
                            break;
                        case 4:
                            codigo = "5.1.0.0";
                            cuentapadre = (from x in esquema.cuenta
                                           where x.idEmpresa == idEmpresa &&
                                           x.codigo == "5.0.0.0"
                                           select x).FirstOrDefault();
                            break;
                        case 5:
                            codigo = "5.1.0.0.0";
                            cuentapadre = (from x in esquema.cuenta
                                           where x.idEmpresa == idEmpresa &&
                                           x.codigo == "5.0.0.0.0"
                                           select x).FirstOrDefault();
                            break;
                        case 6:
                            codigo = "5.1.0.0.0.0";
                            cuentapadre = (from x in esquema.cuenta
                                           where x.idEmpresa == idEmpresa &&
                                           x.codigo == "5.0.0.0.0.0"
                                           select x).FirstOrDefault();
                            break;
                        case 7:
                            codigo = "5.1.0.0.0.0.0";
                            cuentapadre = (from x in esquema.cuenta
                                           where x.idEmpresa == idEmpresa &&
                                           x.codigo == "5.0.0.0.0.0.0"
                                           select x).FirstOrDefault();
                            break;

                        default:
                            break;
                    }

                    cuenta cuenta = new cuenta();
                    cuenta.codigo = codigo;
                    cuenta.nombre = "Costos";
                    cuenta.nivel = 2;
                    cuenta.tipocuenta = 1;
                    cuenta.estado = 0;
                    cuenta.idUsuario = idUsuario;
                    cuenta.idEmpresa = idEmpresa;
                    cuenta.idCuentaPadre = cuentapadre.id;
                    esquema.cuenta.Add(cuenta);
                    esquema.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool AgregarCuentaGastos(int idEmpresa, int nivel, int idUsuario)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var cuentapadre = (from x in esquema.cuenta
                                       where x.idEmpresa == idEmpresa
                                       select x).FirstOrDefault();
                    var codigo = "";
                    switch (nivel)
                    {

                        case 3:
                            codigo = "5.2.0";
                            cuentapadre = (from x in esquema.cuenta
                                           where x.idEmpresa == idEmpresa &&
                                           x.codigo == "5.0.0"
                                           select x).FirstOrDefault();
                            break;
                        case 4:
                            codigo = "5.2.0.0";
                            cuentapadre = (from x in esquema.cuenta
                                           where x.idEmpresa == idEmpresa &&
                                           x.codigo == "5.0.0.0"
                                           select x).FirstOrDefault();
                            break;
                        case 5:
                            codigo = "5.2.0.0.0";
                            cuentapadre = (from x in esquema.cuenta
                                           where x.idEmpresa == idEmpresa &&
                                           x.codigo == "5.0.0.0.0"
                                           select x).FirstOrDefault();
                            break;
                        case 6:
                            codigo = "5.2.0.0.0.0";
                            cuentapadre = (from x in esquema.cuenta
                                           where x.idEmpresa == idEmpresa &&
                                           x.codigo == "5.0.0.0.0.0"
                                           select x).FirstOrDefault();
                            break;
                        case 7:
                            codigo = "5.2.0.0.0.0.0";
                            cuentapadre = (from x in esquema.cuenta
                                           where x.idEmpresa == idEmpresa &&
                                           x.codigo == "5.0.0.0.0.0.0"
                                           select x).FirstOrDefault();
                            break;

                        default:
                            break;
                    }

                    cuenta cuenta = new cuenta();
                    cuenta.codigo = codigo;
                    cuenta.nombre = "Gastos";
                    cuenta.nivel = 2;
                    cuenta.tipocuenta = 1;
                    cuenta.estado = 0;
                    cuenta.idUsuario = idUsuario;
                    cuenta.idEmpresa = idEmpresa;
                    cuenta.idCuentaPadre = cuentapadre.id;
                    esquema.cuenta.Add(cuenta);
                    esquema.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
    }
}
