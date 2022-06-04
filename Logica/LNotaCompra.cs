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
    public class LNotaCompra : LBase<nota>
    {
        public List<ENota> listarNotasDeCompras(int idempresa, int idusuario)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var consulta = (from x in esquema.nota
                                    where x.idEmpresa == idempresa
                                    && x.idUsuario == idusuario
                                    && x.tipo == (int)ETipoNota.Compra
                                    select x).OrderByDescending(x => x.nroNota).ToList();

                    List<ENota> notas = new List<ENota>();


                    foreach (var item in consulta)
                    {
                        ENota nota = new ENota();
                        nota.Id = item.id;
                        nota.NroNota = item.nroNota;
                        nota.Fecha = item.fecha;
                        nota.FechaStr = item.fecha.ToString("dd/MM/yyyy");
                        nota.Descripcion = item.descripcion;
                        nota.Total = item.tipo;
                        nota.Tipo = item.tipo;                        
                        nota.IdEmpresa = item.idEmpresa;
                        nota.IdUsuario = item.idUsuario;
                        nota.Estado = item.estado;

                        notas.Add(nota);
                    }

                    return notas;
                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se puedo obtener la lista de notas de compras");
                }
            }
        }

        public ENota ObtenerNotaCompraPorId(int id, int idEmpresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var consulta = (from x in esquema.nota
                                    where x.id == id
                                    && x.tipo == (int)ETipoNota.Compra
                                    && x.idEmpresa == idEmpresa
                                    select x).FirstOrDefault();

                    ENota nota = new ENota();
                    nota.Id = consulta.id;
                    nota.NroNota = consulta.nroNota;                   
                    nota.Fecha = consulta.fecha;
                    nota.FechaStr = consulta.fecha.ToString("dd/MM/yyyy");
                    nota.Descripcion = consulta.descripcion;
                    nota.Total = consulta.total;
                    nota.Tipo = consulta.tipo;
                    nota.Estado = consulta.estado;
                    if (nota.Estado == 0)
                    {
                        nota.EstadoStr = "Cerrado";
                    }
                    else if (nota.Estado == 1)
                    {
                        nota.EstadoStr = "Abierto";
                    }
                    else if (nota.Estado == 2)
                    {
                        nota.EstadoStr = "Anulado";
                    }
                    nota.IdEmpresa = consulta.idEmpresa;
                    nota.IdUsuario = consulta.idUsuario;
                    

                    return nota;
                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se pudo obtener los datos de la Nota de Compra");
                }
            }
        }

        public List<EDetalleNotaAux> ObtenerDetallePorIdNota(int idnota)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    var consulta = (from x in esquema.detalleNota
                                    where x.idNota == idnota
                                    select x).ToList();

                    List<EDetalleNotaAux> detalles = new List<EDetalleNotaAux>();


                    foreach (var item in consulta)
                    {
                        EDetalleNotaAux detalle = new EDetalleNotaAux();
                        detalle.idArticulo = item.idArticulo;
                        detalle.articulo = item.articulo.nombre;
                        detalle.cantidad = item.cantidad;
                        detalle.precio = item.precioVenta;
                        detalle.subtotal = item.cantidad * item.precioVenta;                       
                        
                        detalles.Add(detalle);
                    }

                    return detalles;
                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se puedo obtener el detalle de la Nota de Compra");
                }
            }
        }

        public int ObtenerNroNota(int idEmpresa, int idUsuario)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    var consulta = (from x in esquema.nota
                                    where x.idEmpresa == idEmpresa
                                    && x.idUsuario == idUsuario
                                    && x.tipo == (int)ETipoNota.Compra
                                    select x).OrderByDescending(x => x.id).FirstOrDefault();

                    var nro = 0;

                    if (consulta != null)
                    {
                        nro = consulta.nroNota + 1;
                    }
                    else
                    {
                        nro = 1;
                    }

                    return nro;

                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se pudo obtener el numero de nota de compra");
                }
            }
        }

        public List<EArticulo> ObtenerArticulos(int idempresa, int idusuario)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var consulta = (from x in esquema.articulo
                                    where x.idEmpresa == idempresa
                                    && x.idUsuario == idusuario                                    
                                    select x).ToList();

                    List<EArticulo> articulos = new List<EArticulo>();


                    foreach (var item in consulta)
                    {
                        EArticulo articulo = new EArticulo();
                        articulo.Id = item.id;                       
                        articulo.Nombre = item.nombre;
                        articulo.Descripcion = item.descripcion;
                        articulo.Cantidad = (int)item.cantidad;
                        articulo.PrecioVenta = item.precioVenta;
                        articulo.IdEmpresa = idempresa;
                        articulo.IdUsuario = idusuario;

                        articulos.Add(articulo);
                    }

                    return articulos;
                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se puedo obtener la lista de articulos");
                }
            }
        }

        public ENota AgregarNotaCompra(ENota nota, List<EDetalleNotaAux> detalle)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    var consultarnro = (from x in esquema.nota
                                          where x.idEmpresa == nota.IdEmpresa
                                          && x.nroNota == nota.NroNota
                                          && x.tipo == (int)ETipoNota.Compra
                                          select x).OrderByDescending(x => x.nroNota).FirstOrDefault();

                    if (consultarnro != null)
                    {
                        nota.NroNota = consultarnro.nroNota + 1;
                    }

                    var verificargestion = (from x in esquema.gestion
                                            where x.idEmpresa == nota.IdEmpresa
                                            && (nota.Fecha >= x.fechainicio) && (nota.Fecha <= x.fechafin)
                                            select x).ToList();

                    if (verificargestion.Count == 0)
                    {
                        throw new BussinessException("Error la fecha no pertenece a ninguna gestion activa.");
                    }

                    var obtenergestion = (from x in esquema.gestion
                                          where x.idEmpresa == nota.IdEmpresa
                                          && (x.estado == (int)Estado.Habilitado || x.estado == (int)Estado.Asignado)
                                          select x).OrderBy(x => x.fechainicio).ToList();

                    //Verificar Si esta dentro de un periodo
                    var countperiodo = 0;
                    foreach (var itemg in obtenergestion)
                    {
                        var verificarperiodo = (from x in esquema.periodo
                                                where x.idGestion == itemg.id
                                                && (nota.Fecha >= x.fechainicio) && (nota.Fecha <= x.fechafin)
                                                select x).ToList();

                        countperiodo = countperiodo + verificarperiodo.Count;
                    }

                    if (countperiodo == 0)
                    {
                        throw new BussinessException("Error la fecha no pertenece a ningun periodo activo.");
                    }

                    nota notac = new nota();
                    notac.nroNota = nota.NroNota;
                    notac.fecha = nota.Fecha;
                    notac.descripcion = nota.Descripcion;
                    notac.total = nota.Total;
                    notac.tipo = 1;
                    notac.idEmpresa = nota.IdEmpresa;
                    notac.idUsuario = nota.IdUsuario;
                    notac.estado = nota.Estado;
                    esquema.nota.Add(notac);
                    esquema.SaveChanges();

                    foreach (var det in detalle)
                    {
                        var consultarnrolote = (from x in esquema.lote
                                                where x.idArticulo == det.idArticulo
                                                select x).OrderByDescending(x => x.nroLote).FirstOrDefault();

                        var nroLote = 1;

                        if (consultarnrolote != null)
                        {
                            nroLote = consultarnrolote.nroLote + 1;
                        }

                        lote lote = new lote();
                        lote.idArticulo = det.idArticulo;
                        lote.nroLote = nroLote;
                        lote.fechaIngreso = nota.Fecha;
                        if (det.fecha != null)
                            lote.fechaVencimiento = Convert.ToDateTime(det.fecha);
                        lote.cantidad = det.cantidad;
                        lote.precioCompra = det.precio;
                        lote.stock = det.cantidad;
                        lote.idNota = notac.id;
                        lote.estado = 1;
                        esquema.lote.Add(lote);

                        detalleNota detalles = new detalleNota();
                        detalles.idArticulo = det.idArticulo;
                        detalles.nroLote = nroLote;
                        detalles.idNota = notac.id;
                        detalles.cantidad = det.cantidad;
                        detalles.precioVenta = det.precio;
                        esquema.detalleNota.Add(detalles);
                        esquema.SaveChanges();

                        var consultaarticulo = (from x in esquema.articulo
                                                where x.id == det.idArticulo
                                                && x.idEmpresa == nota.IdEmpresa
                                                select x).FirstOrDefault();

                        consultaarticulo.cantidad = consultaarticulo.cantidad + det.cantidad;
                        esquema.SaveChanges();
                    }

                    ENota respuesta = new ENota();
                    respuesta.Id = notac.id;

                    return respuesta;

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

        public bool AnularNotaCompra(int id)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    //validar que los lotes no tengan ventas
                    var lotes = (from x in esquema.lote
                            where x.idNota == id                            
                            select x).ToList();

                    if(lotes != null)
                    {
                        foreach(var lot in lotes)
                        {
                            var detalles = (from x in esquema.detalleNota
                                            where x.nroLote == lot.nroLote                                          
                                            select x).ToList();

                            foreach(var de in detalles)
                            {
                                if(de.nota.tipo == (int)ETipoNota.Venta)
                                {
                                    throw new BussinessException("No se puede anular esta nota de compra porque ya se han realizado ventas con los lotes.");
                                }
                            }                           
                        }
                    }

                    //anulamos los lotes y restamos la cantidad a los articulos
                    foreach(var lot in lotes)
                    {
                        lot.estado = 2;                      

                        var articulos = (from x in esquema.articulo
                                         where x.id == lot.idArticulo
                                         select x).FirstOrDefault();

                        articulos.cantidad = articulos.cantidad - lot.cantidad;
                        esquema.SaveChanges();
                    }               

                    //Finalmente anulamos la nota
                    var nota = (from x in esquema.nota
                                       where x.id == id
                                       && x.tipo == (int)ETipoNota.Compra
                                       select x).FirstOrDefault();

               
                    nota.estado = 2;
                    esquema.SaveChanges();
                    return true;
                }
                catch (BussinessException ex)
                {
                    return false;
                }
            }
        }

        public List<ERDatosBasicoNotaCompra> ReporteDatosBasicoNotaCompra(string usuario, string empresa)
        {
            try
            {
                List<ERDatosBasicoNotaCompra> basicos = new List<ERDatosBasicoNotaCompra>();
                ERDatosBasicoNotaCompra eRDatosBasico = new ERDatosBasicoNotaCompra();
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

        public List<ERNotaCompra> ReporteNotaCompra(int idempresa, int idnota)
        {
            List<ERNotaCompra> nota = new List<ERNotaCompra>();
            try
            {
                using (var esquema = GetEsquema())
                {
                    var not = (from x in esquema.nota
                                   where x.id == idnota
                                   && x.idEmpresa == idempresa
                                   select x).ToList();

                    foreach (var no in not)
                    {

                        ERNotaCompra nc = new ERNotaCompra();
                        nc.Nro = no.nroNota;
                        if (no.estado == 0){
                            nc.Estado = "Cerrado";
                        }else if (no.estado == 1){
                            nc.Estado = "Abierto";
                        }else if(no.estado == 2){
                            nc.Estado = "Anulado";
                        }
                        nc.Fecha = no.fecha.ToString("dd/MM/yyyy");
                        nc.Proveedor = "";
                        nc.Descripcion = no.descripcion;                       
                        nc.Total = Convert.ToString(no.total);                        

                        nota.Add(nc);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BussinessException("Error no se puede obtener el reporte de nota de compra");
            }

            return nota;
        }

        public List<ERDetalleNotaCompra> ReporteDetalleNotaCompra(int idnota)
        {
            List<ERDetalleNotaCompra> detalles = new List<ERDetalleNotaCompra>();
            try
            {
                using (var esquema = GetEsquema())
                {
                    var detallec = (from x in esquema.detalleNota
                                    where x.idNota == idnota
                                    select x).ToList();

                    foreach (var de in detallec)
                    {

                        ERDetalleNotaCompra dc = new ERDetalleNotaCompra();
                        dc.Articulo = de.articulo.nombre;
                        dc.Cantidad = de.cantidad;
                        dc.Precio = de.precioVenta.ToString();
                        dc.SubTotal = Convert.ToString(de.cantidad * de.precioVenta);

                        detalles.Add(dc);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BussinessException("Error no se puede obtener el reporte de detalle de la nota de compra");
            }

            return detalles;
        }
    }
}
