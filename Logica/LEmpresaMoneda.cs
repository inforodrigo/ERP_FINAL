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
    public class LEmpresaMoneda : LBase<empresaMoneda>
    {
        public List<EEmpresaMoneda> ObtenerPoridEmpresa(int id)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.empresaMoneda
                                     where x.idEmpresa == id
                                     select x).ToList();

                    List<EEmpresaMoneda> empresaMonedas = new List<EEmpresaMoneda>();
                    foreach(var item in resultado)
                    {
                        EEmpresaMoneda em = new EEmpresaMoneda();
                        if (item.cambio != null)
                            em.Cambio = (double)item.cambio;
                        em.Activo = (int)item.activo;
                        em.FechaRegistro = item.fechaRegistro;
                        em.Fecha = item.fechaRegistro.ToString("dd/MM/yyyy");
                        em.idEmpresa = item.idEmpresa;
                        em.idMonedaPrincipal = item.idMonedaPrincipal;
                        em.MonedaPrincipal = item.moneda.abreviatura;
                        if(item.idMonedaAlternativa != null && item.moneda1.abreviatura !=null){
                            em.idMonedaAlternativa = (int)item.idMonedaAlternativa;
                            em.MonedaAlternativa = item.moneda1.abreviatura;
                        }                                                    
                        em.idUsuario = item.idUsuario;
                        empresaMonedas.Add(em);
                    }                 

                    return empresaMonedas;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EMoneda ObtenerMonedaPrincipal(int id)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.empresaMoneda
                                     where x.idEmpresa == id
                                     select x).FirstOrDefault();

                    var remoneda = (from x in esquema.moneda
                                    where x.id == resultado.idMonedaPrincipal
                                    select x).FirstOrDefault();


                    EMoneda monedaPrin = new EMoneda();
                    monedaPrin.Id = remoneda.id;
                    monedaPrin.Nombre = remoneda.nombre;

                    return monedaPrin;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<EMoneda> ObtenerMonedasAlternativas(int usu, int mon)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    List<EMoneda> monedas = new List<EMoneda>();

                    var resultado = (from x in esquema.moneda
                                     where x.idUsuario == usu &&
                                     x.id != mon
                                     select x).ToList();

                    foreach (var item in resultado)
                    {
                        EMoneda mone = new EMoneda();
                        mone.Id = item.id;
                        mone.Nombre = item.nombre;
                        monedas.Add(mone);
                    }

                    return monedas;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EEmpresaMoneda Agregar(EEmpresaMoneda objEmpresaMoneda)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var monedaActiva = (from x in esquema.empresaMoneda
                                         where x.idEmpresa == objEmpresaMoneda.idEmpresa &&
                                         x.activo == 1
                                         select x).FirstOrDefault();
                    
                    if(monedaActiva.cambio != null)
                    {
                        if(objEmpresaMoneda.Cambio == (double)monedaActiva.cambio)
                        {
                            throw new BussinessException("No se puede registrar el mismo tipo de cambio dos veces seguidas.");
                        }    
                    }

                    CambiarEstado(objEmpresaMoneda.idEmpresa);

                    empresaMoneda moneda = new empresaMoneda();
                    moneda.cambio = objEmpresaMoneda.Cambio;
                    moneda.activo = 1;
                    moneda.fechaRegistro = DateTime.Now;
                    moneda.idEmpresa = objEmpresaMoneda.idEmpresa;
                    moneda.idMonedaPrincipal = objEmpresaMoneda.idMonedaPrincipal;
                    moneda.idMonedaAlternativa = objEmpresaMoneda.idMonedaAlternativa;
                    moneda.idUsuario = objEmpresaMoneda.idUsuario;

                    esquema.empresaMoneda.Add(moneda);
                    esquema.SaveChanges();

                    return objEmpresaMoneda;
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

        public bool CambiarEstado(int id)
        {
            using (var esquema = GetEsquema())
            {
                var resultado = (from x in esquema.empresaMoneda
                                 where x.idEmpresa == id &&
                                 x.activo == 1
                                 select x).FirstOrDefault();
                try
                {
                    resultado.activo = 0;
                    esquema.SaveChanges();
                    return true;
                }
                catch (BussinessException ex)
                {
                    return false;
                }
            }
        }
    }
}
