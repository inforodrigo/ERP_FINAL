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
    public class LReporte : LBase<comprobante>
    {
        public List<ERDatosBasicoSumasSaldos> ReporteDatosBasicos(string usuario, string empresa, string gestion, string moneda)
        {
            try
            {
                List<ERDatosBasicoSumasSaldos> basicos = new List<ERDatosBasicoSumasSaldos>();
                ERDatosBasicoSumasSaldos eRDatosBasico = new ERDatosBasicoSumasSaldos();
                eRDatosBasico.Usuario = usuario;
                eRDatosBasico.NombreEmpresa = empresa;
                eRDatosBasico.Gestion = gestion;
                eRDatosBasico.Moneda = moneda;
                eRDatosBasico.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");

                basicos.Add(eRDatosBasico);

                return basicos;
            }
            catch (Exception ex)
            {
                throw new BussinessException("Ha ocurrido un error.");
            }
        }

        public List<ERSumasSaldos> ReporteDeSumasySaldos(int idGestion, int idMoneda)
        {
            List<ERSumasSaldos> reporte = new List<ERSumasSaldos>();
            try
            {
                using (var esquema = GetEsquema())
                {
                    

                    var gestion = (from x in esquema.gestion
                                    where x.id == idGestion
                                    select x).FirstOrDefault();

                    var moneda = (from x in esquema.empresaMoneda
                                  where x.idEmpresa == gestion.idEmpresa
                                  && x.activo == 1
                                  select x).FirstOrDefault();

                    var cuentas = (from x in esquema.cuenta
                                   where x.idEmpresa == gestion.idEmpresa
                                   && x.tipocuenta == (int)ETipoCuentas.Detallado
                                   orderby x.codigo ascending
                                   select x).ToList();

                    foreach(var cu in cuentas)
                    {
                        ERSumasSaldos sumasSaldos = new ERSumasSaldos();
                        sumasSaldos.Cuenta = cu.codigo + " " + cu.nombre;                       
                        sumasSaldos.SumasDebe = 0;
                        sumasSaldos.SumasHaber = 0;
                        sumasSaldos.SaldosDebe = 0;
                        sumasSaldos.SaldosHaber = 0;

                        var detallecomprobantes = (from x in esquema.detalleComprobante
                                                   where x.idCuenta == cu.id
                                                   && x.comprobante.idEmpresa == gestion.idEmpresa
                                                   && x.comprobante.fecha >= gestion.fechainicio
                                                   && x.comprobante.fecha <= gestion.fechafin
                                                   && x.comprobante.estado != (int)EstadosComprobante.Anulado
                                                   select x).ToList();

                        foreach(var de in detallecomprobantes)
                        {
                            if(moneda.idMonedaPrincipal == idMoneda)
                            {
                                sumasSaldos.SumasDebe = Math.Round((sumasSaldos.SumasDebe + de.montoDebe), 2);
                                sumasSaldos.SumasHaber = Math.Round((sumasSaldos.SumasHaber + de.montoHaber), 2);
                            }
                            else
                            {
                                sumasSaldos.SumasDebe = Math.Round((sumasSaldos.SumasDebe + de.montoDebeAlt), 2);
                                sumasSaldos.SumasHaber = Math.Round((sumasSaldos.SumasHaber + de.montoHaberAlt), 2);
                            }
                        }

                        if(sumasSaldos.SumasDebe > 0 || sumasSaldos.SumasHaber > 0)
                        {
                            double saldoTotal = Math.Round((sumasSaldos.SumasDebe - sumasSaldos.SumasHaber), 2);
                            if(saldoTotal > 0)
                            {
                                sumasSaldos.SaldosDebe = saldoTotal;
                            }
                            else
                            {
                                sumasSaldos.SaldosHaber = saldoTotal;
                            }
                            reporte.Add(sumasSaldos);
                        }
                    }                                   
                }
                return reporte;
            }
            catch (Exception ex)
            {
                throw new BussinessException("Error no se puede obtener el reporte de Comprobacion de Sumas y Saldos");
            }
        }

        public List<ERDatosBasicoLibroDiario> ReporteDatosBasicoLibroDiario(string usuario, string empresa, string gestion, string periodo, string moneda)
        {
            try
            {
                List<ERDatosBasicoLibroDiario> basicos = new List<ERDatosBasicoLibroDiario>();
                ERDatosBasicoLibroDiario eRDatosBasico = new ERDatosBasicoLibroDiario();
                eRDatosBasico.Usuario = usuario;
                eRDatosBasico.NombreEmpresa = empresa;
                eRDatosBasico.Gestion = gestion;
                eRDatosBasico.Periodo = periodo;
                eRDatosBasico.Moneda = moneda;
                eRDatosBasico.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");

                basicos.Add(eRDatosBasico);

                return basicos;
            }
            catch (Exception ex)
            {
                throw new BussinessException("Ha ocurrido un error.");
            }
        }

        public List<ERLibroDiario> ReporteDeLibroDiario(int idGestion,int idPeriodo, int idMoneda)
        {
            List<ERLibroDiario> reporte = new List<ERLibroDiario>();
            try
            {
                using (var esquema = GetEsquema())
                {


                    var gestion = (from x in esquema.gestion
                                   where x.id == idGestion
                                   select x).FirstOrDefault();

                    var moneda = (from x in esquema.empresaMoneda
                                  where x.idEmpresa == gestion.idEmpresa
                                  && x.activo == 1
                                  select x).FirstOrDefault();

                    if(idPeriodo != 0)
                    {
                        var periodo = (from x in esquema.periodo
                                       where x.idGestion == gestion.id
                                       && x.id == idPeriodo
                                       select x).FirstOrDefault();

                        var comprobantes = (from x in esquema.comprobante
                                            where x.idEmpresa == gestion.idEmpresa
                                            && x.fecha >= periodo.fechainicio
                                            && x.fecha <= periodo.fechafin
                                            && x.estado != (int)EstadosComprobante.Anulado
                                            select x).ToList();

                        foreach(var com in comprobantes)
                        {
                            ERLibroDiario libroDiario = new ERLibroDiario();
                            libroDiario.Fecha = com.fecha.ToString("dd/MM/yyyy");
                            libroDiario.Cuenta = com.glosa;
                            reporte.Add(libroDiario);

                            var detallecomprobantes = (from x in esquema.detalleComprobante
                                                       where x.idComprobante == com.id                                                  
                                                       select x).ToList();

                            foreach(var de in detallecomprobantes)
                            {
                                ERLibroDiario libroDiarios = new ERLibroDiario();
                                if (de.montoDebe > 0)
                                {
                                    libroDiarios.Cuenta = de.cuenta.codigo + " - " + de.cuenta.nombre;
                                }
                                else
                                {
                                    libroDiarios.Cuenta = "   " + de.cuenta.codigo + " - " + de.cuenta.nombre;
                                }
                                
                                if (moneda.idMonedaPrincipal == idMoneda)
                                {
                                    libroDiarios.Debe = de.montoDebe;
                                    libroDiarios.Haber = de.montoHaber;
                                }
                                else
                                {
                                    libroDiarios.Debe = Math.Round((de.montoDebeAlt),2);
                                    libroDiarios.Haber = Math.Round((de.montoHaberAlt),2);
                                }
                                reporte.Add(libroDiarios);
                            }
                        }
                    }
                    else
                    {
                        var periodo = (from x in esquema.periodo
                                       where x.idGestion == gestion.id                                       
                                       select x).ToList();

                        foreach(var pe in periodo)
                        {
                            var compro = (from x in esquema.comprobante
                                                where x.idEmpresa == gestion.idEmpresa
                                                && x.fecha >= pe.fechainicio
                                                && x.fecha <= pe.fechafin
                                                && x.estado != (int)EstadosComprobante.Anulado
                                                select x).ToList();

                            foreach (var com in compro)
                            {
                                ERLibroDiario libroDiario = new ERLibroDiario();
                                libroDiario.Fecha = com.fecha.ToString("dd/MM/yyyy");
                                libroDiario.Cuenta = com.glosa;
                                reporte.Add(libroDiario);

                                var detallecomprobantes = (from x in esquema.detalleComprobante
                                                           where x.idComprobante == com.id
                                                           && x.comprobante.idEmpresa == gestion.idEmpresa
                                                           select x).ToList();

                                foreach (var de in detallecomprobantes)
                                {
                                    var cuenta = (from x in esquema.cuenta
                                                  where x.idEmpresa == gestion.idEmpresa
                                                  && x.id == de.idCuenta
                                                  select x).FirstOrDefault();
                                    ERLibroDiario libroDiarios = new ERLibroDiario();
                                    if (de.montoDebe > 0)
                                    {
                                        libroDiarios.Cuenta = de.cuenta.codigo + " - " + de.cuenta.nombre;
                                    }
                                    else
                                    {
                                        libroDiarios.Cuenta = "   " + de.cuenta.codigo + " - " + de.cuenta.nombre;
                                    }
                                    if (moneda.idMonedaPrincipal == idMoneda)
                                    {
                                        libroDiarios.Debe = de.montoDebe;
                                        libroDiarios.Haber = de.montoHaber;
                                    }
                                    else
                                    {
                                        libroDiarios.Debe = Math.Round((de.montoDebeAlt), 2);
                                        libroDiarios.Haber = Math.Round((de.montoHaberAlt), 2);
                                    }
                                    reporte.Add(libroDiarios);
                                }
                            }
                        }                      
                    }                                     
                }
                return reporte;
            }
            catch (Exception ex)
            {
                throw new BussinessException("Error no se puede obtener el reporte de Libro Diario");
            }
        }

        public List<ERDatosBasicoLibroMayor> ReporteDatosBasicoLibroMayor(string usuario, string empresa, string gestion, string periodo, string moneda)
        {
            try
            {
                List<ERDatosBasicoLibroMayor> basicos = new List<ERDatosBasicoLibroMayor>();
                ERDatosBasicoLibroMayor eRDatosBasico = new ERDatosBasicoLibroMayor();
                eRDatosBasico.Usuario = usuario;
                eRDatosBasico.NombreEmpresa = empresa;
                eRDatosBasico.Gestion = gestion;
                eRDatosBasico.Periodo = periodo;
                eRDatosBasico.Moneda = moneda;
                eRDatosBasico.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");

                basicos.Add(eRDatosBasico);

                return basicos;
            }
            catch (Exception ex)
            {
                throw new BussinessException("Ha ocurrido un error.");
            }
        }

        public List<ERLibroMayor> ReporteDeLibroMayor(int idGestion, int idPeriodo, int idMoneda)
        {
            List<ERLibroMayor> reporte = new List<ERLibroMayor>();
            try
            {
                using (var esquema = GetEsquema())
                {


                    var gestion = (from x in esquema.gestion
                                   where x.id == idGestion
                                   select x).FirstOrDefault();

                    var moneda = (from x in esquema.empresaMoneda
                                  where x.idEmpresa == gestion.idEmpresa
                                  && x.activo == 1
                                  select x).FirstOrDefault();

                    if (idPeriodo != 0)
                    {
                        var periodo = (from x in esquema.periodo
                                       where x.idGestion == gestion.id
                                       && x.id == idPeriodo
                                       select x).FirstOrDefault();

                        var cuentas = (from x in esquema.cuenta
                                      where x.idEmpresa == gestion.idEmpresa
                                      && x.tipocuenta == 2
                                      select x).ToList();                       

                        foreach (var cu in cuentas)
                        {                           
                            var detallecomprobantes = (from x in esquema.detalleComprobante
                                                       where x.idCuenta == cu.id
                                                       && x.comprobante.fecha >= periodo.fechainicio
                                                       && x.comprobante.fecha <= periodo.fechafin
                                                       && x.comprobante.estado != (int)EstadosComprobante.Anulado
                                                       select x).ToList();
                            var contador = 0;
                            double saldo = 0;

                            foreach (var de in detallecomprobantes)
                            {
                                ERLibroMayor rLibroMayor = new ERLibroMayor();
                                rLibroMayor.IdCuenta = cu.id;
                                rLibroMayor.Cuenta = cu.codigo + " - " + cu.nombre;
                                rLibroMayor.Fecha = de.comprobante.fecha.ToString("dd/MM/yyyy");
                                rLibroMayor.NroComprobante = de.comprobante.serie;
                                rLibroMayor.Tipo = ObtenerTipoComprobante(de.comprobante.tipoComprobante);
                                rLibroMayor.Glosa = de.comprobante.glosa;

                                if (moneda.idMonedaPrincipal == idMoneda)
                                {
                                    rLibroMayor.Debe = de.montoDebe;
                                    rLibroMayor.Haber = de.montoHaber;
                                    if(contador == 0)
                                    {
                                        if(de.montoDebe > 0)
                                        {
                                            rLibroMayor.Saldo = de.montoDebe;
                                            saldo = de.montoDebe;
                                        }
                                        else
                                        {
                                            rLibroMayor.Saldo = de.montoHaber;
                                            saldo = de.montoHaber;
                                        }                                      
                                    }
                                    else
                                    {
                                        if (de.montoDebe > 0)
                                        {                                           
                                            saldo = saldo + de.montoDebe;
                                            rLibroMayor.Saldo = saldo;
                                        }
                                        else
                                        {                                           
                                            saldo = saldo - de.montoHaber;
                                            rLibroMayor.Saldo = saldo;
                                        }
                                    }                                   
                                }
                                else
                                {
                                    rLibroMayor.Debe = Math.Round((de.montoDebeAlt), 2);
                                    rLibroMayor.Haber = Math.Round((de.montoHaberAlt), 2);
                                    if (contador == 0)
                                    {
                                        if (de.montoDebeAlt > 0)
                                        {
                                            rLibroMayor.Saldo = Math.Round((de.montoDebeAlt), 2);
                                            saldo = Math.Round((de.montoDebeAlt), 2);
                                        }
                                        else
                                        {
                                            rLibroMayor.Saldo = Math.Round((de.montoHaberAlt), 2);
                                            saldo = Math.Round((de.montoHaberAlt), 2);
                                        }
                                    }
                                    else
                                    {
                                        if (de.montoDebeAlt > 0)
                                        {
                                            saldo = saldo + de.montoDebeAlt;
                                            rLibroMayor.Saldo = Math.Round((saldo), 2);
                                        }
                                        else
                                        {
                                            saldo = saldo - de.montoHaberAlt;
                                            rLibroMayor.Saldo = Math.Round((saldo), 2);
                                        }
                                    }
                                }
                                reporte.Add(rLibroMayor);
                                contador++;
                            }
                        }
                    }
                    else
                    {                       
                        var ges = (from x in esquema.gestion
                                       where x.id == gestion.id
                                       select x).FirstOrDefault();
                       
                        var cuentas = (from x in esquema.cuenta
                                           where x.idEmpresa == gestion.idEmpresa
                                           && x.tipocuenta == 2
                                           select x).ToList();                           

                        foreach (var cu in cuentas)
                        {                               
                            var detallecomprobantes = (from x in esquema.detalleComprobante
                                                        where x.idCuenta == cu.id
                                                        && x.comprobante.fecha >= ges.fechainicio
                                                        && x.comprobante.fecha <= ges.fechafin
                                                        && x.comprobante.estado != (int)EstadosComprobante.Anulado
                                                        select x).ToList();                              
                            double saldo = 0;

                            foreach (var de in detallecomprobantes)
                            {
                                ERLibroMayor rLibroMayor = new ERLibroMayor();
                                rLibroMayor.IdCuenta = cu.id;
                                rLibroMayor.Cuenta = cu.codigo + " - " + cu.nombre;
                                rLibroMayor.Fecha = de.comprobante.fecha.ToString("dd/MM/yyyy");
                                rLibroMayor.NroComprobante = de.comprobante.serie;
                                rLibroMayor.Tipo = ObtenerTipoComprobante(de.comprobante.tipoComprobante);
                                rLibroMayor.Glosa = de.comprobante.glosa;

                                if (moneda.idMonedaPrincipal == idMoneda)
                                {
                                    rLibroMayor.Debe = de.montoDebe;
                                    rLibroMayor.Haber = de.montoHaber;                                      
                                    if (rLibroMayor.Debe > 0)
                                    {
                                        saldo = saldo + rLibroMayor.Debe;
                                        if(saldo >= 0)
                                        {
                                            rLibroMayor.Saldo = saldo;
                                        }
                                        else
                                        {
                                            rLibroMayor.Saldo = saldo * -1;
                                        }                                           
                                    }
                                    else
                                    {
                                        saldo = saldo - rLibroMayor.Haber;
                                        if(saldo >= 0)
                                        {
                                            rLibroMayor.Saldo = saldo;
                                        }
                                        else
                                        {
                                            rLibroMayor.Saldo = saldo * -1;
                                        }                                           
                                    }                                      
                                }
                                else
                                {
                                    rLibroMayor.Debe = Math.Round((de.montoDebeAlt), 2);
                                    rLibroMayor.Haber = Math.Round((de.montoHaberAlt), 2);                                       
                                    if (rLibroMayor.Debe > 0)
                                    {
                                        saldo = saldo + rLibroMayor.Debe;
                                        if(saldo >= 0)
                                        {
                                            rLibroMayor.Saldo = saldo;
                                        }
                                        else
                                        {
                                            rLibroMayor.Saldo = saldo * -1;
                                        }
                                    }
                                    else
                                    {
                                        saldo = saldo - rLibroMayor.Haber;

                                        if (saldo >= 0)
                                        {
                                            rLibroMayor.Saldo = saldo;
                                        }
                                        else
                                        {
                                            rLibroMayor.Saldo = saldo * -1;
                                        }
                                    }                                                                                                               
                                }
                                reporte.Add(rLibroMayor);                                   
                            }
                        }                      
                    }
                }
                return reporte;
            }
            catch (Exception ex)
            {
                throw new BussinessException("Error no se puede obtener el reporte de Libro Mayor");
            }
        }

        public string ObtenerTipoComprobante(int tipo)
        {
            var TipoCo = "";
            if(tipo == 1)
            {
                TipoCo = "Ingreso";
            }
            else if(tipo == 2)
            {
                TipoCo = "Egreso";
            }
            else if (tipo == 3)
            {
                TipoCo = "Traspaso";
            }
            else if (tipo == 4)
            {
                TipoCo = "Apertura";
            }
            else if (tipo == 5)
            {
                TipoCo = "Ajuste";
            }
            return TipoCo;
        }
    }
}
