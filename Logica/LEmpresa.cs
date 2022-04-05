using Data;
using Entidad;
using Entidad.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logica
{
    public class LEmpresa : LBase<empresa>
    {
        public EEmpresa Covertir(empresa E)
        {
            EEmpresa emp = new EEmpresa();
            emp.Id = E.id;
            emp.Nombre = E.nombre;
            emp.Nit = E.nit;
            emp.Sigla = E.sigla;
            emp.Telefono = (int)E.telefono;
            emp.Correo = E.correo;
            emp.Direccion = E.direccion;
            emp.Niveles = E.niveles;
            emp.idUsuario = E.idUsuario;
            emp.Usuario = E.usuario.nombre;
            emp.Estado = E.estado;

            return emp;
        }

        public empresa Obtener_empresa(dbErpEntities esquema, long id)
        {
            return (from x in esquema.empresa
                    where x.id == id
                    select x).FirstOrDefault();
        }
        public List<EEmpresa> Obtener(int ordenar)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    List<EEmpresa> empresas = new List<EEmpresa>();
                    if (ordenar == 0)
                    {
                        var resultado = (from x in esquema.empresa
                                         where x.estado != (int)Estado.Eliminado
                                         select x).ToList();
                       
                        foreach (var item in resultado)
                        {
                            EEmpresa e = new EEmpresa();
                            e.Id = item.id;
                            e.Nombre = item.nombre;
                            e.Nit = item.nit;
                            e.Sigla = item.sigla;
                            e.Telefono = (int)item.telefono;
                            e.Correo = item.correo;
                            e.Direccion = item.direccion;
                            e.Niveles = item.niveles;
                            e.idUsuario = item.idUsuario;
                            e.Usuario = item.usuario.nombre;
                            e.Estado = item.estado;
                            empresas.Add(e);
                        }

                    }
                    else
                    {
                        var resultado = (from x in esquema.empresa
                                     where x.estado != (int)Estado.Eliminado
                                     orderby x.id descending
                                     select x).ToList();

                        
                        foreach (var item in resultado)
                        {
                            EEmpresa e = new EEmpresa();
                            e.Id = item.id;
                            e.Nombre = item.nombre;
                            e.Nit = item.nit;
                            e.Sigla = item.sigla;
                            e.Telefono = (int)item.telefono;
                            e.Correo = item.correo;
                            e.Direccion = item.direccion;
                            e.Niveles = item.niveles;
                            e.idUsuario = item.idUsuario;
                            e.Usuario = item.usuario.nombre;
                            e.Estado = item.estado;
                            empresas.Add(e);
                        }

                    }
                    

                    

                    return empresas;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EEmpresa ObtenerPorid(int id)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var resultado = (from x in esquema.empresa
                                     where x.estado != (int)Estado.Eliminado
                                     where x.id == id
                                     select x).FirstOrDefault();

                    EEmpresa emp = new EEmpresa();
                    emp.Id = resultado.id;
                    emp.Nombre = resultado.nombre;
                    emp.Nit = resultado.nit;
                    emp.Sigla = resultado.sigla;
                    emp.Telefono = (int)resultado.telefono;
                    emp.Correo = resultado.correo;
                    emp.Direccion = resultado.direccion;
                    emp.Niveles = resultado.niveles;
                    emp.idUsuario = resultado.idUsuario;
                    emp.Usuario = resultado.usuario.nombre;
                    emp.Estado = resultado.estado;

                    return emp;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EEmpresa Agregar(EEmpresa objempresa)
        {
            try
            {
                using (var esquema = GetEsquema())
                {
                    var ExisteNombre = (from x in esquema.empresa
                                        where x.estado != (int)Estado.Eliminado &&
                                        x.nombre.ToUpper() == objempresa.Nombre.ToUpper()
                                        select x).ToList();

                    if (ExisteNombre.Count > 0)
                    {
                        throw new BussinessException("El nombre de la empresa ya ha sido registrado.");
                    }


                    var Existenit = (from x in esquema.empresa
                                     where x.estado != (int)Estado.Eliminado &&
                                     x.nit == objempresa.Nit
                                     select x).ToList();

                    if (Existenit.Count > 0)
                    {
                        throw new BussinessException("El Nit ya ha sido registrado.");
                    }

                    var Existesigla = (from x in esquema.empresa
                                       where x.estado != (int)Estado.Eliminado &&
                                       x.sigla == objempresa.Sigla
                                       select x).ToList();

                    if (Existesigla.Count > 0)
                    {
                        throw new BussinessException("Las siglas ya han sido registradas.");
                    }

                    string email = objempresa.Correo;
                    var res = 0;
                    if (email != "")
                    {

                        String expresion = @"^[^@]+@[^@]+\.[a-zA-Z]{2,}$";
                        if (Regex.IsMatch(email, expresion))
                        {
                            if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                            {
                                res = 1;
                            }
                            else
                            {
                                res = 2;
                            }
                        }
                        else
                        {
                            res = 2;
                        }
                    }

                    if (res == 2)
                    {
                        throw new BussinessException("El Correo no tiene el formato correcto.");
                    }



                    empresa empresa = new empresa();
                    empresa.nombre = objempresa.Nombre;
                    empresa.nit = objempresa.Nit;
                    empresa.sigla = objempresa.Sigla;
                    empresa.telefono = objempresa.Telefono;
                    empresa.correo = objempresa.Correo;
                    empresa.direccion = objempresa.Direccion;
                    empresa.niveles = objempresa.Niveles;
                    empresa.idUsuario = objempresa.idUsuario;
                    empresa.estado = 0;
                    esquema.empresa.Add(empresa);
                    esquema.SaveChanges();

                    return objempresa;
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

        public EEmpresa Editar(EEmpresa objempresa)
        {
            try
            {
                using (var esquema = GetEsquema())
                {

                    var ExisteNombre = (from x in esquema.empresa
                                        where x.estado != (int)Estado.Eliminado &&
                                        x.id != objempresa.Id &&
                                        x.nombre.ToUpper() == objempresa.Nombre.ToUpper()
                                        select x).ToList();

                    if (ExisteNombre.Count > 0)
                    {
                        throw new BussinessException("El nombre de la empresa ya ha sido registrado.");
                    }


                    var Existenit = (from x in esquema.empresa
                                     where x.estado != (int)Estado.Eliminado &&
                                     x.nit == objempresa.Nit
                                     && x.id != objempresa.Id
                                     select x).ToList();

                    if (Existenit.Count > 0)
                    {
                        throw new BussinessException("El Nit ya ha sido registrado.");
                    }

                    var Existesigla = (from x in esquema.empresa
                                       where x.estado != (int)Estado.Eliminado &&
                                       x.sigla == objempresa.Sigla
                                       && x.id != objempresa.Id
                                       select x).ToList();

                    if (Existesigla.Count > 0)
                    {
                        throw new BussinessException("Las siglas ya han sido registradas.");
                    }

                    string email = objempresa.Correo;
                    var res = 0;
                    if (email != "")
                    {

                        String expresion = @"^[^@]+@[^@]+\.[a-zA-Z]{2,}$";
                        if (Regex.IsMatch(email, expresion))
                        {
                            if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                            {
                                res = 1;
                            }
                            else
                            {
                                res = 2;
                            }
                        }
                        else
                        {
                            res = 2;
                        }
                    }

                    if (res == 2)
                    {
                        throw new BussinessException("El Correo no tiene el formato correcto.");
                    }


                    empresa empresa = Obtener_empresa(esquema, objempresa.Id);
                    empresa.nombre = objempresa.Nombre;
                    empresa.nit = objempresa.Nit;
                    empresa.sigla = objempresa.Sigla;
                    empresa.telefono = objempresa.Telefono;
                    empresa.correo = objempresa.Correo;
                    empresa.direccion = objempresa.Direccion;          
                    esquema.SaveChanges();

                    return objempresa;
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

        public bool Eliminar(int id)
        {
            using (var esquema = GetEsquema())
            {
                var empresa = (from x in esquema.empresa
                               where x.id == id
                               select x).FirstOrDefault();

                try
                {
                    empresa.estado = 2;
                    esquema.SaveChanges();
                    return true;
                }
                catch (BussinessException ex)
                {
                    return false;
                }

            }
        }
    }
}
