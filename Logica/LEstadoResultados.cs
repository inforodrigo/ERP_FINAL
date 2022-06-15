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
    public class LEstadoResultados : LBase<comprobante>
    {

        public List<ERDatosBasicoEstadoResultado> ReporteDatosBasicoEstadoResultados(string usuario, string empresa, string gestion, string fechagestion, string moneda)
        {
            try
            {
                List<ERDatosBasicoEstadoResultado> basicos = new List<ERDatosBasicoEstadoResultado>();
                ERDatosBasicoEstadoResultado eRDatosBasico = new ERDatosBasicoEstadoResultado();
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

        public List<ECabeceraEstadoResultadosIngreso> CabeceraEstadoResultadosIngreso(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<ECabeceraEstadoResultadosIngreso> cabeceraIngresos = new List<ECabeceraEstadoResultadosIngreso>();

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
                            codigoAuxac = "4.0.0";
                            break;
                        case 4:
                            codigoAuxac = "4.0.0.0";
                            break;
                        case 5:
                            codigoAuxac = "4.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxac = "4.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxac = "4.0.0.0.0.0.0";
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
                        ECabeceraEstadoResultadosIngreso d = new ECabeceraEstadoResultadosIngreso();
                        d.CodigoIngreso = cuentaabueloactivo.codigo;
                        d.CuentaIngreso = cuentaabueloactivo.nombre;


                        var detallecomprobante = (from x in esquema.detalleComprobante
                                                  where x.idComprobante == i.id
                                                  select x).ToList();

                        foreach (var j in detallecomprobante)
                        {
                            if (moneda.idMonedaPrincipal == idmoneda)
                            {
                                d.TotalActivo = d.TotalActivo + (j.montoDebe - j.montoHaber);

                            }
                            else
                            {
                                d.TotalActivo = d.TotalActivo + (j.montoDebeAlt - j.montoHaberAlt);
                            }
                        }
                        cabeceraIngresos.Add(d);

                    }

                    return cabeceraIngresos;

                }
                catch (Exception ex)
                {
                    throw new BussinessException("No se pudo obtener el Estado de Resultado");
                }
            }
        }

        public List<ECabeceraEstadoResultadosCosto> CabeceraEstadoResultadosCosto(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<ECabeceraEstadoResultadosCosto> cabeceraCostos = new List<ECabeceraEstadoResultadosCosto>();

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

                    string codigoAuxpas = "";
                    switch (empresa.niveles)
                    {
                        case 3:
                            codigoAuxpas = "5.1.0";
                            break;
                        case 4:
                            codigoAuxpas = "5.1.0.0";
                            break;
                        case 5:
                            codigoAuxpas = "5.1.0.0.0";
                            break;
                        case 6:
                            codigoAuxpas = "5.1.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxpas = "5.1.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabuelopasivo = (from x in esquema.cuenta
                                              where x.idEmpresa == idempresa
                                              && x.nivel == 2 && x.codigo == codigoAuxpas
                                              select x).FirstOrDefault();
                    foreach (var i in comprobante)
                    {
                        ECabeceraEstadoResultadosCosto d = new ECabeceraEstadoResultadosCosto();
                        d.CodigoCosto = cuentaabuelopasivo.codigo;
                        d.CuentaCosto = cuentaabuelopasivo.nombre;



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
                        cabeceraCostos.Add(d);
                    }

                    return cabeceraCostos;

                }
                catch (Exception ex)
                {
                    throw new BussinessException("No se pudo obtener el Estado de Resultado");
                }

            }
        }

        public List<ECabeceraEstadoResultadosGasto> CabeceraEstadoResultadosGasto(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<ECabeceraEstadoResultadosGasto> cabeceraGastos = new List<ECabeceraEstadoResultadosGasto>();

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
                            codigoAuxac = "5.2.0";
                            break;
                        case 4:
                            codigoAuxac = "5.2.0.0";
                            break;
                        case 5:
                            codigoAuxac = "5.2.0.0.0";
                            break;
                        case 6:
                            codigoAuxac = "5.2.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxac = "5.2.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabueloactivo = (from x in esquema.cuenta
                                              where x.idEmpresa == idempresa
                                              && x.nivel == 2 && x.codigo == codigoAuxac
                                              select x).FirstOrDefault();
                    foreach (var i in comprobante)
                    {
                        ECabeceraEstadoResultadosGasto d = new ECabeceraEstadoResultadosGasto();
                        d.CodigoGasto = cuentaabueloactivo.codigo;
                        d.CuentaGasto = cuentaabueloactivo.nombre;


                        var detallecomprobante = (from x in esquema.detalleComprobante
                                                  where x.idComprobante == i.id
                                                  select x).ToList();

                        foreach (var j in detallecomprobante)
                        {

                            if (j.montoDebe > 0)
                            {

                                if (moneda.idMonedaPrincipal == idmoneda)
                                {
                                    d.TotalPasivoPatrimonio = d.TotalPasivoPatrimonio + j.montoDebe;

                                }
                                else
                                {
                                    d.TotalPasivoPatrimonio = d.TotalPasivoPatrimonio + j.montoDebeAlt;
                                }
                            }
                        }
                        cabeceraGastos.Add(d);

                    }

                    return cabeceraGastos;

                }
                catch (Exception ex)
                {
                    throw new BussinessException("No se pudo obtener el Estado de Resultados");
                }

            }
        }

        public List<EEstadoResultadosIngreso> ReporteEstadoResultadosIngreso(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<EEstadoResultadosIngreso> EstadoResultadosIngreso = new List<EEstadoResultadosIngreso>();
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
                            codigoAuxac = "4.0.0";
                            break;
                        case 4:
                            codigoAuxac = "4.0.0.0";
                            break;
                        case 5:
                            codigoAuxac = "4.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxac = "4.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxac = "4.0.0.0.0.0.0";
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
                                        EEstadoResultadosIngreso d = new EEstadoResultadosIngreso();
                                        if (z.id == j.cuenta.idCuentaPadre)
                                        {
                                            d.CuentaCabecera1 = z.nombre;
                                            d.CodigoCabecera1 = z.codigo;
                                            d.IdCabecera1 = z.id;


                                            d.CodigoCuenta = j.cuenta.codigo;
                                            d.Cuenta = j.cuenta.nombre;

                                            if (moneda.idMonedaPrincipal == idmoneda)
                                            {
                                                d.Debe = j.montoHaber - j.montoDebe;
                                            }
                                            else
                                            {
                                                d.Debe = j.montoHaberAlt - j.montoDebeAlt;
                                            }
                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera1;
                                            EstadoResultadosIngreso.Add(d);
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
                                            EEstadoResultadosIngreso d = new EEstadoResultadosIngreso();
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
                                                    d.Debe = j.montoHaber - j.montoDebe;
                                                }
                                                else
                                                {
                                                    d.Debe = j.montoHaberAlt - j.montoDebeAlt;
                                                }
                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera1;
                                                d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                EstadoResultadosIngreso.Add(d);
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
                                                EEstadoResultadosIngreso d = new EEstadoResultadosIngreso();
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
                                                        d.Debe = j.montoHaber - j.montoDebe;
                                                    }
                                                    else
                                                    {
                                                        d.Debe = j.montoHaberAlt - j.montoDebeAlt;
                                                    }
                                                    d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                    d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera1;
                                                    d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                    EstadoResultadosIngreso.Add(d);

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
                                                    EEstadoResultadosIngreso d = new EEstadoResultadosIngreso();
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
                                                            d.Debe = j.montoHaber - j.montoDebe;
                                                        }
                                                        else
                                                        {
                                                            d.Debe = j.montoHaberAlt - j.montoDebeAlt;
                                                        }
                                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                        d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera1;
                                                        d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                        d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;


                                                        EstadoResultadosIngreso.Add(d);
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
                                                        EEstadoResultadosIngreso d = new EEstadoResultadosIngreso();
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
                                                                d.Debe = j.montoHaber - j.montoDebe;
                                                            }
                                                            else
                                                            {
                                                                d.Debe = j.montoHaberAlt - j.montoDebeAlt;
                                                            }
                                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                            d.TotalCabecera2 = d.TotalCabecera2 + d.TotalCabecera1;
                                                            d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera2;
                                                            d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                            d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;

                                                            EstadoResultadosIngreso.Add(d);


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
                    return EstadoResultadosIngreso;

                }
                catch (Exception ex)
                {
                    throw new BussinessException("no se pudo obtener El Estado de Resultados");
                }

            }
        }

        public List<EEstadoResultadosCosto> ReporteEstadoResultadosCosto(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<EEstadoResultadosCosto> EstadoResultadosCosto = new List<EEstadoResultadosCosto>();

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
                            codigoAuxpas = "5.1.0";
                            break;
                        case 4:
                            codigoAuxpas = "5.1.0.0";
                            break;
                        case 5:
                            codigoAuxpas = "5.1.0.0.0";
                            break;
                        case 6:
                            codigoAuxpas = "5.1.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxpas = "5.1.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabuelopasivopatrimonio = (from x in esquema.cuenta
                                                        where x.idEmpresa == idempresa
                                                        && x.nivel == 2 && x.codigo == codigoAuxpas
                                                        select x).ToList();
                    if (empresa.niveles == 3)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {

                            foreach (var i in comprobante)
                            {

                                var detallecomprobante = (from x in esquema.detalleComprobante
                                                          where x.idComprobante == i.id
                                                          select x).ToList();

                                foreach (var j in detallecomprobante)
                                {
                                    EEstadoResultadosCosto d = new EEstadoResultadosCosto();
                                    if (l.id == j.cuenta.idCuentaPadre)
                                    {
                                        d.CuentaCabecera1 = l.nombre;
                                        d.CodigoCabecera1 = l.codigo;
                                        d.IdCabecera1 = l.id;


                                        d.CodigoCuenta = j.cuenta.codigo;
                                        d.Cuenta = j.cuenta.nombre;

                                        if (moneda.idMonedaPrincipal == idmoneda)
                                        {
                                            d.Haber = j.montoDebe - j.montoHaber;
                                        }
                                        else
                                        {
                                            d.Haber = j.montoDebeAlt - j.montoHaberAlt;
                                        }
                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera1;
                                        EstadoResultadosCosto.Add(d);
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
                                           && x.nivel == 3 && x.idCuentaPadre == l.id
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
                                        EEstadoResultadosCosto d = new EEstadoResultadosCosto();
                                        if (l.id == j.cuenta.idCuentaPadre)
                                        {
                                            d.CuentaCabecera1 = z.nombre;
                                            d.CodigoCabecera1 = z.codigo;
                                            d.IdCabecera1 = z.id;
                                            d.CuentaCabecera5 = l.nombre;
                                            d.CodigoCabecera5 = l.codigo;
                                            d.IdCabecera5 = l.id;


                                            d.CodigoCuenta = j.cuenta.codigo;
                                            d.Cuenta = j.cuenta.nombre;

                                            if (moneda.idMonedaPrincipal == idmoneda)
                                            {
                                                d.Haber = j.montoDebe - j.montoHaber;
                                            }
                                            else
                                            {
                                                d.Haber = j.montoDebeAlt - j.montoHaberAlt;
                                            }
                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                            d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera1;
                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                            EstadoResultadosCosto.Add(d);

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
                                           && x.nivel == 3 && x.idCuentaPadre == l.id
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.cuenta
                                                where x.idEmpresa == idempresa
                                                && x.nivel == 4 && x.idCuentaPadre == z.id
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
                                            EEstadoResultadosCosto d = new EEstadoResultadosCosto();
                                            if (q.id == j.cuenta.idCuentaPadre)
                                            {
                                                d.CuentaCabecera1 = q.nombre;
                                                d.CodigoCabecera1 = q.codigo;
                                                d.IdCabecera1 = q.id;
                                                d.CuentaCabecera4 = z.nombre;
                                                d.CodigoCabecera4 = z.codigo;
                                                d.IdCabecera4 = z.id;
                                                d.CuentaCabecera5 = l.nombre;
                                                d.CodigoCabecera5 = l.codigo;
                                                d.IdCabecera5 = l.id;


                                                d.CodigoCuenta = j.cuenta.codigo;
                                                d.Cuenta = j.cuenta.nombre;

                                                if (moneda.idMonedaPrincipal == idmoneda)
                                                {
                                                    d.Haber = j.montoDebe - j.montoHaber;
                                                }
                                                else
                                                {
                                                    d.Haber = j.montoDebeAlt - j.montoHaberAlt;
                                                }
                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera1;
                                                d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                EstadoResultadosCosto.Add(d);

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
                                           && x.nivel == 3 && x.idCuentaPadre == l.id
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.cuenta
                                                where x.idEmpresa == idempresa
                                                && x.nivel == 4 && x.idCuentaPadre == z.id
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.cuenta
                                                    where x.idEmpresa == idempresa
                                                    && x.nivel == 5 && x.idCuentaPadre == q.id
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
                                                EEstadoResultadosCosto d = new EEstadoResultadosCosto();
                                                if (w.id == j.cuenta.idCuentaPadre)
                                                {
                                                    d.CuentaCabecera1 = w.nombre;
                                                    d.CodigoCabecera1 = w.codigo;
                                                    d.IdCabecera1 = w.id;
                                                    d.CuentaCabecera3 = q.nombre;
                                                    d.CodigoCabecera3 = q.codigo;
                                                    d.IdCabecera3 = q.id;
                                                    d.CuentaCabecera4 = z.nombre;
                                                    d.CodigoCabecera4 = z.codigo;
                                                    d.IdCabecera4 = z.id;
                                                    d.CuentaCabecera5 = l.nombre;
                                                    d.CodigoCabecera5 = l.codigo;
                                                    d.IdCabecera5 = l.id;


                                                    d.CodigoCuenta = j.cuenta.codigo;
                                                    d.Cuenta = j.cuenta.nombre;

                                                    if (moneda.idMonedaPrincipal == idmoneda)
                                                    {
                                                        d.Haber = j.montoDebe - j.montoHaber;
                                                    }
                                                    else
                                                    {
                                                        d.Haber = j.montoDebeAlt - j.montoHaberAlt;
                                                    }
                                                    d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                    d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera1;
                                                    d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                    d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                    EstadoResultadosCosto.Add(d);

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
                                           && x.nivel == 3 && x.idCuentaPadre == l.id
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.cuenta
                                                where x.idEmpresa == idempresa
                                                && x.nivel == 4 && x.idCuentaPadre == z.id
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.cuenta
                                                    where x.idEmpresa == idempresa
                                                    && x.nivel == 5 && x.idCuentaPadre == q.id
                                                    select x).ToList();
                                    foreach (var w in cuentas2)
                                    {
                                        var cuentas3 = (from x in esquema.cuenta
                                                        where x.idEmpresa == idempresa
                                                        && x.nivel == 6 && x.idCuentaPadre == w.id
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
                                                    EEstadoResultadosCosto d = new EEstadoResultadosCosto();
                                                    if (e.id == j.cuenta.idCuentaPadre)
                                                    {
                                                        d.CuentaCabecera1 = e.nombre;
                                                        d.CodigoCabecera1 = e.codigo;
                                                        d.IdCabecera1 = e.id;
                                                        d.CuentaCabecera2 = w.nombre;
                                                        d.CodigoCabecera2 = w.codigo;
                                                        d.IdCabecera2 = w.id;
                                                        d.CuentaCabecera3 = q.nombre;
                                                        d.CodigoCabecera3 = q.codigo;
                                                        d.IdCabecera3 = q.id;
                                                        d.CuentaCabecera4 = z.nombre;
                                                        d.CodigoCabecera4 = z.codigo;
                                                        d.IdCabecera4 = z.id;
                                                        d.CuentaCabecera5 = l.nombre;
                                                        d.CodigoCabecera5 = l.codigo;
                                                        d.IdCabecera5 = l.id;


                                                        d.CodigoCuenta = j.cuenta.codigo;
                                                        d.Cuenta = j.cuenta.nombre;

                                                        if (moneda.idMonedaPrincipal == idmoneda)
                                                        {
                                                            d.Haber = j.montoDebe - j.montoHaber;
                                                        }
                                                        else
                                                        {
                                                            d.Haber = j.montoDebeAlt - j.montoHaberAlt;
                                                        }
                                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                        d.TotalCabecera2 = d.TotalCabecera2 + d.TotalCabecera1;
                                                        d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera2;
                                                        d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                        d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                        EstadoResultadosCosto.Add(d);

                                                    }
                                                }
                                            }

                                        }


                                    }


                                }
                            }
                        }
                    }

                    return EstadoResultadosCosto;

                }
                catch (Exception ex)
                {
                    throw new BussinessException("No se pudo obtener el Estado de Resultados");
                }

            }
        }

        public List<EEstadoResultadosGasto> ReporteEstadoResultadosGasto(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<EEstadoResultadosGasto> EstadoResultadosGasto = new List<EEstadoResultadosGasto>();

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
                            codigoAuxpat = "5.2.0";
                            break;
                        case 4:
                            codigoAuxpat = "5.2.0.0";
                            break;
                        case 5:
                            codigoAuxpat = "5.2.0.0.0";
                            break;
                        case 6:
                            codigoAuxpat = "5.2.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxpat = "5.2.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabuelopasivopatrimonio = (from x in esquema.cuenta
                                                        where x.idEmpresa == idempresa
                                                        && x.nivel == 2 && x.codigo == codigoAuxpat
                                                        select x).ToList();
                    if (empresa.niveles == 3)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {

                            foreach (var i in comprobante)
                            {

                                var detallecomprobante = (from x in esquema.detalleComprobante
                                                          where x.idComprobante == i.id
                                                          select x).ToList();

                                foreach (var j in detallecomprobante)
                                {
                                    EEstadoResultadosGasto d = new EEstadoResultadosGasto();
                                    if (l.id == j.cuenta.idCuentaPadre)
                                    {
                                        d.CuentaCabecera1 = l.nombre;
                                        d.CodigoCabecera1 = l.codigo;
                                        d.IdCabecera1 = l.id;

                                        d.CodigoCuenta = j.cuenta.codigo;
                                        d.Cuenta = j.cuenta.nombre;

                                        if (moneda.idMonedaPrincipal == idmoneda)
                                        {
                                            d.Haber = j.montoDebe - j.montoHaber;
                                        }
                                        else
                                        {
                                            d.Haber = j.montoDebeAlt - j.montoHaberAlt;
                                        }
                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera1;
                                        EstadoResultadosGasto.Add(d);
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
                                           && x.nivel == 3 && x.idCuentaPadre == l.id
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
                                        EEstadoResultadosGasto d = new EEstadoResultadosGasto();
                                        if (l.id == j.cuenta.idCuentaPadre)
                                        {
                                            d.CuentaCabecera1 = z.nombre;
                                            d.CodigoCabecera1 = z.codigo;
                                            d.IdCabecera1 = z.id;
                                            d.CuentaCabecera5 = l.nombre;
                                            d.CodigoCabecera5 = l.codigo;
                                            d.IdCabecera5 = l.id;


                                            d.CodigoCuenta = j.cuenta.codigo;
                                            d.Cuenta = j.cuenta.nombre;

                                            if (moneda.idMonedaPrincipal == idmoneda)
                                            {
                                                d.Haber = j.montoDebe - j.montoHaber;
                                            }
                                            else
                                            {
                                                d.Haber = j.montoDebeAlt - j.montoHaberAlt;
                                            }
                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                            d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera1;
                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                            EstadoResultadosGasto.Add(d);

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
                                           && x.nivel == 3 && x.idCuentaPadre == l.id
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.cuenta
                                                where x.idEmpresa == idempresa
                                                && x.nivel == 4 && x.idCuentaPadre == z.id
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
                                            EEstadoResultadosGasto d = new EEstadoResultadosGasto();
                                            if (q.id == j.cuenta.idCuentaPadre)
                                            {
                                                d.CuentaCabecera1 = q.nombre;
                                                d.CodigoCabecera1 = q.codigo;
                                                d.IdCabecera1 = q.id;
                                                d.CuentaCabecera4 = z.nombre;
                                                d.CodigoCabecera4 = z.codigo;
                                                d.IdCabecera4 = z.id;
                                                d.CuentaCabecera5 = l.nombre;
                                                d.CodigoCabecera5 = l.codigo;
                                                d.IdCabecera5 = l.id;


                                                d.CodigoCuenta = j.cuenta.codigo;
                                                d.Cuenta = j.cuenta.nombre;

                                                if (moneda.idMonedaPrincipal == idmoneda)
                                                {
                                                    d.Haber = j.montoDebe - j.montoHaber;
                                                }
                                                else
                                                {
                                                    d.Haber = j.montoDebeAlt - j.montoHaberAlt;
                                                }
                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera1;
                                                d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                EstadoResultadosGasto.Add(d);

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
                                           && x.nivel == 3 && x.idCuentaPadre == l.id
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.cuenta
                                                where x.idEmpresa == idempresa
                                                && x.nivel == 4 && x.idCuentaPadre == z.id
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.cuenta
                                                    where x.idEmpresa == idempresa
                                                    && x.nivel == 5 && x.idCuentaPadre == q.id
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
                                                EEstadoResultadosGasto d = new EEstadoResultadosGasto();
                                                if (w.id == j.cuenta.idCuentaPadre)
                                                {
                                                    d.CuentaCabecera1 = w.nombre;
                                                    d.CodigoCabecera1 = w.codigo;
                                                    d.IdCabecera1 = w.id;
                                                    d.CuentaCabecera3 = q.nombre;
                                                    d.CodigoCabecera3 = q.codigo;
                                                    d.IdCabecera3 = q.id;
                                                    d.CuentaCabecera4 = z.nombre;
                                                    d.CodigoCabecera4 = z.codigo;
                                                    d.IdCabecera4 = z.id;
                                                    d.CuentaCabecera5 = l.nombre;
                                                    d.CodigoCabecera5 = l.codigo;
                                                    d.IdCabecera5 = l.id;


                                                    d.CodigoCuenta = j.cuenta.codigo;
                                                    d.Cuenta = j.cuenta.nombre;

                                                    if (moneda.idMonedaPrincipal == idmoneda)
                                                    {
                                                        d.Haber = j.montoDebe - j.montoHaber;
                                                    }
                                                    else
                                                    {
                                                        d.Haber = j.montoDebeAlt - j.montoHaberAlt;
                                                    }
                                                    d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                    d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera1;
                                                    d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                    d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                    EstadoResultadosGasto.Add(d);

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
                                           && x.nivel == 3 && x.idCuentaPadre == l.id
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.cuenta
                                                where x.idEmpresa == idempresa
                                                && x.nivel == 4 && x.idCuentaPadre == z.id
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.cuenta
                                                    where x.idEmpresa == idempresa
                                                    && x.nivel == 5 && x.idCuentaPadre == q.id
                                                    select x).ToList();
                                    foreach (var w in cuentas2)
                                    {
                                        var cuentas3 = (from x in esquema.cuenta
                                                        where x.idEmpresa == idempresa
                                                        && x.nivel == 6 && x.idCuentaPadre == w.id
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
                                                    EEstadoResultadosGasto d = new EEstadoResultadosGasto();
                                                    if (e.id == j.cuenta.idCuentaPadre)
                                                    {
                                                        d.CuentaCabecera1 = e.nombre;
                                                        d.CodigoCabecera1 = e.codigo;
                                                        d.IdCabecera1 = e.id;
                                                        d.CuentaCabecera2 = w.nombre;
                                                        d.CodigoCabecera2 = w.codigo;
                                                        d.IdCabecera2 = w.id;
                                                        d.CuentaCabecera3 = q.nombre;
                                                        d.CodigoCabecera3 = q.codigo;
                                                        d.IdCabecera3 = q.id;
                                                        d.CuentaCabecera4 = z.nombre;
                                                        d.CodigoCabecera4 = z.codigo;
                                                        d.IdCabecera4 = z.id;
                                                        d.CuentaCabecera5 = l.nombre;
                                                        d.CodigoCabecera5 = l.codigo;
                                                        d.IdCabecera5 = l.id;


                                                        d.CodigoCuenta = j.cuenta.codigo;
                                                        d.Cuenta = j.cuenta.nombre;

                                                        if (moneda.idMonedaPrincipal == idmoneda)
                                                        {
                                                            d.Haber = j.montoDebe - j.montoHaber;
                                                        }
                                                        else
                                                        {
                                                            d.Haber = j.montoDebeAlt - j.montoHaberAlt;
                                                        }
                                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                        d.TotalCabecera2 = d.TotalCabecera2 + d.TotalCabecera1;
                                                        d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera2;
                                                        d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                        d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                        EstadoResultadosGasto.Add(d);

                                                    }
                                                }
                                            }

                                        }


                                    }


                                }
                            }
                        }
                    }

                    return EstadoResultadosGasto;

                }
                catch (Exception ex)
                {
                    throw new BussinessException("No se pudo obtener el Estado de Resultados");
                }

            }
        }

    }
}
