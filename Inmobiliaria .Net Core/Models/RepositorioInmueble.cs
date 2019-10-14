using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
	public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
	{
		public RepositorioInmueble(IConfiguration configuration) : base(configuration)
		{

		}

		public int Alta(Inmueble p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inmueble (Direccion, Ambientes, Tipo, Uso, Precio, Disponible, IdPropietario) " +
					$"VALUES ('{p.Direccion}', '{p.Ambientes}', '{p.Tipo}', '{p.Uso}', '{p.Precio}', '{p.Disponible}', '{p.IdPropietario}')";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    p.IdInmueble = Convert.ToInt32(id);
					connection.Close();
				}
			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Inmueble WHERE IdInmueble = {id}";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}		

		public int Modificacion(Inmueble inmueble)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Inmueble SET Direccion=@direccion, Ambientes=@ambientes, Tipo=@tipo, Uso=@uso, Precio=@precio, Disponible=@disponible " +
					$"WHERE IdInmueble = {inmueble.IdInmueble}";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@direccion", SqlDbType.VarChar).Value = inmueble.Direccion;
					command.Parameters.Add("@ambientes", SqlDbType.Int).Value = inmueble.Ambientes;
					command.Parameters.Add("@tipo", SqlDbType.VarChar).Value = inmueble.Tipo;
					command.Parameters.Add("@uso", SqlDbType.VarChar).Value = inmueble.Uso;
					command.Parameters.Add("@precio", SqlDbType.Int).Value = inmueble.Precio;
					command.Parameters.Add("@disponible", SqlDbType.VarChar).Value = inmueble.Disponible;
					//command.Parameters.Add("@idpropietario", SqlDbType.Int).Value = inmueble.IdPropietario;
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Inmueble> BuscarDisponibles()
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdInmueble, Direccion, Ambientes, Tipo, Uso, Precio, Disponible, Inmueble.IdPropietario, Propietario.Nombre, Propietario.Apellido FROM Inmueble INNER JOIN Propietario ON Propietario.IdPropietario=Inmueble.IdPropietario WHERE Disponible='Si'";

				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble i = new Inmueble
						{
							IdInmueble = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Tipo = reader.GetString(3),
							Uso = reader.GetString(4),
							Precio = reader.GetInt32(5),
							Disponible = reader.GetString(6),
							Propietario = new Propietario
							{
								IdPropietario = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9)
							}
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}


		public IList<Inmueble> ObtenerTodos()
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
                string sql = $"SELECT IdInmueble, Direccion, Ambientes, Tipo, Uso, Precio, Disponible, Inmueble.IdPropietario, Propietario.Nombre, Propietario.Apellido FROM Inmueble INNER JOIN Propietario ON Propietario.IdPropietario=Inmueble.IdPropietario";
                   
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
                        Inmueble i = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Tipo = reader.GetString(3),
                            Uso = reader.GetString(4),
                            Precio = reader.GetInt32(5),
                            Disponible = reader.GetString(6),
                            Propietario = new Propietario
                            {
                                IdPropietario = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9)
                            } 
                    };
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Inmueble ObtenerPorId(int id)
		{
			Inmueble p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdInmueble, Direccion, Ambientes, Tipo, Uso, Precio, Disponible, Inmueble.IdPropietario, Propietario.Nombre, Propietario.Apellido FROM Inmueble JOIN Propietario ON(Propietario.IdPropietario=Inmueble.IdPropietario)" +
					$" WHERE IdInmueble=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
						p = new Inmueble
						{
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Tipo = reader.GetString(3),
                            Uso = reader.GetString(4),
                            Precio = reader.GetInt32(5),
                            Disponible = reader.GetString(6),
                            Propietario = new Propietario
                            {
                                IdPropietario = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9)
                            }
                            
                        };
					}
					connection.Close();
				}
			}
			return p;
		}

        public IList<Inmueble> BuscarPorPropietario(int idPropietario)
        {
            List<Inmueble> res = new List<Inmueble>();
            Inmueble entidad = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInmueble, Direccion, Ambientes, Tipo, Uso, Precio, Disponible, Inmueble.IdPropietario, Propietario.Nombre, Propietario.Apellido" +
                    $" FROM Inmueble INNER JOIN Propietario ON Inmueble.IdPropietario = Propietario.IdPropietario" +
                    $" WHERE IdPropietario=@idPropietario";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@idPropietario", SqlDbType.Int).Value = idPropietario;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        entidad = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Tipo = reader.GetString(3),
                            Uso = reader.GetString(4),
                            Precio = reader.GetInt32(5),
                            Disponible = reader.GetString(6),
                            Propietario = new Propietario
                            {
                                IdPropietario = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9)
                            }
                        };
                        res.Add(entidad);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
