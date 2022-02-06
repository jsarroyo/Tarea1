
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LN_API.Entities
{
    public class UsuariosObj
    {
        //Objeto
        public long Consecutivo { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
    }

    public class RespuestaUsuarios
    {
        public int codigoRespuesta { get; set; }
        public string descripcion { get; set; }
        public List<UsuariosObj> datos { get; set; }
    }

}