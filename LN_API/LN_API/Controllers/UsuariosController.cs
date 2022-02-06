using LN_API.Entities;
using LN_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LN_API.Controllers
{
    public class UsuariosController : ApiController
    {
        //Controlador

        public static List<UsuariosObj> usuarios = new List<UsuariosObj>();
        UsuariosModel logicaUsuarios = new UsuariosModel();

        [HttpGet]
        [Route("api/Usuarios/ObtenerUsuarios")]
        public RespuestaUsuarios Obtener()
        {
            try
            {
                RespuestaUsuarios respuesta = new RespuestaUsuarios();
                respuesta.datos = usuarios;
                respuesta.codigoRespuesta = 0;
                respuesta.descripcion = "OK";
                return respuesta;
            }
            catch (Exception ex)
            {
                RespuestaUsuarios respuesta = new RespuestaUsuarios();
                respuesta.datos = null;
                respuesta.codigoRespuesta = -1;
                respuesta.descripcion = ex.Message;
                return respuesta;
            }
        }

        [HttpGet]
        [Route("api/Usuarios/ObtenerUsuario")]
        public List<UsuariosObj> Obtener(long Consecutivo)
        {
            try
            {
                //LinQ
                return usuarios.Where(x => x.Consecutivo == Consecutivo).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("api/Usuarios/RegistrarUsuario")]
        public List<UsuariosObj> Registrar(UsuariosObj usuarioRegistro)
        {
            try
            {
                usuarios.Add(usuarioRegistro);
                return usuarios;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpDelete]
        [Route("api/Usuarios/BorrarUsuario")]
        public List<UsuariosObj> Borrar(long Consecutivo)
        {
            try
            {
                var consulta = usuarios.Where(x => x.Consecutivo == Consecutivo).FirstOrDefault();
                usuarios.Remove(consulta);
                return usuarios;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPut]
        [Route("api/Usuarios/ActualizarUsuario")]
        public List<UsuariosObj> Actualizar(UsuariosObj usuarioRegistro)
        {
            try
            {
                var consulta = usuarios.Where(x => x.Consecutivo == usuarioRegistro.Consecutivo).FirstOrDefault();

                if (consulta != null)
                {
                    consulta.Nombre = usuarioRegistro.Nombre;
                    consulta.Password = usuarioRegistro.Password;
                    consulta.Username = usuarioRegistro.Username;
                    return usuarios;
                }
                else
                {
                    return new List<UsuariosObj>();
                }

            }
            catch (Exception)
            {
                return null;
            }

        }

    }
}