using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
	public class RepositorioInquilino : RepositorioBase, IRepositorio<Inquilino>
	{
		public RepositorioInquilino(IConfiguration configuration) : base(configuration)
		{
			
		}

		public int Alta(Inquilino p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inquilino (Dni, Apellido, Nombre, Direccion, Telefono) " +
					$"VALUES ('{p.Dni}', '{p.Apellido}','{p.Nombre}','{p.Direccion}', '{p.Telefono}')";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
					var id = command.ExecuteScalar();
					p.IdInquilino = Convert.ToInt32(id);
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
				string sql = $"DELETE FROM Inquilino WHERE IdInquilino = {id}";
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
		public int Modificacion(Inquilino p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Inquilino SET Dni='{p.Dni}', Apellido='{p.Apellido}', Nombre='{p.Nombre}', Direccion='{p.Direccion}', Telefono='{p.Telefono}' " +
					$"WHERE IdInquilino = {p.IdInquilino}";
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

		public IList<Inquilino> ObtenerTodos()
		{
			IList<Inquilino> res = new List<Inquilino>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdInquilino, Dni, Apellido, Nombre, Direccion, Telefono" +
                    $" FROM Inquilino";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inquilino p = new Inquilino
						{
							IdInquilino = reader.GetInt32(0),
                            Dni = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Nombre = reader.GetString(3),
							Direccion = reader.GetString(4),
							Telefono = reader.GetString(5)	
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Inquilino ObtenerPorId(int id)
		{
			Inquilino p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdInquilino, Dni, Apellido, Nombre, Direccion, Telefono FROM Inquilino" +
					$" WHERE IdInquilino=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						p = new Inquilino
						{
                            IdInquilino = reader.GetInt32(0),
                            Dni = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Nombre = reader.GetString(3),
                            Direccion = reader.GetString(4),
                            Telefono = reader.GetString(5)
                        };
						return p;
					}
					connection.Close();
				}
			}
			return p;
		}
	}
}
