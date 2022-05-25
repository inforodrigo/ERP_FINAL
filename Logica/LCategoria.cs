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
    public class LCategoria : LBase<categoria>
    {
        public List<ECategoria> ListarCategorias(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcategoria = (from x in esquema.categoria
                                      where x.idEmpresa == idempresa
                                      && x.idCategoriaPadre == null
                                      select x).ToList();

                    List<ECategoria> categorias = new List<ECategoria>();


                    foreach (var i in linqcategoria)
                    {
                        ECategoria categoria = new ECategoria();
                        categoria.Id = i.id;
                        categoria.IdCategoria = i.id;
                        categoria.Nombre = i.nombre;
                        categoria.text = i.nombre;                        
                        categoria.Descripcion = i.descripcion;                       
                        categoria.IdUsuario = (int)i.idUsuario;
                        categoria.IdEmpresa = (int)i.idEmpresa;
                        if (i.idCategoriaPadre != null)
                        {
                            categoria.IdCategoriaPadre = (int)i.idCategoriaPadre;
                        }
                        categoria.children = CuentaHijos(categoria.Id, idempresa);
                        categorias.Add(categoria);
                    }

                    return categorias;
                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se puedo obtener la lista de categorias");
                }
            }
        }

        public List<ECategoria> CuentaHijos(long idPadre, long idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcategoria = (from x in esquema.categoria
                                      where x.idEmpresa == idempresa
                                      && x.idCategoriaPadre == idPadre
                                      select x).ToList();


                    List<ECategoria> categorias = new List<ECategoria>();

                    foreach (var i in linqcategoria)
                    {
                        ECategoria categoria = new ECategoria();
                        categoria.Id = i.id;
                        categoria.IdCategoria = i.id;                       
                        categoria.Nombre = i.nombre;
                        categoria.text = i.nombre;
                        categoria.Descripcion = i.descripcion;
                        categoria.IdUsuario = (int)i.idUsuario;
                        categoria.IdEmpresa = (int)i.idEmpresa;
                        categoria.IdCategoriaPadre = (int)i.idCategoriaPadre;
                        categoria.children = CuentaHijos(categoria.Id, idempresa);
                        categorias.Add(categoria);
                    }
                    return categorias;
                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se puedo obtener la lista de categorias");
                }
            }
        }

        public ECategoria ObtenerCategoria(long idcategoria)
        {

            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcategoria = (from x in esquema.categoria
                                      where x.id == idcategoria
                                      select x).FirstOrDefault();

                    if (linqcategoria == null)
                    {
                        throw new BussinessException("Error no se pudo obtener la categoria");
                    }

                    ECategoria e = new ECategoria();

                    e.Id = linqcategoria.id;                    
                    e.Nombre = linqcategoria.nombre;
                    e.Descripcion = linqcategoria.descripcion;
                    e.IdUsuario = (int)linqcategoria.idUsuario;
                    e.IdEmpresa = (int)linqcategoria.idEmpresa;
                    if (linqcategoria.idCategoriaPadre != null)
                    {
                        e.IdCategoriaPadre = (int)linqcategoria.idCategoriaPadre;
                    }

                    return e;

                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se puedo obtener la categoria");
                }

            }
        }

        public ECategoria Agregar(ECategoria objCategoria, int idcategoria, int padre)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var empresa = (from x in esquema.empresa
                                   where x.id == objCategoria.IdEmpresa
                                   select x).FirstOrDefault();

                    var ExisteNombre = (from x in esquema.categoria
                                        where x.idEmpresa == objCategoria.IdEmpresa &&
                                        x.nombre.ToUpper() == objCategoria.Nombre.ToUpper()
                                        select x).ToList();

                    if (ExisteNombre.Count > 0)
                    {
                        throw new BussinessException("El nombre de la categoria no esta disponible.");
                    }

                    var linqcategoria = (from x in esquema.categoria
                                      where x.idEmpresa == objCategoria.IdEmpresa
                                      select x).FirstOrDefault();


                    if (linqcategoria == null)
                    {                       
                        if (empresa != null)
                        {                                                      
                            categoria categoria = new categoria();
                            categoria.nombre = objCategoria.Nombre;
                            categoria.descripcion = objCategoria.Descripcion;
                            categoria.idUsuario = objCategoria.IdUsuario;
                            categoria.idEmpresa = objCategoria.IdEmpresa;

                            esquema.categoria.Add(categoria);
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
                                var categoriapadre = (from x in esquema.categoria
                                                   where x.idEmpresa == objCategoria.IdEmpresa
                                                   && x.id == idcategoria
                                                   select x).FirstOrDefault();

                                if (categoriapadre != null)
                                {                                  

                                    var hijos = (from x in esquema.categoria
                                                 where x.idEmpresa == objCategoria.IdEmpresa
                                                 && x.idCategoriaPadre == idcategoria
                                                 orderby x.id descending
                                                 select x).FirstOrDefault();
                                    if (hijos != null)
                                    {                                       
                                        categoria categoria = new categoria();
                                        categoria.nombre = objCategoria.Nombre;
                                        categoria.descripcion = objCategoria.Descripcion;
                                        categoria.idUsuario = objCategoria.IdUsuario;
                                        categoria.idEmpresa = objCategoria.IdEmpresa;
                                        categoria.idCategoriaPadre = objCategoria.IdCategoriaPadre;

                                        esquema.categoria.Add(categoria);
                                        esquema.SaveChanges();

                                    }
                                    else
                                    {
                                        categoria categoria = new categoria();
                                        categoria.nombre = objCategoria.Nombre;
                                        categoria.descripcion = objCategoria.Descripcion;
                                        categoria.idUsuario = objCategoria.IdUsuario;
                                        categoria.idEmpresa = objCategoria.IdEmpresa;
                                        categoria.idCategoriaPadre = objCategoria.IdCategoriaPadre;

                                        esquema.categoria.Add(categoria);
                                        esquema.SaveChanges();
                                    }
                                }
                            }
                            else if (padre == 1)
                            {
                                //agregar padres

                                var linqpadres = (from x in esquema.categoria
                                                  where x.idEmpresa == objCategoria.IdEmpresa                                                 
                                                  orderby x.id descending
                                                  select x).FirstOrDefault();

                                if (linqpadres != null)
                                {                                   

                                    categoria categoria = new categoria();
                                    categoria.nombre = objCategoria.Nombre;
                                    categoria.descripcion = objCategoria.Descripcion;
                                    categoria.idUsuario = objCategoria.IdUsuario;
                                    categoria.idEmpresa = objCategoria.IdEmpresa;

                                    esquema.categoria.Add(categoria);
                                    esquema.SaveChanges();

                                }
                            }
                        }
                    }

                    return objCategoria;
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

        public ECategoria ModificarCategoria(int idcategoria, string nombre, string descripcion, int idempresa)
        {

            using (var esquema = GetEsquema())
            {
                try
                {
                    var validar = (from x in esquema.categoria
                                   where x.idEmpresa == idempresa
                                   && x.id != idcategoria
                                   && x.nombre.Trim().ToUpper() == nombre.Trim().ToUpper()
                                   select x
                                   ).FirstOrDefault();

                    if (validar != null)
                    {
                        throw new BussinessException("El nombre de la categoria en esta empresa ya existe");
                    }

                    var linqcategoria = (from x in esquema.categoria
                                      where x.id == idcategoria
                                      select x).FirstOrDefault();

                    if (linqcategoria == null)
                    {
                        throw new BussinessException("Error no se puede obtener la categoria");
                    }

                    linqcategoria.nombre = nombre;
                    linqcategoria.descripcion = descripcion;
                    esquema.SaveChanges();

                    ECategoria e = new ECategoria();

                    e.Id = linqcategoria.id;                   
                    e.Nombre = linqcategoria.nombre;                    
                    e.Descripcion = linqcategoria.descripcion;
                    e.IdUsuario = (int)linqcategoria.idUsuario;
                    e.IdEmpresa = (int)linqcategoria.idEmpresa;
                    if (linqcategoria.idCategoriaPadre != null)
                    {
                        e.IdCategoriaPadre = (int)linqcategoria.idCategoriaPadre;
                    }

                    return e;

                }
                catch (Exception ex)
                {
                    throw new BussinessException(ex.Message);
                }

            }
        }

        public ECategoria EliminarCategoria(int idcategoria)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    var linqcategoria = (from x in esquema.categoria
                                      where x.id == idcategoria
                                      select x).FirstOrDefault();

                    if (linqcategoria == null)
                    {
                        throw new BussinessException("Error no se pudo obtener la categoria");
                    }


                    if (linqcategoria.categoria1.Count() > 0)
                    {
                        throw new BussinessException("No se puede eliminar la categoria de esta empresa porque esta relacionada");

                    }

                    esquema.categoria.Remove(linqcategoria);
                    esquema.SaveChanges();

                    ECategoria e = new ECategoria();

                    e.Id = linqcategoria.id;                   
                    e.Nombre = linqcategoria.nombre;                   
                    e.Descripcion = linqcategoria.descripcion;
                    e.IdUsuario = (int)linqcategoria.idUsuario;
                    e.IdEmpresa = (int)linqcategoria.idEmpresa;
                    if (linqcategoria.idCategoriaPadre != null)
                    {
                        e.IdCategoriaPadre = (int)linqcategoria.idCategoriaPadre;
                    }

                    return e;

                }
                catch (Exception ex)
                {
                    throw new BussinessException(ex.Message);
                }

            }
        }
    }
}
