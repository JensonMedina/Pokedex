using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Datos
{
    public class ElementosDatos
    {
        public List<Elemento>listar()
        {
            List<Elemento> lista = new List<Elemento>();
            AccesoDatos Datos = new AccesoDatos();
            try
            {
                Datos.setQuery("select Id, Descripcion from ELEMENTOS");
                Datos.EjecutarLectura();
                while (Datos.lector.Read())
                    {
                        Elemento aux = new Elemento();
                        aux.Id = (int)Datos.lector["Id"];
                        aux.Descripcion = (string)Datos.lector["Descripcion"];

                        lista.Add(aux);
                    }
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                Datos.CerrarConexion();
            }
        }
    }
}
