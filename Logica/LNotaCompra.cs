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
    }
}
