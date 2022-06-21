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
    public class LArticulo : LBase<articulo>
    {
        public List<EArticulo> ObtenerPorIdEmpresa(int id)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.articulo
                                     where x.idEmpresa == id                                   
                                     select x).ToList();

                    List<EArticulo> articulos = new List<EArticulo>();
                    foreach (var item in resultado)
                    {
                        EArticulo e = new EArticulo();
                        e.Id = item.id;
                        e.Nombre = item.nombre;
                        e.Descripcion = item.descripcion;
                        if(item.cantidad != null)
                            e.Cantidad = (int)item.cantidad;
                        e.PrecioVenta = item.precioVenta;
                        e.IdEmpresa = item.empresa.id;
                        e.IdUsuario = item.usuario.id;                      
                        articulos.Add(e);
                    }

                    return articulos;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ECategoria> ObtenerCategoriasEditar(int idempresa, int idarticulo)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var categorias = (from x in esquema.categoria
                                     where x.idEmpresa == idempresa
                                     select x).ToList();

                    var catseleccionadas = (from x in esquema.articuloCategoria
                                     where x.idArticulo == idarticulo                                    
                                     select x).ToList();

                    List<ECategoria> catego = new List<ECategoria>();

                    foreach (var cat in categorias)
                    {
                        foreach (var item in catseleccionadas)
                        {
                            if (cat.id != item.idCategoria)
                            {
                                ECategoria cate = new ECategoria();
                                cate.Id = cat.id;
                                cate.Nombre = cat.nombre;
                                if (cat.idCategoriaPadre != null)
                                {
                                    cate.IdCategoriaPadre = (int)cat.idCategoriaPadre;
                                }
                                catego.Add(cate);

                                foreach (var ca in catego)
                                {
                                    if(cat.id != ca.Id)
                                    {
                                        ECategoria categ = new ECategoria();
                                        categ.Id = cat.id;
                                        categ.Nombre = cat.nombre;
                                        if (cat.idCategoriaPadre != null)
                                        {
                                            categ.IdCategoriaPadre = (int)cat.idCategoriaPadre;
                                        }
                                        catego.Add(categ);
                                    }
                                }
                            }
                            
                        }
                    }
                    

                    return catego;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EArticulo ObtenerPorId(int idempresa, int idarticulo)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.articulo
                                     where x.idEmpresa == idempresa
                                     && x.id == idarticulo
                                     select x).FirstOrDefault();

                    EArticulo articulos = new EArticulo();
                    articulos.Id = resultado.id;
                    articulos.Nombre = resultado.nombre;
                    articulos.Descripcion = resultado.descripcion;
                    if (resultado.cantidad != null)
                        articulos.Cantidad = (int)resultado.cantidad;
                    articulos.PrecioVenta = resultado.precioVenta;
                    articulos.IdEmpresa = resultado.empresa.id;
                    articulos.IdUsuario = resultado.usuario.id;          

                    return articulos;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<int>  ObtenerCategoriasPorIdArticulo(int idempresa, int idarticulo)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.articuloCategoria
                                     where x.idArticulo == idarticulo                                   
                                     select x).ToList();
                    //List<EArticuloCategoria> cat = new List<EArticuloCategoria>();
                    List<int> categorias = new List<int>();

                    foreach (var a in resultado)
                    {
                        int id = a.idCategoria;
                        categorias.Add(id);
                        /*EArticuloCategoria art = new EArticuloCategoria();
                        art.IdArticulo = a.idArticulo;
                        art.Articulo = a.articulo.nombre;
                        art.IdCategoria = a.idCategoria;
                        art.Categoria = a.categoria.nombre;
                        cat.Add(art);*/
                    }                  

                    return categorias;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EArticulo Agregar(EArticulo objarticulo, List<int> categorias)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    
                    articulo articulo = new articulo();
                    articulo.nombre = objarticulo.Nombre;
                    articulo.descripcion = objarticulo.Descripcion;
                    articulo.cantidad = 0;
                    articulo.precioVenta = objarticulo.PrecioVenta;
                    articulo.idEmpresa = objarticulo.IdEmpresa;
                    articulo.idUsuario = objarticulo.IdUsuario;
                    esquema.articulo.Add(articulo);
                    esquema.SaveChanges();

                    foreach (var item in categorias)
                    {
                        articuloCategoria cat = new articuloCategoria();
                        cat.idArticulo = articulo.id;
                        cat.idCategoria = item;
                        esquema.articuloCategoria.Add(cat);
                    };                                                       
                    
                    esquema.SaveChanges();                  

                    return objarticulo;
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

        public EArticulo EditarArticulo(EArticulo objarticulo)
        {
            try
            {
                using (var esquema = GetEsquema())
                {                                  

                    var articulo = (from x in esquema.articulo                                 
                                   where x.id == objarticulo.Id
                                   && x.idEmpresa == objarticulo.IdEmpresa
                                   select x).FirstOrDefault();

                    articulo.nombre = objarticulo.Nombre;
                    articulo.descripcion = objarticulo.Descripcion;
                    articulo.precioVenta = objarticulo.PrecioVenta;
                    esquema.SaveChanges();

                    return objarticulo;
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

        public List<ELote> ObtenerLotesXArticulo(int idempresa, int id)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.lote
                                     where x.idArticulo == id
                                     select x).ToList();

                    List<ELote> lotes = new List<ELote>();
                    foreach (var item in resultado)
                    {
                        ELote lo = new ELote();
                        lo.IdArticulo = item.idArticulo;
                        lo.NombreArticulo = item.articulo.nombre;
                        lo.NroLote = item.nroLote;
                        lo.FechaIngreso = item.fechaIngreso;
                        lo.FechaIngresoStr = item.fechaIngreso.ToString("dd/MM/yyyy");
                        if(item.fechaVencimiento != null){
                            lo.FechaVencimiento = (DateTime)item.fechaVencimiento;
                            lo.FechaVencimientoStr = lo.FechaVencimiento.ToString("dd/MM/yyyy");
                        } else {
                            lo.FechaVencimientoStr = "-";
                        }                            
                        lo.Cantidad = item.cantidad;
                        lo.PrecioCompra = item.precioCompra;
                        lo.Stock = (int)item.stock;
                        lo.IdNota = item.idNota;
                        lo.Estado = item.estado;
                        if(item.estado == 0){
                            lo.EstadoStr = "Agotado";
                        }else if(item.estado == 1){
                            lo.EstadoStr = "Activo";
                        }else if (item.estado == 2){
                            lo.EstadoStr = "Anulado";
                        }
                        lotes.Add(lo);

                    }

                    return lotes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool EliminarArticulo(int id)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    var notas = (from x in esquema.detalleNota
                                 where x.idArticulo == id
                                 select x).ToList();

                    if(notas.Count > 0)
                    {
                        throw new BussinessException("No se puede eliminar este articulo porque ya se han realizado notas.");
                    }
                    var categorias = (from x in esquema.articuloCategoria
                                      where x.idArticulo == id
                                      select x).ToList();

                    foreach(var cat in categorias)
                    {                       
                        esquema.articuloCategoria.Remove(cat);
                        esquema.SaveChanges();
                    }

                    var articulo = (from x in esquema.articulo
                                 where x.id == id
                                 select x).FirstOrDefault();

                    esquema.articulo.Remove(articulo);
                    esquema.SaveChanges();

                    return true;
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

        public List<ERDatosBasicoStockArticulo> ReporteDatosBasicoStockArticulo(string usuario, string empresa)
        {
            try
            {
                List<ERDatosBasicoStockArticulo> basicos = new List<ERDatosBasicoStockArticulo>();
                ERDatosBasicoStockArticulo eRDatosBasico = new ERDatosBasicoStockArticulo();
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

        public List<ERStockArticulos> ReporteStockArticulo(int idcategoria, int cantidad, int idempresa)
        {
            List<ERStockArticulos> stocks = new List<ERStockArticulos>();
            try
            {
                using (var esquema = GetEsquema())
                {
                    var categorias = listarCategorias(idempresa, idcategoria);
                    var categoriashijos = CategoriaHijos(idcategoria, idempresa);

                    //Articulos de la categoria seleccionada
                    foreach (var cat in categorias)
                    {
                        var catar = (from x in esquema.articuloCategoria
                                     where x.idCategoria == cat.Id
                                     select x).ToList();

                        foreach (var ar in catar)
                        {
                            var articulos = (from x in esquema.articulo
                                         where x.id == ar.idArticulo
                                         && x.idEmpresa == idempresa
                                         && x.cantidad <= cantidad
                                         select x).FirstOrDefault();

                            if(articulos != null)
                            {
                                ERStockArticulos stock = new ERStockArticulos();
                                stock.IdCategoria = ar.idCategoria;
                                stock.IdArticulo = ar.idArticulo;
                                stock.Categoria = ar.categoria.nombre;
                                stock.Articulo = ar.articulo.nombre;
                                stock.Stock = (int)articulos.cantidad;
                                stocks.Add(stock);
                            }                          
                        }
                    }

                    //Articulos de los hijos de la categoria seleccionada
                    foreach (var cat in categoriashijos)
                    {
                        var catar = (from x in esquema.articuloCategoria
                                     where x.idCategoria == cat.Id
                                     select x).ToList();

                        foreach (var ar in catar)
                        {
                            var articulos = (from x in esquema.articulo
                                             where x.id == ar.idArticulo
                                             && x.idEmpresa == idempresa
                                             && x.cantidad <= cantidad
                                             select x).FirstOrDefault();

                            if (articulos != null)
                            {
                                ERStockArticulos stock = new ERStockArticulos();
                                stock.IdCategoria = ar.idCategoria;
                                stock.IdArticulo = ar.idArticulo;
                                stock.Categoria = ar.categoria.nombre;
                                stock.Articulo = ar.articulo.nombre;
                                stock.Stock = (int)articulos.cantidad;
                                stocks.Add(stock);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BussinessException("Error no se puede obtener el reporte de nota de compra");
            }

            return stocks;
        }

        public List<ECategoria> listarCategorias(int idempresa, int idcategoria)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcategoria = (from x in esquema.categoria
                                      where x.idEmpresa == idempresa 
                                      && x.id == idcategoria
                                      select x).ToList();

                    List<ECategoria> categorias = new List<ECategoria>();


                    foreach (var i in linqcategoria)
                    {
                        ECategoria cat = new ECategoria();
                        cat.Id = i.id;
                        cat.Nombre = i.nombre;                    
                        cat.IdUsuario = (int)i.idUsuario;
                        cat.IdEmpresa = (int)i.idEmpresa;
                        if (i.idCategoriaPadre != null)
                        {
                            cat.IdCategoriaPadre = (int)i.idCategoriaPadre;
                        }
                        cat.children = CategoriaHijos(cat.Id, idempresa);
                        categorias.Add(cat);
                    }

                    return categorias;
                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se puedo obtener la lista de categorias");
                }
            }
        }

        public List<ECategoria> CategoriaHijos(long idPadre, long idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcategoria = (from x in esquema.categoria
                                      where x.idEmpresa == idempresa
                                      && x.idCategoriaPadre == idPadre
                                      select x).ToList();


                    List<ECategoria> cuentas = new List<ECategoria>();

                    foreach (var i in linqcategoria)
                    {
                        ECategoria cat = new ECategoria();
                        cat.Id = i.id;
                        cat.IdCategoria = i.id;
                        cat.Nombre = i.nombre;                       
                        cat.IdUsuario = (int)i.idUsuario;
                        cat.IdEmpresa = (int)i.idEmpresa;
                        cat.IdCategoriaPadre = (int)i.idCategoriaPadre;
                        cat.children = CategoriaHijos(cat.Id, idempresa);
                        cuentas.Add(cat);
                    }
                    return cuentas;
                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se puedo obtener la lista de categorias");
                }
            }
        }

    }
}
