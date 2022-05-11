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
    }
}
