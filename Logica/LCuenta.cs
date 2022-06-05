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
    public class LCuenta : LBase<gestion>
    {
        public List<ECuenta> listarCuentas(int idusuario, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.cuenta
                                      where x.idEmpresa == idempresa
                                      && x.idCuentaPadre == null
                                      select x).ToList();
                 
                    List<ECuenta> cuentas = new List<ECuenta>();


                    foreach (var i in linqcuenta)
                    {
                        ECuenta cuenta = new ECuenta();
                        cuenta.Id = i.id;
                        cuenta.IdCuenta = i.id;
                        cuenta.Codigo = i.codigo;
                        cuenta.Nombre = i.nombre;
                        cuenta.text = i.codigo + " " + i.nombre;
                        cuenta.TipoCuenta = i.tipocuenta;
                        cuenta.Nivel = i.nivel;
                        cuenta.IdUsuario = (int)i.idUsuario;
                        cuenta.IdEmpresa = (int)i.idEmpresa;
                        if(i.idCuentaPadre != null)
                        {
                            cuenta.IdCuentaPadre = (int)i.idCuentaPadre;
                        }                                                
                        cuenta.children = CuentaHijos(cuenta.Id, idempresa);
                        cuentas.Add(cuenta);
                    }
                   
                    return cuentas;
                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se puedo obtener la lista de empresas");
                }
            }
        }

        public List<ECuenta> CuentaHijos(long idPadre, long idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.cuenta
                                      where x.idEmpresa == idempresa
                                      && x.idCuentaPadre == idPadre
                                      select x).ToList();


                    List<ECuenta> cuentas = new List<ECuenta>();                  

                    foreach (var i in linqcuenta)
                    {
                        ECuenta cuenta = new ECuenta();
                        cuenta.Id = i.id;
                        cuenta.IdCuenta = i.id;
                        cuenta.Codigo = i.codigo;
                        cuenta.Nombre = i.nombre;
                        cuenta.text = i.codigo + " " + i.nombre;
                        cuenta.TipoCuenta = i.tipocuenta;
                        cuenta.Nivel = i.nivel;
                        cuenta.IdUsuario = (int)i.idUsuario;
                        cuenta.IdEmpresa = (int)i.idEmpresa;
                        cuenta.IdCuentaPadre = (int)i.idCuentaPadre;
                        cuenta.children = CuentaHijos(cuenta.Id, idempresa);
                        cuentas.Add(cuenta);
                    }
                    return cuentas;
                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se puedo obtener la lista de empresas");
                }
            }
        }

        public ECuenta obtenerCuenta(long idcuenta)
        {

            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.cuenta
                                      where x.id == idcuenta
                                      select x).FirstOrDefault();

                    if (linqcuenta == null)
                    {
                        throw new BussinessException("Error no se pudo obtener la cuenta");
                    }
                              
                    ECuenta e = new ECuenta();

                    e.Id = linqcuenta.id;
                    e.Codigo = linqcuenta.codigo;
                    e.Nombre = linqcuenta.nombre;
                    e.Nivel = linqcuenta.nivel;
                    e.TipoCuenta = linqcuenta.tipocuenta;
                    e.Estado = linqcuenta.estado;
                    e.IdUsuario = (int)linqcuenta.idUsuario;
                    e.IdEmpresa = (int)linqcuenta.idEmpresa;
                    if (linqcuenta.idCuentaPadre != null)
                    {
                        e.IdCuentaPadre = (int)linqcuenta.idCuentaPadre;
                    }

                    return e;

                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se puedo obtener la cuenta");
                }

            }
        }

        public List<ECuenta> ObtenerPorIdEmpresa(int id)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.cuenta
                                     where x.estado != (int)Estado.Eliminado
                                     && x.idEmpresa == id
                                     select x).ToList();

                    List<ECuenta> cuentas = new List<ECuenta>();
                    foreach (var item in resultado)
                    {
                        ECuenta e = new ECuenta();
                        e.Id = item.id;
                        e.Codigo = item.codigo;
                        e.Nombre = item.nombre;
                        e.Nivel = item.nivel;
                        e.TipoCuenta = item.tipocuenta;
                        e.Estado = item.estado;
                        e.IdUsuario = (int)item.idUsuario;
                        e.IdEmpresa = (int)item.idEmpresa;
                        e.IdCuentaPadre = (int)item.idCuentaPadre;
                        cuentas.Add(e);
                    }

                    return cuentas;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ECuenta> ObtenerCuentasConfiguracion(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var consulta = (from x in esquema.cuenta
                                    where x.estado == (int)Estado.Habilitado
                                    && x.idEmpresa == idempresa                                    
                                    && x.tipocuenta == (int)ETipoCuentas.Detallado
                                    select x).ToList();

                    List<ECuenta> cuentas = new List<ECuenta>();


                    foreach (var item in consulta)
                    {
                        ECuenta cuenta = new ECuenta();
                        cuenta.Id = item.id;
                        cuenta.Codigo = item.codigo;
                        cuenta.Nombre = item.nombre;
                        cuenta.IdCuentaPadre = (int)item.idCuentaPadre;

                        cuentas.Add(cuenta);
                    }

                    return cuentas;
                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se puedo obtener la lista de cuentas");
                }
            }
        }

        public ECuenta Agregar(ECuenta objCuenta, int idcuenta, int padre)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var empresa = (from x in esquema.empresa
                                   where x.id == objCuenta.IdEmpresa
                                   select x).FirstOrDefault();

                    var ExisteNombre = (from x in esquema.cuenta
                                        where x.estado == (int)Estado.Habilitado &&
                                        x.idEmpresa == objCuenta.IdEmpresa &&
                                        x.nombre.ToUpper() == objCuenta.Nombre.ToUpper()
                                        select x).ToList();

                    if (ExisteNombre.Count > 0)
                    {
                        throw new BussinessException("El nombre de la cuenta no esta disponible.");
                    }

                    var linqcuenta = (from x in esquema.cuenta
                                      where x.idEmpresa == objCuenta.IdEmpresa
                                      select x).FirstOrDefault();


                    if (linqcuenta == null)
                    {
                        string codigoAux = "";

                        if (empresa != null)
                        {

                            switch (empresa.niveles)
                            {

                                case 3:
                                    codigoAux = "1.0.0";
                                    break;
                                case 4:
                                    codigoAux = "1.0.0.0";
                                    break;
                                case 5:
                                    codigoAux = "1.0.0.0.0";
                                    break;
                                case 6:
                                    codigoAux = "1.0.0.0.0.0";
                                    break;
                                case 7:
                                    codigoAux = "1.0.0.0.0.0.0";
                                    break;

                                default:
                                    break;
                            }

                            objCuenta.Codigo = codigoAux;
                            objCuenta.Nivel = 1;

                            cuenta cuenta = new cuenta();
                            cuenta.nombre = objCuenta.Nombre;
                            cuenta.codigo = objCuenta.Codigo;
                            cuenta.nivel = objCuenta.Nivel;
                            cuenta.tipocuenta = objCuenta.TipoCuenta;
                            cuenta.estado = 0;
                            cuenta.idUsuario = objCuenta.IdUsuario;
                            cuenta.idEmpresa = objCuenta.IdEmpresa;                          

                            esquema.cuenta.Add(cuenta);
                            esquema.SaveChanges();
                        }
                    }
                    else
                    {
                        if (empresa != null)
                        {

                            if (padre == 0)
                            {
                                //hijos
                                var cuentapadre = (from x in esquema.cuenta
                                                   where x.idEmpresa == objCuenta.IdEmpresa
                                                   && x.id == idcuenta
                                                   select x).FirstOrDefault();

                                if (cuentapadre != null)
                                {
                                    int codigo = 0;
                                    string aux = "";
                                    int encuentra = 0;

                                    var hijos = (from x in esquema.cuenta
                                                 where x.idEmpresa == objCuenta.IdEmpresa
                                                 && x.idCuentaPadre == idcuenta
                                                 orderby x.id descending
                                                 select x).FirstOrDefault();
                                    if (hijos != null)
                                    {
                                        string[] valores = hijos.codigo.Split('.');

                                        for (int i = 0; i < valores.Count(); i++)
                                        {
                                            if (i == (hijos.nivel - 1))
                                            {                                               
                                                codigo = Int32.Parse(valores[i]) + 1;
                                                encuentra = 1;
                                                valores[i] = codigo.ToString();
                                               
                                                if (i == (valores.Count() - 1))
                                                {
                                                    objCuenta.TipoCuenta = (int)ETipoCuentas.Detallado;
                                                }
                                            }

                                            aux = aux + valores[i] + ".";
                                        }

                                        if (encuentra == 0)
                                        {
                                            throw new BussinessException("No se puede registrar por que sobrepasa el nivel de la empresa");
                                        }

                                        objCuenta.Codigo = aux.Remove(aux.Length - 1);
                                        objCuenta.Nivel = hijos.nivel;

                                        cuenta cuenta = new cuenta();
                                        cuenta.nombre = objCuenta.Nombre;
                                        cuenta.codigo = objCuenta.Codigo;
                                        cuenta.nivel = objCuenta.Nivel;
                                        cuenta.tipocuenta = objCuenta.TipoCuenta;
                                        cuenta.estado = 0;
                                        cuenta.idUsuario = objCuenta.IdUsuario;
                                        cuenta.idEmpresa = objCuenta.IdEmpresa;
                                        cuenta.idCuentaPadre = objCuenta.IdCuentaPadre;

                                        esquema.cuenta.Add(cuenta);
                                        esquema.SaveChanges();
                                       
                                    }
                                    else
                                    {

                                        string[] valores = cuentapadre.codigo.Split('.');

                                        for (int i = 0; i < valores.Count(); i++)
                                        {
                                            if (i == (cuentapadre.nivel))
                                            {
                                                codigo = Int32.Parse(valores[i]) + 1;
                                                encuentra = 1;
                                                valores[i] = codigo.ToString();                                               

                                                if (i == (valores.Count() - 1))
                                                {
                                                    objCuenta.TipoCuenta = (int)ETipoCuentas.Detallado;
                                                }
                                            }

                                            aux = aux + valores[i] + ".";

                                        }

                                        if (encuentra == 0)
                                        {
                                            throw new BussinessException("No se puede registrar por que sobrepasa el nivel de la empresa");
                                        }

                                        objCuenta.Codigo = aux.Remove(aux.Length - 1);
                                        objCuenta.Nivel = cuentapadre.nivel + 1;

                                        cuenta cuenta = new cuenta();
                                        cuenta.nombre = objCuenta.Nombre;
                                        cuenta.codigo = objCuenta.Codigo;
                                        cuenta.nivel = objCuenta.Nivel;
                                        cuenta.tipocuenta = objCuenta.TipoCuenta;
                                        cuenta.estado = 0;
                                        cuenta.idUsuario = objCuenta.IdUsuario;
                                        cuenta.idEmpresa = objCuenta.IdEmpresa;
                                        cuenta.idCuentaPadre = objCuenta.IdCuentaPadre;

                                        esquema.cuenta.Add(cuenta);
                                        esquema.SaveChanges();
                                    }
                                }
                            }
                            else if (padre == 1)
                            {
                                //agregar padres

                                var linqpadres = (from x in esquema.cuenta
                                                  where x.idEmpresa == objCuenta.IdEmpresa
                                                  && x.nivel == 1
                                                  orderby x.id descending
                                                  select x).FirstOrDefault();

                                if (linqpadres != null)
                                {
                                    int codigo = 0;
                                    string aux = "";
                                    int encuentra = 0;


                                    string[] valores = linqpadres.codigo.Split('.');


                                    for (int i = 0; i < valores.Count(); i++)
                                    {

                                        if (i == (linqpadres.nivel - 1))
                                        {                                          
                                            codigo = Int32.Parse(valores[i]) + 1;
                                            encuentra = 1;
                                            valores[i] = codigo.ToString();                                           
                                        }

                                        aux = aux + valores[i] + ".";
                                    }

                                    if (encuentra == 0)
                                    {
                                        throw new BussinessException("No se puede registrar por que sobrepasa el nivel de la empresa");
                                    }

                                    objCuenta.Codigo = aux.Remove(aux.Length - 1);
                                    objCuenta.Nivel = linqpadres.nivel;

                                    cuenta cuenta = new cuenta();
                                    cuenta.nombre = objCuenta.Nombre;
                                    cuenta.codigo = objCuenta.Codigo;
                                    cuenta.nivel = objCuenta.Nivel;
                                    cuenta.tipocuenta = objCuenta.TipoCuenta;
                                    cuenta.estado = 0;
                                    cuenta.idUsuario = objCuenta.IdUsuario;
                                    cuenta.idEmpresa = objCuenta.IdEmpresa;                                   

                                    esquema.cuenta.Add(cuenta);
                                    esquema.SaveChanges();

                                }
                            }
                        }
                    }                 

                    return objCuenta;                 
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

        public ECuenta ModificarCuenta(int idcuenta, string nombre, int idempresa)
        {

            using (var esquema = GetEsquema())
            {

                try
                {

                    var validar = (from x in esquema.cuenta
                                   where x.idEmpresa == idempresa
                                   && x.id != idcuenta
                                   && x.nombre.Trim().ToUpper() == nombre.Trim().ToUpper()
                                   select x
                                   ).FirstOrDefault();

                    if (validar != null)
                    {
                        throw new BussinessException("El nombre de la cuenta en esta empresa ya existe");
                    }

                    var linqcuenta = (from x in esquema.cuenta
                                      where x.id == idcuenta
                                      select x).FirstOrDefault();

                    if (linqcuenta == null)
                    {
                        throw new BussinessException("Error no se puede obtener la cuenta");
                    }

                    linqcuenta.nombre = nombre;
                    esquema.SaveChanges();

                    ECuenta e = new ECuenta();

                    e.Id = linqcuenta.id;
                    e.Codigo = linqcuenta.codigo;
                    e.Nombre = linqcuenta.nombre;
                    e.Nivel = linqcuenta.nivel;
                    e.TipoCuenta = linqcuenta.tipocuenta;
                    e.Estado = linqcuenta.estado;
                    e.IdUsuario = (int)linqcuenta.idUsuario;
                    e.IdEmpresa = (int)linqcuenta.idEmpresa;
                    if (linqcuenta.idCuentaPadre != null)
                    {
                        e.IdCuentaPadre = (int)linqcuenta.idCuentaPadre;
                    }

                    return e;

                }
                catch (Exception ex)
                {
                    throw new BussinessException(ex.Message);
                }

            }
        }

        public ECuenta EliminarCuenta(int idcuenta)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    var linqcuenta = (from x in esquema.cuenta
                                      where x.id == idcuenta
                                      select x).FirstOrDefault();

                    if (linqcuenta == null)
                    {
                        throw new BussinessException("Error no se pudo obtener la cuenta");
                    }


                    if (linqcuenta.cuenta1.Count() > 0)
                    {
                        throw new BussinessException("No se puede eliminar la cuenta de esta empresa porque esta relacionada");

                    }               

                    esquema.cuenta.Remove(linqcuenta);
                    esquema.SaveChanges();

                    ECuenta e = new ECuenta();

                    e.Id = linqcuenta.id;
                    e.Codigo = linqcuenta.codigo;
                    e.Nombre = linqcuenta.nombre;
                    e.Nivel = linqcuenta.nivel;
                    e.TipoCuenta = linqcuenta.tipocuenta;
                    e.Estado = linqcuenta.estado;
                    e.IdUsuario = (int)linqcuenta.idUsuario;
                    e.IdEmpresa = (int)linqcuenta.idEmpresa;
                    if (linqcuenta.idCuentaPadre != null)
                    {
                        e.IdCuentaPadre = (int)linqcuenta.idCuentaPadre;
                    }

                    return e;

                }
                catch (Exception ex)
                {
                    throw new BussinessException(ex.Message);
                }

            }
        }
        public EConfiguracionIntegracion RegistrarConfiguracion(EConfiguracionIntegracion conf)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    confIntegracion integracion = new confIntegracion();
                    integracion.idEmpresa = conf.IdEmpresa;
                    integracion.integracion = conf.Integracion;
                    integracion.caja = conf.Caja;
                    integracion.creditoFiscal = conf.CreditoFiscal;
                    integracion.debitoFiscal = conf.DebitoFiscal;
                    integracion.compras = conf.Compras;
                    integracion.ventas = conf.Ventas;
                    integracion.it = conf.It;
                    integracion.itxPagar = conf.ItxPagar;
                    esquema.confIntegracion.Add(integracion);
                    esquema.SaveChanges();
                   

                    return conf;
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
        }

        public int VerificarConfiguracion(int idEmpresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    var consulta = (from x in esquema.confIntegracion
                                    where x.idEmpresa == idEmpresa                                   
                                    select x).FirstOrDefault();

                    var nro = 0;
                    if(consulta != null)
                    {
                        nro = 1;
                    }
                    else
                    {
                        nro = 0;
                    }                  

                    return nro;

                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se pudo verificar la configuracion");
                }
            }
        }

        public EConfiguracionIntegracion ObtenerConfiguracion(int idEmpresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    var consulta = (from x in esquema.confIntegracion
                                    where x.idEmpresa == idEmpresa
                                    select x).FirstOrDefault();

                    EConfiguracionIntegracion conf = new EConfiguracionIntegracion();
                    conf.Integracion = consulta.integracion;
                    conf.CajaStr = consulta.cuenta.codigo + " - " + consulta.cuenta.nombre;
                    conf.CreditoFiscalStr = consulta.cuenta1.codigo + " - " + consulta.cuenta1.nombre;
                    conf.DebitoFiscalStr = consulta.cuenta2.codigo + " - " + consulta.cuenta2.nombre;
                    conf.ComprasStr = consulta.cuenta3.codigo + " - " + consulta.cuenta3.nombre;
                    conf.VentasStr = consulta.cuenta4.codigo + " - " + consulta.cuenta4.nombre;
                    conf.ItStr = consulta.cuenta5.codigo + " - " + consulta.cuenta5.nombre;
                    conf.ItxPagarStr = consulta.cuenta6.codigo + " - " + consulta.cuenta6.nombre;                   

                    return conf;

                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se pudo obtener la configuracion");
                }
            }
        }

        public bool ActualizarConfiguracion(int idEmpresa, int configuracion)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    var consulta = (from x in esquema.confIntegracion
                                    where x.idEmpresa == idEmpresa
                                    select x).FirstOrDefault();

                    consulta.integracion = configuracion;
                    esquema.SaveChanges();

                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
