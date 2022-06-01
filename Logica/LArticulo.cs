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
    }
}
