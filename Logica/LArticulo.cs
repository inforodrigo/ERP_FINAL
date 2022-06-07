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
    }
}
