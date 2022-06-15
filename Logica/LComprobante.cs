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
    public class LComprobante : LBase<comprobante>
    {
        public List<EComprobante> listarComprobantes(int idempresa, int idusuario)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var consulta = (from x in esquema.comprobante
                                      where x.idEmpresa == idempresa       
                                      && x.idUsuario == idusuario
                                      select x).OrderByDescending(x => x.id).ToList();

                    List<EComprobante> comprobantes = new List<EComprobante>();


                    foreach (var item in consulta)
                    {
                        EComprobante comprobante = new EComprobante();
                        comprobante.Id = item.id;
                        comprobante.Serie = item.serie;
                        comprobante.Glosa = item.glosa;
                        comprobante.Fecha = item.fecha;
                        comprobante.Fechas = item.fecha.ToString("dd/MM/yyyy");
                        comprobante.TipoCambio = item.tipoCambio;
                        comprobante.Estado = item.estado;
                        comprobante.TipoComprobante = item.tipoComprobante;
                        comprobante.IdEmpresa = item.idEmpresa;
                        comprobante.IdUsuario = item.idUsuario;
                        comprobante.IdMoneda = item.idMoneda;
                        comprobante.Moneda = item.moneda.nombre;
                       
                        comprobantes.Add(comprobante);
                    }

                    return comprobantes;
                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se puedo obtener la lista de comprobantes");
                }
            }
        }

        public EComprobante ObtenerComprobantePorId(int id, int idEmpresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var consulta = (from x in esquema.comprobante
                                    where x.id == id
                                    && x.idEmpresa == idEmpresa
                                    select x).FirstOrDefault();
                   
                        EComprobante comprobante = new EComprobante();
                        comprobante.Id = consulta.id;
                        comprobante.Serie = consulta.serie;
                        comprobante.Glosa = consulta.glosa;
                        comprobante.Fecha = consulta.fecha;
                        comprobante.Fechas = consulta.fecha.ToString("dd/MM/yyyy");
                        comprobante.TipoCambio = consulta.tipoCambio;
                        comprobante.Estado = consulta.estado;
                        if(comprobante.Estado == 0){
                            comprobante.EstadoStr = "Cerrado";
                        }else if(comprobante.Estado == 1){
                            comprobante.EstadoStr = "Abierto";
                        }else if (comprobante.Estado == 2){
                            comprobante.EstadoStr = "Anulado";
                        }
                    comprobante.TipoComprobante = consulta.tipoComprobante;
                        if(consulta.tipoComprobante == 1){
                            comprobante.TipoComprobanteStr = "Ingreso";
                        }else if(consulta.tipoComprobante == 2){
                            comprobante.TipoComprobanteStr = "Egreso";
                        }else if (consulta.tipoComprobante == 3){
                            comprobante.TipoComprobanteStr = "Traspaso";
                        }else if (consulta.tipoComprobante == 4){
                            comprobante.TipoComprobanteStr = "Apertura";
                        }else if (consulta.tipoComprobante == 5){
                            comprobante.TipoComprobanteStr = "Ajuste";
                        }
                        comprobante.IdEmpresa = consulta.idEmpresa;
                        comprobante.IdUsuario = consulta.idUsuario;
                        comprobante.IdMoneda = consulta.idMoneda;
                        comprobante.Moneda = consulta.moneda.nombre;                                        

                    return comprobante;
                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se pudo obtener los datos del comprobante");
                }
            }
        }

        public List<EDetalleComprobante> ObtenerDetallePorIdComprobante(int idComprobante)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    var consulta = (from x in esquema.detalleComprobante
                                    where x.idComprobante == idComprobante
                                    select x).ToList();

                    List<EDetalleComprobante> detalle = new List<EDetalleComprobante>();


                    foreach (var item in consulta)
                    {
                        EDetalleComprobante comprobante = new EDetalleComprobante();
                        comprobante.Id = item.id;
                        comprobante.Numero = item.numero;
                        comprobante.Glosa = item.glosa;
                        comprobante.montoDebe = item.montoDebe;
                        comprobante.montoDebeAlt = item.montoDebeAlt;
                        comprobante.montoHaber = item.montoHaber;
                        comprobante.montoHaberAlt = item.montoHaberAlt;
                        comprobante.idCuenta = item.idCuenta;
                        comprobante.Cuenta = item.cuenta.codigo + " - " + item.cuenta.nombre;
                        detalle.Add(comprobante);
                    }

                    return detalle;
                }
                catch (Exception ex)
                {
                    throw new BussinessException("Error no se puedo obtener la lista de comprobantes");
                }
            }
        }

        public List<EMoneda> ObtenerMonedasComprobante(int idEmpresa, int idUsuario)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var monedaPrincipal = (from x in esquema.moneda join em in esquema.empresaMoneda on
                                     x.id equals em.idMonedaPrincipal
                                     where x.idUsuario == idUsuario && em.idEmpresa == idEmpresa && em.idUsuario == idUsuario
                                     && em.idMonedaAlternativa == null
                                     select x).ToList();

                    var monedaActiva = (from x in esquema.moneda
                                           join em in esquema.empresaMoneda on x.id equals em.idMonedaAlternativa
                                           where x.idUsuario == idUsuario && em.idEmpresa == idEmpresa && em.idUsuario == idUsuario
                                           && em.activo == 1
                                           select x).ToList();                    

                    List<EMoneda> monedas = new List<EMoneda>();

                    foreach (var item in monedaPrincipal)
                    {
                        EMoneda monedaPrin = new EMoneda();
                        monedaPrin.Id = item.id;
                        monedaPrin.Nombre = item.nombre;
                        monedaPrin.Abreviatura = item.abreviatura;
                        monedas.Add(monedaPrin);
                    }

                    foreach (var item in monedaActiva)
                    {
                        EMoneda monedaPrin = new EMoneda();
                        monedaPrin.Id = item.id;
                        monedaPrin.Nombre = item.nombre;
                        monedaPrin.Abreviatura = item.abreviatura;
                        monedas.Add(monedaPrin);
                    }

                    return monedas;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int ObtenerSerie(int idEmpresa, int idUsuario)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.comprobante                                    
                                     where x.idUsuario == idUsuario && x.idEmpresa == idEmpresa
                                     select x).OrderByDescending(x => x.id).FirstOrDefault();
                    var serie = 0;

                    if(resultado != null)
                    {
                        serie = resultado.serie + 1;
                    }
                    else
                    {
                        serie = 1;
                    }                  

                    return serie;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string ObtenerCambio(int idEmpresa, int idUsuario)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.empresaMoneda
                                     where x.idUsuario == idUsuario && x.idEmpresa == idEmpresa
                                     && x.activo == 1
                                     select x).FirstOrDefault();
                    var cambio = "";

                    if (resultado != null)
                    {
                        cambio = resultado.cambio.ToString();
                    }
                    else
                    {
                        cambio = "";
                    }

                    return cambio;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ECuenta> ObtenerCuentas(int idempresa, int idusuario)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var consulta = (from x in esquema.cuenta
                                    where x.estado == (int)Estado.Habilitado
                                    && x.idEmpresa == idempresa
                                    && x.idUsuario == idusuario
                                    && x.tipocuenta == 2
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

        public EComprobante AgregarComprobante(EComprobante comprobante, List<EDetalleComprobante> detalle)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    var consultarserie = (from x in esquema.comprobante
                                          where x.idEmpresa == comprobante.IdEmpresa
                                          && x.serie == comprobante.Serie
                                          select x).OrderByDescending(x => x.serie).FirstOrDefault();

                    if (consultarserie != null)
                    {
                        comprobante.Serie = consultarserie.serie + 1;
                    }
                    else
                    {
                        comprobante.Serie = 1;
                    }

                    var verificargestion = (from x in esquema.gestion
                                       where x.idEmpresa == comprobante.IdEmpresa
                                       && (comprobante.Fecha >= x.fechainicio) && (comprobante.Fecha <= x.fechafin)
                                       select x).ToList();

                    if(verificargestion.Count == 0)
                    {
                        throw new BussinessException("Error la fecha no pertenece a ninguna gestion activa.");
                    }

                    var obtenergestion = (from x in esquema.gestion
                                   where x.idEmpresa == comprobante.IdEmpresa
                                   && (x.estado == (int)Estado.Habilitado || x.estado == (int)Estado.Asignado)
                                   select x).OrderBy(x => x.fechainicio).ToList();

                    //Verificar Si esta dentro de un periodo
                    var countperiodo = 0;
                    foreach(var itemg in obtenergestion)
                    {
                        var verificarperiodo = (from x in esquema.periodo
                                                where x.idGestion == itemg.id
                                                && (comprobante.Fecha >= x.fechainicio) && (comprobante.Fecha <= x.fechafin)
                                                select x).ToList();
                       
                        countperiodo = countperiodo + verificarperiodo.Count;                                             
                    }

                    if(countperiodo == 0)
                    {
                        throw new BussinessException("Error la fecha no pertenece a ningun periodo activo.");
                    }

                    //Verificar si ya existe un comprobante de apertura
                    if(comprobante.TipoComprobante == (int)ETipoComprobante.Apertura)
                    {                        
                        var linqgestion = (from x in esquema.gestion
                                            where x.idEmpresa == comprobante.IdEmpresa
                                            && (comprobante.Fecha >= x.fechainicio) && (comprobante.Fecha <= x.fechafin)
                                            select x).First();

                        var linqcompro = (from x in esquema.comprobante
                                          where x.idEmpresa == comprobante.IdEmpresa
                                          && (x.fecha >= linqgestion.fechainicio) && (x.fecha <= linqgestion.fechafin)
                                          && x.tipoComprobante == (int)ETipoComprobante.Apertura
                                          && x.estado == (int)EstadosComprobante.Abierto
                                          select x).ToList();

                        if(linqcompro.Count > 0)
                        {
                            throw new BussinessException("Error ya existe un comprobante de apertura activo para esta gestion.");
                        }
                    }                
                    
                    comprobante compro = new comprobante();
                    compro.serie = comprobante.Serie;
                    compro.glosa = comprobante.Glosa;
                    compro.fecha = comprobante.Fecha;
                    compro.tipoCambio = comprobante.TipoCambio;
                    compro.estado = comprobante.Estado;
                    compro.tipoComprobante = comprobante.TipoComprobante;
                    compro.idEmpresa = comprobante.IdEmpresa;
                    compro.idUsuario = comprobante.IdUsuario;
                    compro.idMoneda = comprobante.IdMoneda;
                    esquema.comprobante.Add(compro);
                    esquema.SaveChanges();

                    var idcomprobante = (from x in esquema.comprobante
                                         where x.idEmpresa == comprobante.IdEmpresa
                                         && x.serie == comprobante.Serie
                                         select x).FirstOrDefault();

                    //Verificar si la moneda del comprobante es principal o alternativa para poder sacar la conversion
                    bool esmonedaprincipal = true;

                    var tipomonedap = (from x in esquema.empresaMoneda
                                       where x.idEmpresa == comprobante.IdEmpresa
                                       && x.idMonedaPrincipal == comprobante.IdMoneda
                                       && x.idMonedaAlternativa == null
                                       select x).FirstOrDefault();

                    //si trae datos entonces es moneda principal sino es moneda alternativa
                    if (tipomonedap != null)
                    {
                        esmonedaprincipal = true;
                    }
                    else
                    {
                        esmonedaprincipal = false;                      
                    }

                    

                    foreach (var det in detalle)
                    {
                        var numero = (from x in esquema.detalleComprobante
                                      where x.idUsuario == det.idUsuario
                                      select x).OrderByDescending(x => x.id).FirstOrDefault();

                        var num = 0;
                        if(numero != null)
                        {
                            num = numero.numero + 1;
                        }
                        else
                        {
                            num = 1;
                        }
                        detalleComprobante d = new detalleComprobante();
                        d.numero = num;
                        d.glosa = det.Glosa;
                        if (esmonedaprincipal)
                        {
                            if(comprobante.TipoCambio == 0)
                            {
                                d.montoDebe = det.montoDebe;
                                d.montoHaber = det.montoHaber;
                                d.montoDebeAlt = 0;
                                d.montoHaberAlt = 0;
                            }
                            else
                            {
                                d.montoDebe = det.montoDebe;
                                d.montoHaber = det.montoHaber;
                                d.montoDebeAlt = det.montoDebe / comprobante.TipoCambio;
                                d.montoHaberAlt = det.montoHaber / comprobante.TipoCambio;
                            }
                        }
                        else
                        {
                            d.montoDebe = det.montoDebe * comprobante.TipoCambio;
                            d.montoHaber = det.montoHaber * comprobante.TipoCambio;
                            d.montoDebeAlt = det.montoDebe;
                            d.montoHaberAlt = det.montoHaber;
                        }                                                       
                        d.idUsuario = det.idUsuario;
                        d.idCuenta = det.idCuenta;
                        d.idComprobante = idcomprobante.id;
                        esquema.detalleComprobante.Add(d);
                        esquema.SaveChanges();
                    }
                    EComprobante data = new EComprobante();
                    data.Id = idcomprobante.id;

                    return data;
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

        public bool AnularComprobante(int id)
        {          
            using (var esquema = GetEsquema())
            {
                var notas = (from x in esquema.nota
                                   where x.idComprobante == id
                                   select x).FirstOrDefault();

                if(notas != null)
                {
                    throw new BussinessException("No se puede anular este comprobante por que esta relacionado con una Nota de Compra o Venta.");
                }

                var comprobante = (from x in esquema.comprobante
                                where x.id == id
                                select x).FirstOrDefault();

                try
                {
                    comprobante.estado = 2;
                    esquema.SaveChanges();
                    return true;
                }
                catch (BussinessException ex)
                {
                    return false;
                }
            }                       
        }

        public List<ERComprobanteContable> ReporteComprobanteContable(int idempresa, int idcomprobante)
        {
            List<ERComprobanteContable> comprobante = new List<ERComprobanteContable>();
            try
            {
                using (var esquema = GetEsquema())
                {
                    var comprob = (from x in esquema.comprobante
                                   where x.id == idcomprobante
                                   && x.idEmpresa == idempresa
                                   select x).ToList();

                    foreach (var compro in comprob)
                    {

                        ERComprobanteContable cc = new ERComprobanteContable();
                        cc.Serie = compro.serie;
                        if (compro.estado == 0){
                            cc.Estado = "Cerrado";
                        }else if (compro.estado == 1){
                            cc.Estado = "Abierto";
                        }else if (compro.estado == 2)
                        {
                            cc.Estado = "Anulado";
                        }
                        cc.Fecha = compro.fecha.ToString("dd/MM/yyyy");
                        cc.Moneda = compro.moneda.nombre + " - " + compro.moneda.abreviatura;
                        cc.TipoCambio = compro.tipoCambio.ToString();
                        if (compro.tipoComprobante == 1){
                            cc.TipoComprobante = "Ingreso";
                        }else if (compro.tipoComprobante == 2){
                            cc.TipoComprobante = "Egreso";
                        }else if (compro.tipoComprobante == 3){
                            cc.TipoComprobante = "Traspaso";
                        }else if (compro.tipoComprobante == 4){
                            cc.TipoComprobante = "Apertura";
                        }else if (compro.tipoComprobante == 5){
                            cc.TipoComprobante = "Ajuste";
                        }
                        cc.Glosa = compro.glosa;

                        var detallec = (from x in esquema.detalleComprobante
                                        where x.idComprobante == idcomprobante
                                        select x).ToList();

                        double totaldebe = 0;
                        double totalhaber = 0;
                        foreach (var de in detallec)
                        {
                            totaldebe = totaldebe + de.montoDebe;
                            totalhaber = totalhaber + de.montoHaber;
                        }

                        cc.TotalDebe = totaldebe.ToString();
                        cc.TotalHaber = totalhaber.ToString();

                        comprobante.Add(cc);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BussinessException("Error no se puede obtener el reporte de comprobante");
            }

            return comprobante;
        }

        public List<ERDetalleComprobanteContable> ReporteDetalleComprobante(int idcomprobante)
        {
            List<ERDetalleComprobanteContable> detalles = new List<ERDetalleComprobanteContable>();
            try
            {
                using (var esquema = GetEsquema())
                {
                    var detallec = (from x in esquema.detalleComprobante
                                   where x.idComprobante == idcomprobante
                                   select x).ToList();

                    foreach (var de in detallec)
                    {

                        ERDetalleComprobanteContable dc = new ERDetalleComprobanteContable();
                        dc.Cuenta = de.cuenta.codigo + " - " + de.cuenta.nombre;
                        dc.Glosa = de.glosa;
                        dc.Debe = de.montoDebe.ToString();
                        dc.Haber = de.montoHaber.ToString();                     

                        detalles.Add(dc);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BussinessException("Error no se puede obtener el reporte de detalle de comprobante");
            }

            return detalles;
        }

        public List<ERDatosBasicoComprobante> ReporteDatosBasicoComprobante(string usuario, string empresa)
        {
            try
            {
                List<ERDatosBasicoComprobante> basicos = new List<ERDatosBasicoComprobante>();
                ERDatosBasicoComprobante eRDatosBasico = new ERDatosBasicoComprobante();
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
