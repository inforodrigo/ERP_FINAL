using Data;
using Entidad;
using Entidad.Enums;
using Entidad.EReportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LGestion : LBase<gestion>
    {

        public List<EGestion> Obtener()
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.gestion
                                     where x.estado != (int)Estado.Eliminado
                                     select x).ToList();

                    List<EGestion> gestiones = new List<EGestion>();
                    foreach (var item in resultado)
                    {
                        EGestion e = new EGestion();
                        e.Id = item.id;
                        e.Nombre = item.nombre;
                        e.FechaInicio = item.fechainicio;
                        e.FechaFin = item.fechafin;
                        e.IdUsuario = item.usuario.id;
                        e.IdEmpresa = item.empresa.id;
                        e.Usuario = item.usuario.nombre;
                        e.Empresa = item.empresa.nombre;
                        e.Estado = item.estado;
                        gestiones.Add(e);
                    }

                    return gestiones;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<EGestion> ObtenerPorIdEmpresa(int id)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.gestion
                                     where x.estado != (int)Estado.Eliminado
                                     where x.idEmpresa == id
                                     select x).OrderBy(x => x.fechainicio).ToList();

                    List<EGestion> gestiones = new List<EGestion>();
                    foreach (var item in resultado)
                    {
                        EGestion e = new EGestion();
                        e.Id = item.id;
                        e.Nombre = item.nombre;
                        e.FechaInicio = item.fechainicio;
                        e.fechaIni = item.fechainicio.ToString("dd/MM/yyyy");
                        e.FechaFin = item.fechafin;
                        e.fechaFi = item.fechafin.ToString("dd/MM/yyyy");
                        e.IdUsuario = item.usuario.id;
                        e.IdEmpresa = item.empresa.id;
                        e.Usuario = item.usuario.nombre;
                        e.Empresa = item.empresa.nombre;
                        e.Estado = item.estado;
                        gestiones.Add(e);
                    }

                    return gestiones;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EGestion ObtenerPorid(int id)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.gestion
                                     where x.estado != (int)Estado.Eliminado
                                     where x.id == id
                                     select x).FirstOrDefault();

                    EGestion ges = new EGestion();
                    ges.Id = resultado.id;
                    ges.Nombre = resultado.nombre;
                    ges.FechaInicio = resultado.fechainicio;
                    ges.FechaFin = resultado.fechafin;
                    ges.IdUsuario = (int)resultado.idUsuario;
                    ges.IdEmpresa = (int)resultado.idEmpresa;
                    ges.Estado = resultado.estado;

                    return ges;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public gestion ObtenerGestionPorid(int id)
        {
            using (var esquema = GetEsquema())
            {
                var resultado = (from x in esquema.gestion
                                    where x.estado != (int)Estado.Eliminado
                                    where x.id == id
                                    select x).FirstOrDefault();

    

                return resultado;
            }
        }

        public bool CambiarEstado(int id, int estado)
        {
            using (var esquema = GetEsquema())
            {
                var resultado = (from x in esquema.gestion
                                 where x.estado != (int)Estado.Eliminado
                                 where x.id == id
                                 select x).FirstOrDefault();
                try
                {
                    resultado.estado = estado;
                    esquema.SaveChanges();
                    return true;
                }
                catch (BussinessException ex)
                {
                    return false;
                }       
            }
        }

        public EGestion Agregar(EGestion objgestion)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var gestionActiva = (from x in esquema.gestion
                                         where
                                         x.idEmpresa == objgestion.IdEmpresa &&
                                         x.estado != (int)Estado.Desabilitado
                                         select x).ToList();

                    if (gestionActiva.Count == 2)
                    {
                        throw new BussinessException("Ya existen dos gestiones abiertas.");
                    }

                    var ExisteNombre = (from x in esquema.gestion
                                        where x.estado == (int)Estado.Habilitado &&
                                        x.idEmpresa == objgestion.IdEmpresa &&
                                        x.nombre.ToUpper() == objgestion.Nombre.ToUpper()
                                        select x).ToList();

                    if (ExisteNombre.Count > 0)
                    {
                        throw new BussinessException("El nombre de la gestion no esta disponible.");
                    }

                    var ExisteGestion = (from x in esquema.gestion
                                         where x.idEmpresa == objgestion.IdEmpresa &&
                                         x.estado != (int)Estado.Desabilitado &&                                       
                                         (x.fechainicio <= objgestion.FechaFin) && (objgestion.FechaInicio <= x.fechafin) 
                                         select x).ToList();
                    //||(x.fechainicio == objgestion.FechaInicio) && (x.fechafin == objgestion.FechaFin)
                    if (ExisteGestion.Count > 0)
                    {
                        throw new BussinessException("Existe solapamiento de fecha con otra gestion.");
                    }                    

                    gestion gestion = new gestion();
                    gestion.nombre = objgestion.Nombre;
                    gestion.fechainicio = (DateTime)objgestion.FechaInicio;
                    gestion.fechafin = (DateTime)objgestion.FechaFin;
                    gestion.idUsuario = objgestion.IdUsuario;
                    gestion.idEmpresa = objgestion.IdEmpresa;
                    gestion.estado = 0;
                    esquema.gestion.Add(gestion);
                    esquema.SaveChanges();

                    return objgestion;
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

        public EGestion Editar(EGestion objgestion)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var ExisteNombre = (from x in esquema.gestion
                                        where x.estado == (int)Estado.Habilitado &&
                                        x.idEmpresa == objgestion.IdEmpresa &&
                                        x.id != objgestion.Id &&
                                        x.nombre.ToUpper() == objgestion.Nombre.ToUpper()
                                        select x).ToList();

                    if (ExisteNombre.Count > 0)
                    {
                        throw new BussinessException("El nombre de la gestion no esta disponible.");
                    }

                    if(objgestion.FechaFin <= objgestion.FechaInicio)
                    {
                        throw new BussinessException("Error la fecha de fin es menor a la de inicio.");
                    }                 

                    var ExisteGestion = (from x in esquema.gestion
                                         where x.idEmpresa == objgestion.IdEmpresa &&
                                         x.estado != (int)Estado.Desabilitado &&                                        
                                         x.id != objgestion.Id &&
                                         (x.fechainicio <= objgestion.FechaFin) && (objgestion.FechaInicio <= x.fechafin)
                                         select x).ToList();
                    //||(x.fechainicio == objgestion.FechaInicio) && (x.fechafin == objgestion.FechaFin)
                    if (ExisteGestion.Count > 0)
                    {
                        throw new BussinessException("Error existe solapamiento de fecha con otra gestion.");
                    }

                    var gestion = (from x in esquema.gestion
                                     where x.estado != (int)Estado.Eliminado
                                     where x.id == objgestion.Id
                                     select x).FirstOrDefault();
                    
                    gestion.nombre = objgestion.Nombre;
                    gestion.fechainicio = (DateTime)objgestion.FechaInicio;
                    gestion.fechafin = (DateTime)objgestion.FechaFin;         
                    esquema.SaveChanges();

                    return objgestion;
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
                    gestion gestion = esquema.gestion.Where(x => x.id == id).FirstOrDefault();
                    if (gestion == null)
                    {
                        return false;
                    }
                    esquema.gestion.Remove(gestion);
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

        public List<ERGestion> ReporteGestion(int idempresa)
        {
            List<ERGestion> gestiones = new List<ERGestion>();

            try
            {
                using (var esquema = GetEsquema())
                {
                    var gestion = (from x in esquema.gestion
                                   where x.idEmpresa == idempresa
                                   select x).ToList();



                    foreach (var ge in gestion)
                    {
                        ERGestion g = new ERGestion();
                        g.NombreGestion = ge.nombre;
                        g.FechaInicio = ge.fechainicio.ToString("dd/MM/yyyy");
                        g.FechaFin = ge.fechafin.ToString("dd/MM/yyyy");
                        if (ge.estado == (int)Estado.Habilitado)
                        {
                            g.Estado = "Abierta";
                        }
                        else if (ge.estado == (int)Estado.Desabilitado)
                        {
                            g.Estado = "Cerrado";
                        }
                        else
                        {
                            g.Estado = "";
                        }

                        gestiones.Add(g);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BussinessException("Error no se puede obtener el reporte de gestiones");
            }

            return gestiones;
        }

        public List<ERDatosBasicoGestion> ReporteDatosBasicoGestion(string usuario, string empresa)
        {
            try
            {
                List<ERDatosBasicoGestion> basicos = new List<ERDatosBasicoGestion>();
                ERDatosBasicoGestion eRDatosBasico = new ERDatosBasicoGestion();
                eRDatosBasico.Usuario = usuario;
                eRDatosBasico.NombreEmpresa = empresa;
                eRDatosBasico.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");

                basicos.Add(eRDatosBasico);

                return basicos;
            }
            catch (Exception ex)
            {
                throw new BussinessException("Ha ocurrido un error.");
            }
        }
    }
}
