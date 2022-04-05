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
    public class LPeriodo : LBase<gestion>
    {     

        public EPeriodo ObtenerPorid(int id)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.periodo
                                     where x.estado != (int)Estado.Eliminado
                                     where x.id == id
                                     select x).FirstOrDefault();

                    EPeriodo per = new EPeriodo();
                    per.Id = resultado.id;
                    per.Nombre = resultado.nombre;
                    per.FechaInicio = resultado.fechainicio;
                    per.FechaFin = resultado.fechafin;
                    per.IdUsuario = (int)resultado.idUsuario;
                    per.IdGestion = (int)resultado.idGestion;
                    per.Estado = resultado.estado;

                    return per;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<EPeriodo> ObtenerPorIdGestion(int id)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.periodo
                                     where x.estado != (int)Estado.Eliminado
                                     where x.idGestion == id
                                     select x).OrderBy(x => x.fechainicio).ToList();

                    List<EPeriodo> periodos = new List<EPeriodo>();
                    foreach (var item in resultado)
                    {
                        EPeriodo e = new EPeriodo();
                        e.Id = item.id;
                        e.Nombre = item.nombre;
                        e.FechaInicio = item.fechainicio;
                        e.fechaIni = item.fechainicio.ToString("dd/MM/yyyy");
                        e.FechaFin = item.fechafin;
                        e.fechaFi = item.fechafin.ToString("dd/MM/yyyy");
                        e.IdUsuario = item.usuario.id;
                        e.IdGestion = item.gestion.id;
                        e.Usuario = item.usuario.nombre;
                        e.Gestion = item.gestion.nombre;
                        e.Estado = item.estado;
                        periodos.Add(e);
                    }

                    return periodos;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EPeriodo Agregar(EPeriodo objperiodo)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var ExisteNombre = (from x in esquema.periodo
                                        where x.estado != (int)Estado.Eliminado &&
                                        x.idGestion == objperiodo.IdGestion && 
                                        x.nombre.ToUpper() == objperiodo.Nombre.ToUpper()
                                        select x).ToList();

                    if (ExisteNombre.Count > 0)
                    {
                        throw new BussinessException("El nombre del periodo ya esta en uso.");
                    }

                    var RangoGestion = (from x in esquema.gestion
                                        where x.id == objperiodo.IdGestion
                                        select x).FirstOrDefault();

                    if(objperiodo.FechaInicio < RangoGestion.fechainicio || objperiodo.FechaFin > RangoGestion.fechafin)
                    {
                        throw new BussinessException("El rango de fecha del periodo esta fuera del rango de la gestion.");
                    }

                    var ExistePeriodo = (from x in esquema.periodo
                                         where x.idGestion == objperiodo.IdGestion &&
                                         x.estado == (int)Estado.Habilitado &&                                        
                                         (x.fechainicio <= objperiodo.FechaFin) && (objperiodo.FechaInicio <= x.fechafin)                               
                                         select x).ToList();
                    //||(x.fechainicio == objperiodo.FechaInicio) && (x.fechafin == objperiodo.FechaFin)
                    if (ExistePeriodo.Count > 0)
                    {
                        throw new BussinessException("Error existe solapamiento de fecha con otro periodo.");
                    }

                    periodo periodo = new periodo();
                    periodo.nombre = objperiodo.Nombre;
                    periodo.fechainicio = (DateTime)objperiodo.FechaInicio;
                    periodo.fechafin = (DateTime)objperiodo.FechaFin;
                    periodo.idUsuario = objperiodo.IdUsuario;
                    periodo.idGestion = objperiodo.IdGestion;
                    periodo.estado = 0;
                    esquema.periodo.Add(periodo);
                    esquema.SaveChanges();
                  
                    return objperiodo;
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

        public EPeriodo Editar(EPeriodo objperiodo)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var ExisteNombre = (from x in esquema.periodo
                                        where x.estado != (int)Estado.Eliminado &&
                                        x.idGestion == objperiodo.IdGestion &&
                                        x.id != objperiodo.Id &&
                                        x.nombre.ToUpper() == objperiodo.Nombre.ToUpper()
                                        select x).ToList();

                    if (ExisteNombre.Count > 0)
                    {
                        throw new BussinessException("El nombre del periodo ya esta en uso.");
                    }

                    var RangoGestion = (from x in esquema.gestion
                                        where x.id == objperiodo.IdGestion
                                        select x).FirstOrDefault();

                    if (objperiodo.FechaInicio < RangoGestion.fechainicio || objperiodo.FechaFin > RangoGestion.fechafin)
                    {
                        throw new BussinessException("El rango de fecha del periodo esta fuera del rango de la gestion.");
                    }

                    var ExistePeriodo = (from x in esquema.periodo
                                         where x.idGestion == objperiodo.IdGestion &&
                                         x.estado == (int)Estado.Habilitado &&
                                         x.id != objperiodo.Id &&                                        
                                         (x.fechainicio <= objperiodo.FechaFin) && (objperiodo.FechaInicio <= x.fechafin)
                                         select x).ToList();
                    
                    if (ExistePeriodo.Count > 0)
                    {
                        throw new BussinessException("Error existe solapamiento de fecha con otro periodo.");
                    }

                    var periodo = (from x in esquema.periodo
                                   where x.estado != (int)Estado.Eliminado
                                   && x.id == objperiodo.Id
                                   select x).FirstOrDefault();

                    periodo.nombre = objperiodo.Nombre;
                    periodo.fechainicio = (DateTime)objperiodo.FechaInicio;
                    periodo.fechafin = (DateTime)objperiodo.FechaFin;                  
                    esquema.SaveChanges();

                    return objperiodo;
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
            try
            {
                using (var esquema = GetEsquema())
                {
                    periodo periodo = esquema.periodo.Where(x => x.id == id).FirstOrDefault();
                    if (periodo == null)
                    {
                        return false;
                    }
                    esquema.periodo.Remove(periodo);
                    esquema.SaveChanges();

                    return true;
                }
            }
            catch (BussinessException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
