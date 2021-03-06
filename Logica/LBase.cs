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
    public class LBase<E> : LBaseBase<dbErpEntities, E>
    {
        protected override dbErpEntities GetEsquema()
        {
            return new dbErpEntities();
        }

        private static LBase<E> instacia;

        private LLogin lLogin;
        private LGestion lGestion;
        private LEmpresa lEmpresa;
        private LPeriodo lPeriodo;
        private LCuenta lCuenta;
        private LEmpresaMoneda lEmpresaMoneda;
        private LComprobante lComprobante;
        private LReporte lReporte;
        private LBalanceInicial lBalanceInicial;
        private LCategoria lCategoria;
        private LArticulo lArticulo;
        private LNotaCompra lNotaCompra;
        private LNotaVenta lNotaVenta;
        private LEstadoResultados lEstadoResultados;
        private LRBalanceGeneral lRBalanceGeneral;

        public static LBase<E> Instancia
        {
            get
            {
                if (instacia == null)
                {
                    instacia = new LBase<E>();
                }
                return instacia;
            }
        }

        public LLogin LLogin
        {
            get
            {
                if (lLogin == null)
                {
                    lLogin = new LLogin();
                }
                return lLogin;
            }
        }
        public LGestion LGestion
        {
            get
            {
                if (lGestion == null)
                {
                    lGestion = new LGestion();
                }
                return lGestion;
            }
        }

        public LEmpresa LEmpresa
        {
            get
            {
                if (lEmpresa == null)
                {
                    lEmpresa = new LEmpresa();
                }
                return lEmpresa;
            }
        }

        public LPeriodo LPeriodo
        {
            get
            {
                if (lPeriodo == null)
                {
                    lPeriodo = new LPeriodo();
                }
                return lPeriodo;
            }
        }

        public LCuenta LCuenta
        {
            get
            {
                if(lCuenta == null)
                {
                    lCuenta = new LCuenta();
                }
                return lCuenta;
            }
        }

        public LEmpresaMoneda LEmpresaMoneda
        {
            get
            {
                if (lEmpresaMoneda == null)
                {
                    lEmpresaMoneda = new LEmpresaMoneda();
                }
                return lEmpresaMoneda;
            }
        }

        public LComprobante LComprobante
        {
            get
            {
                if(lComprobante == null)
                {
                    lComprobante = new LComprobante();
                }
                return lComprobante;
            }
        }
        public LReporte LReporte
        {
            get
            {
                if (lReporte == null)
                {
                    lReporte = new LReporte();
                }
                return lReporte;
            }
        }
        public LBalanceInicial LBalanceInicial
        {
            get
            {
                if (lBalanceInicial == null)
                {
                    lBalanceInicial = new LBalanceInicial();
                }
                return lBalanceInicial;
            }
        }

        public LCategoria LCategoria
        {
            get
            {
                if(lCategoria == null)
                {
                    lCategoria = new LCategoria();
                }
                return lCategoria;
            }
        }
        
        public LArticulo LArticulo
        {
            get
            {
                if(lArticulo == null)
                {
                    lArticulo = new LArticulo();
                }
                return lArticulo;
            }
        }

        public LNotaCompra LNotaCompra
        {
            get
            {
                if (lNotaCompra == null)
                {
                    lNotaCompra = new LNotaCompra();
                }
                return lNotaCompra;
            }
        }

        public LNotaVenta LNotaVenta
        {
            get
            {
                if (lNotaVenta == null)
                {
                    lNotaVenta = new LNotaVenta();
                }
                return lNotaVenta;
            }
        }

        public LEstadoResultados LEstadoResultados
        {
            get
            {
                if (lEstadoResultados == null)
                {
                    lEstadoResultados = new LEstadoResultados();
                }
                return lEstadoResultados;
            }
        }

        public LRBalanceGeneral LRBalanceGeneral
        {
            get
            {
                if (lRBalanceGeneral == null)
                {
                    lRBalanceGeneral = new LRBalanceGeneral();
                }
                return lRBalanceGeneral;
            }
        }
    }
}
