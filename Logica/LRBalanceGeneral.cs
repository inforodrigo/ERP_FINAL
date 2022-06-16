﻿using Data;
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
    public class LRBalanceGeneral : LBase<comprobante>
    {
        public List<ERDatosBasicoBalanceGeneral> ReporteDatosBasicosBalanceGeneral(string usuario, string empresa, string gestion, string fechagestion, string moneda)
        {
            try
            {
                List<ERDatosBasicoBalanceGeneral> basicos = new List<ERDatosBasicoBalanceGeneral>();
                ERDatosBasicoBalanceGeneral eRDatosBasico = new ERDatosBasicoBalanceGeneral();
                eRDatosBasico.Usuario = usuario;
                eRDatosBasico.NombreEmpresa = empresa;
                eRDatosBasico.Gestion = gestion + " - " + fechagestion;
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

        public List<ECabeceraGeneralActivo> CabeceraGeneralActivos(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<ECabeceraGeneralActivo> cabeceraActivos = new List<ECabeceraGeneralActivo>();

                    var moneda = (from x in esquema.empresaMoneda
                                  where x.idEmpresa == idempresa
                                  && x.activo == 1
                                  select x).FirstOrDefault();

                    var gestion = (from x in esquema.gestion
                                   where x.id == idgestion
                                   select x).FirstOrDefault();

                    var empresa = (from x in esquema.empresa
                                   where x.id == idempresa
                                   select x).FirstOrDefault();

                    var comprobante = (from x in esquema.comprobante
                                       where x.idEmpresa == idempresa
                                       && x.fecha >= gestion.fechainicio && x.fecha <= gestion.fechafin
                                       && x.estado != (int)EstadosComprobante.Anulado
                                       select x).ToList();

                    string codigoAuxac = "";
                    switch (empresa.niveles)
                    {
                        case 3:
                            codigoAuxac = "1.0.0";
                            break;
                        case 4:
                            codigoAuxac = "1.0.0.0";
                            break;
                        case 5:
                            codigoAuxac = "1.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxac = "1.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxac = "1.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabueloactivo = (from x in esquema.cuenta
                                              where x.idEmpresa == idempresa
                                              && x.nivel == 1 && x.codigo == codigoAuxac
                                              select x).FirstOrDefault();
                    foreach (var i in comprobante)
                    {
                        ECabeceraGeneralActivo d = new ECabeceraGeneralActivo();
                        d.CodigoActivo = cuentaabueloactivo.codigo;
                        d.CuentaActivo = cuentaabueloactivo.nombre;


                        var detallecomprobante = (from x in esquema.detalleComprobante
                                                  where x.idComprobante == i.id
                                                  select x).ToList();

                        foreach (var j in detallecomprobante)
                        {

                            if (j.montoDebe > 0)
                            {

                                if (moneda.idMonedaPrincipal == idmoneda)
                                {
                                    d.TotalActivo = d.TotalActivo + j.montoDebe;

                                }
                                else
                                {
                                    d.TotalActivo = d.TotalActivo + j.montoDebeAlt;
                                }
                            }
                        }
                        cabeceraActivos.Add(d);

                    }

                    return cabeceraActivos;

                }
                catch (Exception ex)
                {
                    throw new BussinessException("No se pudo obtener el balance General");
                }

            }
        }

        public List<ECabeceraGeneralPasivoPatrimonio> CabeceraGeneralPasivoPatrimonios(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<ECabeceraGeneralPasivoPatrimonio> cabeceraPasivoPatrimonios = new List<ECabeceraGeneralPasivoPatrimonio>();

                    var moneda = (from x in esquema.empresaMoneda
                                  where x.idEmpresa == idempresa
                                  && x.activo == 1
                                  select x).FirstOrDefault();

                    var gestion = (from x in esquema.gestion
                                   where x.id == idgestion
                                   select x).FirstOrDefault();

                    var empresa = (from x in esquema.empresa
                                   where x.id == idempresa
                                   select x).FirstOrDefault();

                    var comprobante = (from x in esquema.comprobante
                                       where x.id == idempresa
                                       && x.fecha >= gestion.fechainicio && x.fecha <= gestion.fechafin
                                       && x.estado != (int)EstadosComprobante.Anulado
                                       select x).ToList();

                    string codigoAuxpas = "";
                    string codigoAuxpat = "";
                    switch (empresa.niveles)
                    {
                        case 3:
                            codigoAuxpas = "2.0.0";
                            codigoAuxpat = "3.0.0";
                            break;
                        case 4:
                            codigoAuxpas = "2.0.0.0";
                            codigoAuxpat = "3.0.0.0";
                            break;
                        case 5:
                            codigoAuxpas = "2.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxpas = "2.0.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxpas = "2.0.0.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabuelopasivo = (from x in esquema.cuenta
                                              where x.idEmpresa == idempresa
                                              && x.nivel == 1 && x.codigo == codigoAuxpas
                                              select x).FirstOrDefault();

                    var cuentaabuelopatrimonio = (from x in esquema.cuenta
                                                  where x.idEmpresa == idempresa
                                                  && x.nivel == 1 && x.codigo == codigoAuxpat
                                                  select x).FirstOrDefault();
                    foreach (var i in comprobante)
                    {
                        ECabeceraGeneralPasivoPatrimonio d = new ECabeceraGeneralPasivoPatrimonio();
                        d.CodigoPasivo = cuentaabuelopasivo.codigo;
                        d.CuentaPasivo = cuentaabuelopasivo.nombre;
                        d.CodigoPatrimonio = cuentaabuelopatrimonio.codigo;
                        d.CuentaPatrimonio = cuentaabuelopatrimonio.nombre;


                        var detallecomprobante = (from x in esquema.detalleComprobante
                                                  where x.idComprobante == i.id
                                                  select x).ToList();

                        foreach (var j in detallecomprobante)
                        {

                            if (j.montoHaber > 0)
                            {
                                if (moneda.idMonedaPrincipal == idmoneda)
                                {
                                    d.TotalPasivoPatrimonio = d.TotalPasivoPatrimonio + j.montoHaber;

                                }
                                else
                                {
                                    d.TotalPasivoPatrimonio = d.TotalPasivoPatrimonio + j.montoHaberAlt;
                                }
                            }


                        }
                        cabeceraPasivoPatrimonios.Add(d);
                    }

                    return cabeceraPasivoPatrimonios;

                }
                catch (Exception ex)
                {
                    throw new BussinessException("No se pudo obtener el Balance General");
                }

            }
        }

        public List<EBalanceGeneralActivo> ReporteBalanceGeneralActivo(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<EBalanceGeneralActivo> balanceInicialActivos = new List<EBalanceGeneralActivo>();
                    var moneda = (from x in esquema.empresaMoneda
                                  where x.idEmpresa == idempresa
                                  && x.activo == 1
                                  select x).FirstOrDefault();

                    var empresa = (from x in esquema.empresa
                                   where x.id == idempresa
                                   select x).FirstOrDefault();

                    var gestion = (from x in esquema.gestion
                                   where x.id == idgestion
                                   select x).FirstOrDefault();

                    var comprobante = (from x in esquema.comprobante
                                       where x.idEmpresa == idempresa
                                       && x.fecha >= gestion.fechainicio && x.fecha <= gestion.fechafin
                                       && x.estado != (int)EstadosComprobante.Anulado
                                       select x).ToList();
                    double totalcabecera = 0;
                    double totalgeneral = 0;
                    string codigoAuxac = "";
                    switch (empresa.niveles)
                    {
                        case 3:
                            codigoAuxac = "1.0.0";
                            break;
                        case 4:
                            codigoAuxac = "1.0.0.0";
                            break;
                        case 5:
                            codigoAuxac = "1.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxac = "1.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxac = "1.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabueloactivos = (from x in esquema.cuenta
                                               where x.idEmpresa == idempresa
                                               && x.nivel == 1 && x.codigo == codigoAuxac
                                               select x).ToList();

                    if (empresa.niveles == 3)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.cuenta
                                           where x.idEmpresa == idempresa
                                           && x.nivel == 2 && x.idCuentaPadre == l.id
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {

                                foreach (var i in comprobante)
                                {
                                    var detallecomprobante = (from x in esquema.detalleComprobante
                                                              where x.idComprobante == i.id
                                                              select x).ToList();
                                    foreach (var j in detallecomprobante)
                                    {
                                        EBalanceGeneralActivo d = new EBalanceGeneralActivo();
                                        if (z.id == j.cuenta.idCuentaPadre)
                                        {
                                            d.CuentaCabecera1 = z.nombre;
                                            d.CodigoCabecera1 = z.codigo;
                                            d.IdCabecera1 = z.id;


                                            d.CodigoCuenta = j.cuenta.codigo;
                                            d.Cuenta = j.cuenta.nombre;

                                            if (moneda.idMonedaPrincipal == idmoneda)
                                            {
                                                d.Debe = j.montoDebe - j.montoHaber;
                                            }
                                            else
                                            {
                                                d.Debe = j.montoDebeAlt - j.montoHaberAlt;
                                            }
                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                            totalgeneral = totalgeneral + d.TotalCabecera1;
                                            balanceInicialActivos.Add(d);
                                        }

                                    }

                                }
                            }
                        }
                    }
                    else if (empresa.niveles == 4)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.cuenta
                                           where x.idEmpresa == idempresa
                                           && x.nivel == 2 && x.idCuentaPadre == l.id
                                           select x).ToList();

                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.cuenta
                                                where x.idEmpresa == idempresa
                                                && x.nivel == 3 && x.idCuentaPadre == z.id
                                                select x).ToList();
                                foreach (var q in cuentas2)
                                {

                                    foreach (var i in comprobante)
                                    {
                                        var detallecomprobante = (from x in esquema.detalleComprobante
                                                                  where x.idComprobante == i.id
                                                                  select x).ToList();
                                        foreach (var j in detallecomprobante)
                                        {
                                            EBalanceGeneralActivo d = new EBalanceGeneralActivo();
                                            if (q.id == j.cuenta.idCuentaPadre)
                                            {
                                                d.CuentaCabecera5 = z.nombre;
                                                d.CodigoCabecera5 = z.codigo;
                                                d.IdCabecera5 = z.id;
                                                d.CuentaCabecera1 = q.nombre;
                                                d.CodigoCabecera1 = q.codigo;
                                                d.IdCabecera1 = q.id;


                                                d.CodigoCuenta = j.cuenta.codigo;
                                                d.Cuenta = j.cuenta.nombre;

                                                if (moneda.idMonedaPrincipal == idmoneda)
                                                {
                                                    d.Debe = j.montoDebe - j.montoHaber;
                                                }
                                                else
                                                {
                                                    d.Debe = j.montoDebeAlt - j.montoHaberAlt;
                                                }
                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera1;
                                                d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                balanceInicialActivos.Add(d);
                                            }

                                        }

                                    }


                                }
                            }
                        }
                    }
                    else if (empresa.niveles == 5)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.cuenta
                                           where x.idEmpresa == idempresa
                                           && x.nivel == 2 && x.idCuentaPadre == l.id
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.cuenta
                                                where x.idEmpresa == idempresa
                                                && x.nivel == 3 && x.idCuentaPadre == z.id
                                                select x).ToList();
                                foreach (var q in cuentas2)
                                {
                                    var cuentas3 = (from x in esquema.cuenta
                                                    where x.idEmpresa == idempresa
                                                    && x.nivel == 4 && x.idCuentaPadre == q.id
                                                    select x).ToList();

                                    foreach (var w in cuentas3)
                                    {

                                        foreach (var i in comprobante)
                                        {
                                            var detallecomprobante = (from x in esquema.detalleComprobante
                                                                      where x.idComprobante == i.id
                                                                      select x).ToList();
                                            foreach (var j in detallecomprobante)
                                            {
                                                EBalanceGeneralActivo d = new EBalanceGeneralActivo();
                                                if (w.id == j.cuenta.idCuentaPadre)
                                                {
                                                    d.IdCabecera5 = z.id;
                                                    d.CuentaCabecera5 = z.nombre;
                                                    d.CodigoCabecera5 = z.codigo;
                                                    d.IdCabecera4 = q.id;
                                                    d.CuentaCabecera4 = q.nombre;
                                                    d.CodigoCabecera4 = q.codigo;
                                                    d.CuentaCabecera1 = w.nombre;
                                                    d.CodigoCabecera1 = w.codigo;
                                                    d.IdCabecera1 = w.id;


                                                    d.CodigoCuenta = j.cuenta.codigo;
                                                    d.Cuenta = j.cuenta.nombre;

                                                    if (moneda.idMonedaPrincipal == idmoneda)
                                                    {
                                                        d.Debe = j.montoDebe - j.montoHaber;
                                                    }
                                                    else
                                                    {
                                                        d.Debe = j.montoDebeAlt - j.montoHaberAlt;
                                                    }
                                                    d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                    d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera1;
                                                    d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                    balanceInicialActivos.Add(d);

                                                }

                                            }

                                        }


                                    }

                                }
                            }
                        }
                    }
                    else if (empresa.niveles == 6)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.cuenta
                                           where x.idEmpresa == idempresa
                                           && x.nivel == 2 && x.idCuentaPadre == l.id
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.cuenta
                                                where x.idEmpresa == idempresa
                                                && x.nivel == 3 && x.idCuentaPadre == z.id
                                                select x).ToList();
                                foreach (var q in cuentas2)
                                {

                                    var cuentas3 = (from x in esquema.cuenta
                                                    where x.idEmpresa == idempresa
                                                    && x.nivel == 4 && x.idCuentaPadre == q.id
                                                    select x).ToList();
                                    foreach (var w in cuentas3)
                                    {
                                        var cuentas4 = (from x in esquema.cuenta
                                                        where x.idEmpresa == idempresa
                                                        && x.nivel == 5 && x.idCuentaPadre == w.id
                                                        select x).ToList();
                                        foreach (var e in cuentas4)
                                        {

                                            foreach (var i in comprobante)
                                            {
                                                var detallecomprobante = (from x in esquema.detalleComprobante
                                                                          where x.idComprobante == i.id
                                                                          select x).ToList();
                                                foreach (var j in detallecomprobante)
                                                {
                                                    EBalanceGeneralActivo d = new EBalanceGeneralActivo();
                                                    if (e.id == j.cuenta.idCuentaPadre)
                                                    {
                                                        d.CuentaCabecera1 = e.nombre;
                                                        d.CodigoCabecera1 = e.codigo;
                                                        d.IdCabecera1 = e.id;
                                                        d.CuentaCabecera5 = z.nombre;
                                                        d.CodigoCabecera5 = z.codigo;
                                                        d.IdCabecera5 = z.id;
                                                        d.CuentaCabecera4 = q.nombre;
                                                        d.CodigoCabecera4 = q.codigo;
                                                        d.IdCabecera4 = q.id;
                                                        d.CuentaCabecera3 = w.nombre;
                                                        d.CodigoCabecera3 = w.codigo;
                                                        d.IdCabecera3 = w.id;



                                                        d.CodigoCuenta = j.cuenta.codigo;
                                                        d.Cuenta = j.cuenta.nombre;

                                                        if (moneda.idMonedaPrincipal == idmoneda)
                                                        {
                                                            d.Debe = j.montoDebe - j.montoHaber;
                                                        }
                                                        else
                                                        {
                                                            d.Debe = j.montoDebeAlt - j.montoHaberAlt;
                                                        }
                                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                        d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera1;
                                                        d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                        d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;


                                                        balanceInicialActivos.Add(d);
                                                    }

                                                }

                                            }


                                        }

                                    }

                                }
                            }
                        }
                    }
                    else if (empresa.niveles == 7)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.cuenta
                                           where x.idEmpresa == idempresa
                                           && x.nivel == 2 && x.idCuentaPadre == l.id
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.cuenta
                                                where x.idEmpresa == idempresa
                                                && x.nivel == 3 && x.idCuentaPadre == z.id
                                                select x).ToList();
                                foreach (var q in cuentas2)
                                {

                                    var cuentas3 = (from x in esquema.cuenta
                                                    where x.idEmpresa == idempresa
                                                    && x.nivel == 4 && x.idCuentaPadre == q.id
                                                    select x).ToList();
                                    foreach (var w in cuentas3)
                                    {
                                        var cuentas4 = (from x in esquema.cuenta
                                                        where x.idEmpresa == idempresa
                                                        && x.nivel == 5 && x.idCuentaPadre == w.id
                                                        select x).ToList();
                                        foreach (var e in cuentas4)
                                        {
                                            var cuentas5 = (from x in esquema.cuenta
                                                            where x.idEmpresa == idempresa
                                                            && x.nivel == 6 && x.idCuentaPadre == e.id
                                                            select x).ToList();
                                            foreach (var r in cuentas5)
                                            {
                                                foreach (var i in comprobante)
                                                {
                                                    var detallecomprobante = (from x in esquema.detalleComprobante
                                                                              where x.idComprobante == i.id
                                                                              select x).ToList();
                                                    foreach (var j in detallecomprobante)
                                                    {
                                                        EBalanceGeneralActivo d = new EBalanceGeneralActivo();
                                                        if (r.id == j.cuenta.idCuentaPadre)
                                                        {
                                                            d.CuentaCabecera1 = r.nombre;
                                                            d.CodigoCabecera1 = r.codigo;
                                                            d.IdCabecera1 = r.id;
                                                            d.CuentaCabecera2 = e.nombre;
                                                            d.CodigoCabecera2 = e.codigo;
                                                            d.IdCabecera2 = e.id;
                                                            d.CuentaCabecera3 = w.nombre;
                                                            d.CodigoCabecera3 = w.codigo;
                                                            d.IdCabecera3 = w.id;
                                                            d.CuentaCabecera4 = q.nombre;
                                                            d.CodigoCabecera4 = q.codigo;
                                                            d.IdCabecera4 = q.id;
                                                            d.CuentaCabecera5 = z.nombre;
                                                            d.CodigoCabecera5 = z.codigo;
                                                            d.IdCabecera5 = z.id;


                                                            d.CodigoCuenta = j.cuenta.codigo;
                                                            d.Cuenta = j.cuenta.nombre;

                                                            if (moneda.idMonedaPrincipal == idmoneda)
                                                            {
                                                                d.Debe = j.montoDebe - j.montoHaber;
                                                            }
                                                            else
                                                            {
                                                                d.Debe = j.montoDebeAlt - j.montoHaberAlt;
                                                            }
                                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                            d.TotalCabecera2 = d.TotalCabecera2 + d.TotalCabecera1;
                                                            d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera2;
                                                            d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                            d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;

                                                            balanceInicialActivos.Add(d);


                                                        }

                                                    }

                                                }


                                            }

                                        }

                                    }

                                }
                            }
                        }
                    }
                    return balanceInicialActivos;

                }
                catch (Exception ex)
                {
                    throw new BussinessException("No se pudo obtener el balance General");
                }

            }
        }

        public List<EBalanceGeneralPasivoPatrimonio> ReporteBalanceGeneralPasivoPatrimonio(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<EBalanceGeneralPasivoPatrimonio> balanceInicialPasivoPatrimonios = new List<EBalanceGeneralPasivoPatrimonio>();

                    var moneda = (from x in esquema.empresaMoneda
                                  where x.idEmpresa == idempresa
                                  && x.activo == 1
                                  select x).FirstOrDefault();

                    var empresa = (from x in esquema.empresa
                                   where x.id == idempresa
                                   select x).FirstOrDefault();

                    var gestion = (from x in esquema.gestion
                                   where x.id == idgestion
                                   select x).FirstOrDefault();

                    var comprobante = (from x in esquema.comprobante
                                       where x.idEmpresa == idempresa
                                       && x.fecha >= gestion.fechainicio && x.fecha <= gestion.fechafin
                                       && x.estado != (int)EstadosComprobante.Anulado
                                       select x).ToList();
                    double totalcabecera = 0;
                    double totalgeneral = 0;
                    string codigoAuxpas = "";
                    string codigoAuxpat = "";
                    switch (empresa.niveles)
                    {
                        case 3:
                            codigoAuxpas = "2.0.0";
                            codigoAuxpat = "3.0.0";
                            break;
                        case 4:
                            codigoAuxpas = "2.0.0.0";
                            codigoAuxpat = "3.0.0.0";
                            break;
                        case 5:
                            codigoAuxpas = "2.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxpas = "2.0.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxpas = "2.0.0.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabuelopasivopatrimonio = (from x in esquema.cuenta
                                                        where x.idEmpresa == idempresa
                                                        && x.nivel == 1 && x.codigo == codigoAuxpas || x.codigo == codigoAuxpat
                                                        select x).ToList();
                    if (empresa.niveles == 3)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.cuenta
                                           where x.idEmpresa == idempresa
                                           && x.nivel == 2 && x.idCuentaPadre == l.id
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {

                                foreach (var i in comprobante)
                                {

                                    var detallecomprobante = (from x in esquema.detalleComprobante
                                                              where x.idComprobante == i.id
                                                              select x).ToList();

                                    foreach (var j in detallecomprobante)
                                    {
                                        EBalanceGeneralPasivoPatrimonio d = new EBalanceGeneralPasivoPatrimonio();
                                        if (z.id == j.cuenta.idCuentaPadre)
                                        {
                                            d.CuentaCabecera1 = z.nombre;
                                            d.CodigoCabecera1 = z.codigo;
                                            d.IdCabecera1 = z.id;


                                            d.CodigoCuenta = j.cuenta.codigo;
                                            d.Cuenta = j.cuenta.nombre;

                                            if (moneda.idMonedaPrincipal == idmoneda)
                                            {
                                                d.Haber = j.montoHaber - j.montoDebe;
                                            }
                                            else
                                            {
                                                d.Haber = j.montoHaberAlt - j.montoDebeAlt;
                                            }
                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera1;
                                            balanceInicialPasivoPatrimonios.Add(d);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (empresa.niveles == 4)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.cuenta
                                           where x.idEmpresa == idempresa
                                           && x.nivel == 2 && x.idCuentaPadre == l.id
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.cuenta
                                                where x.idEmpresa == idempresa
                                                && x.nivel == 3 && x.idCuentaPadre == z.id
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {

                                    foreach (var i in comprobante)
                                    {

                                        var detallecomprobante = (from x in esquema.detalleComprobante
                                                                  where x.idComprobante == i.id
                                                                  select x).ToList();

                                        foreach (var j in detallecomprobante)
                                        {
                                            EBalanceGeneralPasivoPatrimonio d = new EBalanceGeneralPasivoPatrimonio();
                                            if (q.id == j.cuenta.idCuentaPadre)
                                            {
                                                d.CuentaCabecera1 = q.nombre;
                                                d.CodigoCabecera1 = q.codigo;
                                                d.IdCabecera1 = q.id;
                                                d.CuentaCabecera5 = z.nombre;
                                                d.CodigoCabecera5 = z.codigo;
                                                d.IdCabecera5 = z.id;


                                                d.CodigoCuenta = j.cuenta.codigo;
                                                d.Cuenta = j.cuenta.nombre;

                                                if (moneda.idMonedaPrincipal == idmoneda)
                                                {
                                                    d.Haber = j.montoHaber - j.montoDebe;
                                                }
                                                else
                                                {
                                                    d.Haber = j.montoHaberAlt - j.montoDebeAlt;
                                                }
                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera1;
                                                d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                balanceInicialPasivoPatrimonios.Add(d);

                                            }
                                        }
                                    }


                                }
                            }
                        }
                    }
                    else if (empresa.niveles == 5)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.cuenta
                                           where x.idEmpresa == idempresa
                                           && x.nivel == 2 && x.idCuentaPadre == l.id
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.cuenta
                                                where x.idEmpresa == idempresa
                                                && x.nivel == 3 && x.idCuentaPadre == z.id
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.cuenta
                                                    where x.idEmpresa == idempresa
                                                    && x.nivel == 4 && x.idCuentaPadre == q.id
                                                    select x).ToList();
                                    foreach (var w in cuentas2)
                                    {
                                        foreach (var i in comprobante)
                                        {

                                            var detallecomprobante = (from x in esquema.detalleComprobante
                                                                      where x.idComprobante == i.id
                                                                      select x).ToList();

                                            foreach (var j in detallecomprobante)
                                            {
                                                EBalanceGeneralPasivoPatrimonio d = new EBalanceGeneralPasivoPatrimonio();
                                                if (w.id == j.cuenta.idCuentaPadre)
                                                {
                                                    d.CuentaCabecera1 = w.nombre;
                                                    d.CodigoCabecera1 = w.codigo;
                                                    d.IdCabecera1 = w.id;
                                                    d.CuentaCabecera4 = q.nombre;
                                                    d.CodigoCabecera4 = q.codigo;
                                                    d.IdCabecera4 = q.id;
                                                    d.CuentaCabecera5 = z.nombre;
                                                    d.CodigoCabecera5 = z.codigo;
                                                    d.IdCabecera5 = z.id;


                                                    d.CodigoCuenta = j.cuenta.codigo;
                                                    d.Cuenta = j.cuenta.nombre;

                                                    if (moneda.idMonedaPrincipal == idmoneda)
                                                    {
                                                        d.Haber = j.montoHaber - j.montoDebe;
                                                    }
                                                    else
                                                    {
                                                        d.Haber = j.montoHaberAlt - j.montoDebeAlt;
                                                    }
                                                    d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                    d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera1;
                                                    d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                    balanceInicialPasivoPatrimonios.Add(d);

                                                }
                                            }
                                        }


                                    }


                                }
                            }
                        }
                    }
                    else if (empresa.niveles == 6)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.cuenta
                                           where x.idEmpresa == idempresa
                                           && x.nivel == 2 && x.idCuentaPadre == l.id
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.cuenta
                                                where x.idEmpresa == idempresa
                                                && x.nivel == 3 && x.idCuentaPadre == z.id
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.cuenta
                                                    where x.idEmpresa == idempresa
                                                    && x.nivel == 4 && x.idCuentaPadre == q.id
                                                    select x).ToList();
                                    foreach (var w in cuentas2)
                                    {
                                        var cuentas3 = (from x in esquema.cuenta
                                                        where x.idEmpresa == idempresa
                                                        && x.nivel == 5 && x.idCuentaPadre == w.id
                                                        select x).ToList();
                                        foreach (var e in cuentas3)
                                        {
                                            foreach (var i in comprobante)
                                            {

                                                var detallecomprobante = (from x in esquema.detalleComprobante
                                                                          where x.idComprobante == i.id
                                                                          select x).ToList();

                                                foreach (var j in detallecomprobante)
                                                {
                                                    EBalanceGeneralPasivoPatrimonio d = new EBalanceGeneralPasivoPatrimonio();
                                                    if (e.id == j.cuenta.idCuentaPadre)
                                                    {
                                                        d.CuentaCabecera1 = e.nombre;
                                                        d.CodigoCabecera1 = e.codigo;
                                                        d.IdCabecera1 = e.id;
                                                        d.CuentaCabecera3 = w.nombre;
                                                        d.CodigoCabecera3 = w.codigo;
                                                        d.IdCabecera3 = w.id;
                                                        d.CuentaCabecera4 = q.nombre;
                                                        d.CodigoCabecera4 = q.codigo;
                                                        d.IdCabecera4 = q.id;
                                                        d.CuentaCabecera5 = z.nombre;
                                                        d.CodigoCabecera5 = z.codigo;
                                                        d.IdCabecera5 = z.id;


                                                        d.CodigoCuenta = j.cuenta.codigo;
                                                        d.Cuenta = j.cuenta.nombre;

                                                        if (moneda.idMonedaPrincipal == idmoneda)
                                                        {
                                                            d.Haber = j.montoHaber - j.montoDebe;
                                                        }
                                                        else
                                                        {
                                                            d.Haber = j.montoHaberAlt - j.montoDebeAlt;
                                                        }
                                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                        d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera1;
                                                        d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                        d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                        balanceInicialPasivoPatrimonios.Add(d);

                                                    }
                                                }
                                            }


                                        }


                                    }


                                }
                            }
                        }
                    }
                    else if (empresa.niveles == 7)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.cuenta
                                           where x.idEmpresa == idempresa
                                           && x.nivel == 2 && x.idCuentaPadre == l.id
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.cuenta
                                                where x.idEmpresa == idempresa
                                                && x.nivel == 3 && x.idCuentaPadre == z.id
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.cuenta
                                                    where x.idEmpresa == idempresa
                                                    && x.nivel == 4 && x.idCuentaPadre == q.id
                                                    select x).ToList();
                                    foreach (var w in cuentas2)
                                    {
                                        var cuentas3 = (from x in esquema.cuenta
                                                        where x.idEmpresa == idempresa
                                                        && x.nivel == 5 && x.idCuentaPadre == w.id
                                                        select x).ToList();
                                        foreach (var e in cuentas3)
                                        {
                                            var cuentas4 = (from x in esquema.cuenta
                                                            where x.idEmpresa == idempresa
                                                            && x.nivel == 6 && x.idCuentaPadre == e.id
                                                            select x).ToList();
                                            foreach (var r in cuentas4)
                                            {
                                                foreach (var i in comprobante)
                                                {

                                                    var detallecomprobante = (from x in esquema.detalleComprobante
                                                                              where x.idComprobante == i.id
                                                                              select x).ToList();

                                                    foreach (var j in detallecomprobante)
                                                    {
                                                        EBalanceGeneralPasivoPatrimonio d = new EBalanceGeneralPasivoPatrimonio();
                                                        if (r.id == j.cuenta.idCuentaPadre)
                                                        {
                                                            d.CuentaCabecera1 = r.nombre;
                                                            d.CodigoCabecera1 = r.codigo;
                                                            d.IdCabecera1 = r.id;
                                                            d.CuentaCabecera2 = e.nombre;
                                                            d.CodigoCabecera2 = e.codigo;
                                                            d.IdCabecera2 = e.id;
                                                            d.CuentaCabecera3 = w.nombre;
                                                            d.CodigoCabecera3 = w.codigo;
                                                            d.IdCabecera3 = w.id;
                                                            d.CuentaCabecera4 = q.nombre;
                                                            d.CodigoCabecera4 = q.codigo;
                                                            d.IdCabecera4 = q.id;
                                                            d.CuentaCabecera5 = z.nombre;
                                                            d.CodigoCabecera5 = z.codigo;
                                                            d.IdCabecera5 = z.id;


                                                            d.CodigoCuenta = j.cuenta.codigo;
                                                            d.Cuenta = j.cuenta.nombre;

                                                            if (moneda.idMonedaPrincipal == idmoneda)
                                                            {
                                                                d.Haber = j.montoHaber - j.montoDebe;
                                                            }
                                                            else
                                                            {
                                                                d.Haber = j.montoHaberAlt - j.montoDebeAlt;
                                                            }
                                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                            d.TotalCabecera2 = d.TotalCabecera2 + d.TotalCabecera1;
                                                            d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera2;
                                                            d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                            d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                            balanceInicialPasivoPatrimonios.Add(d);

                                                        }
                                                    }
                                                }


                                            }

                                        }


                                    }


                                }
                            }
                        }
                    }

                    return balanceInicialPasivoPatrimonios;

                }
                catch (Exception ex)
                {
                    throw new BussinessException("No se pudo obtener el balance General");
                }

            }
        }

    }
}
