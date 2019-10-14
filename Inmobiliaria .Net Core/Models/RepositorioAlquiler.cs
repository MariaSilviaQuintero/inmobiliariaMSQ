using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
    public class RepositorioAlquiler : RepositorioBase, IRepositorio<Alquiler>
    { 
        public RepositorioAlquiler(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Alquiler a)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Alquiler (Importe, FechaInicio, FechaFin, IdInquilino, IdInmueble) " +
                    $"VALUES ('{a.Importe}', '{a.FechaInicio}','{a.FechaFin}', '{a.IdInquilino}', '{a.IdInmueble}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
					var id = command.ExecuteScalar();
					a.IdAlquiler = Convert.ToInt32(id);
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
                string sql = $"DELETE FROM Alquiler WHERE IdAlquiler = {id}";
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
  
		public int Modificacion(Alquiler a)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Alquiler SET Importe=@importe, FechaInicio=@fechainicio, FechaFin=@fechafin " +
					$"WHERE IdAlquiler = {a.IdAlquiler}";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@importe", SqlDbType.Decimal).Value = a.Importe;
					command.Parameters.Add("@fechainicio", SqlDbType.VarChar).Value = a.FechaInicio;
					command.Parameters.Add("@fechafin", SqlDbType.VarChar).Value = a.FechaFin;
					//command.Parameters.Add("@idinquilino", SqlDbType.Int).Value = a.IdInquilino;
					//command.Parameters.Add("@idinmueble", SqlDbType.Int).Value = a.IdInmueble;
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Alquiler> ObtenerTodos()
        {
            IList<Alquiler> res = new List<Alquiler>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdAlquiler, Importe, FechaInicio, FechaFin, Alquiler.IdInquilino, Alquiler.IdInmueble, Inquilino.Nombre, Inquilino.Apellido, Inmueble.Direccion FROM Alquiler INNER JOIN Inquilino ON Inquilino.IdInquilino=Alquiler.IdInquilino INNER JOIN Inmueble ON Inmueble.IdInmueble=Alquiler.IdInmueble";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Alquiler a = new Alquiler
                        {
                            IdAlquiler = reader.GetInt32(0),
                            Importe = reader.GetInt32(1),
                            FechaInicio = reader.GetString(2),
                            FechaFin = reader.GetString(3),
                            Inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(4),
                                Nombre = reader.GetString(6),
                                Apellido = reader.GetString(7),
                            },
                            Inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32(5),
                                Direccion = reader.GetString(8)
                            }
                        };
                        res.Add(a);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Alquiler ObtenerPorId(int id)
        {
            Alquiler p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdAlquiler, Importe, FechaInicio, FechaFin, Alquiler.IdInquilino, Alquiler.IdInmueble, Inquilino.Nombre, Inquilino.Apellido, Inmueble.Direccion FROM Alquiler JOIN Inquilino ON(Inquilino.IdInquilino=Alquiler.IdInquilino) JOIN Inmueble ON(Inmueble.IdInmueble=Alquiler.IdInmueble)" +
                    $" WHERE IdAlquiler=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        p = new Alquiler
                        {
                            IdInmueble = reader.GetInt32(0),
                            Importe = reader.GetInt32(1),
                            FechaInicio = reader.GetString(2),
                            FechaFin = reader.GetString(3),
                            Inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(4),
                                Nombre = reader.GetString(6),
                                Apellido = reader.GetString(7),
                            },
                            Inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32(5),
                                Direccion = reader.GetString(8)
                            }
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

