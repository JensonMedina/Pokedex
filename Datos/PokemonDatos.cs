using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;

namespace Datos
{
    public class PokemonDatos
    {
        public List<Pokemon> listar()
        {
            List<Pokemon> lista = new List<Pokemon>();
            SqlConnection Conexion = new SqlConnection();
            SqlCommand Comando = new SqlCommand();
            SqlDataReader Lector;

            try
            {
                Conexion.ConnectionString = "server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true";
                Comando.CommandType = System.Data.CommandType.Text;
                Comando.CommandText = "Select P.Id, Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad From POKEMONS P, ELEMENTOS E, ELEMENTOS D where E.Id = P.IdTipo AND D.Id = P.IdDebilidad AND Activo = 1";
                Comando.Connection = Conexion;

                Conexion.Open();
                Lector = Comando.ExecuteReader();

                while (Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)Lector["Id"];
                    aux.Numero = (int)Lector["Numero"];
                    aux.Nombre = (string)Lector["Nombre"];
                    aux.Descripcion = (string)Lector["Descripcion"];

                    if(!(Lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)Lector["UrlImagen"];

                    aux.Tipo = new Elemento();
                    aux.Tipo.Id = (int)Lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)Lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)Lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)Lector["Debilidad"];

                    lista.Add(aux);
                }

                Conexion.Close();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Agregar(Pokemon Nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setQuery("Insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen)values(" + Nuevo.Numero + ", '" + Nuevo.Nombre + "', '" + Nuevo.Descripcion + "', 1, @IdTipo, @IdDebilidad, @UrlImagen)");
                datos.setParametros("@IdTipo", Nuevo.Tipo.Id);
                datos.setParametros("@IdDebilidad", Nuevo.Debilidad.Id);
                datos.setParametros("@UrlImagen", Nuevo.UrlImagen);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public void Modificar(Pokemon modificado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setQuery("update POKEMONS set Numero = @numero, Nombre = @nombre, Descripcion = @descripcion, UrlImagen = @urlimagen, IdTipo = @idtipo, IdDebilidad = @iddebilidad where Id = @id");
                datos.setParametros("@numero", modificado.Numero);
                datos.setParametros("@nombre", modificado.Nombre);
                datos.setParametros("@descripcion", modificado.Descripcion);
                datos.setParametros("@urlimagen", modificado.UrlImagen);
                datos.setParametros("@idtipo", modificado.Tipo.Id);
                datos.setParametros("@iddebilidad", modificado.Debilidad.Id);
                datos.setParametros("@id", modificado.Id);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public void EliminarFisico(int Id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setQuery("delete POKEMONS where Id = @id");
                datos.setParametros("@id", Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void EliminarLogico(int Id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setQuery("update POKEMONS set Activo = 0 where Id = @id");
                datos.setParametros("@id", Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Pokemon> Filtrar(string campo, string criterio, string filtro)
        {
            List<Pokemon> lista = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad And P.Activo = 1 And ";

                if (campo == "Número")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Numero > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Numero < " + filtro;
                            break;
                        default:
                            consulta += "Numero = " + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "P.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "P.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "P.Descripcion like '%" + filtro + "%' ";
                            break;
                    }
                }

                datos.setQuery(consulta);
                datos.EjecutarLectura();

                while (datos.lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)datos.lector["Id"];
                    aux.Numero = (int)datos.lector["Numero"];
                    aux.Nombre = (string)datos.lector["Nombre"];
                    aux.Descripcion = (string)datos.lector["Descripcion"];

                    if (!(datos.lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)datos.lector["UrlImagen"];

                    aux.Tipo = new Elemento();
                    aux.Tipo.Id = (int)datos.lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)datos.lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)datos.lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)datos.lector["Debilidad"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

}
