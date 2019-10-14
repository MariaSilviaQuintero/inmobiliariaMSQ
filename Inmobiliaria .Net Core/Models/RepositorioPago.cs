using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Inmobiliaria_.Net_Core.Models
{
	public class RepositorioPago : RepositorioBase, IRepositorio<Pago>
	{
        public RepositorioPago(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Pago p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
				string sql = $"INSERT INTO Pago (NroPago, IdAlquiler, Fecha, Importe) " +
                    $"VALUES ('{p.NroPago}', '{p.IdAlquiler}', '{p.Fecha}', '{p.Importe}')";

				using (SqlCommand command = new SqlCommand(sql, connection))
                {
					command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    p.IdPago = Convert.ToInt32(id);
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
                string sql = $"DELETE FROM Pago WHERE IdPago = {id}";
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

        public int Modificacion(Pago pago)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
				string sql = $"UPDATE Pago SET Importe=@importe " +
                    $"WHERE IdPago = {pago.IdPago}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    //command.Parameters.Add("@nropago", SqlDbType.Int).Value = pago.NroPago;
                    //command.Parameters.Add("@idalquiler", SqlDbType.Int).Value = pago.IdAlquiler;
                    //command.Parameters.Add("@fecha", SqlDbType.VarChar).Value = pago.Fecha;
                    command.Parameters.Add("@importe", SqlDbType.Int).Value = pago.Importe;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Pago> ObtenerTodos()
        {
            IList<Pago> res = new List<Pago>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPago, NroPago, Pago.IdAlquiler, Alquiler.IdInmueble, Inmueble.Direccion, Alquiler.IdInquilino, Inquilino.Nombre, Inquilino.Apellido, Fecha, Pago.Importe FROM Pago INNER JOIN Alquiler ON Alquiler.IdAlquiler=Pago.IdAlquiler INNER JOIN Inmueble ON Inmueble.IdInmueble = Alquiler.IdInmueble INNER JOIN Inquilino ON Inquilino.IdInquilino = Alquiler.IdInquilino";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago pago = new Pago
                        {
                            IdPago = reader.GetInt32(0),
                            NroPago = reader.GetInt32(1),
                            Alquiler = new Alquiler
                            {
                                IdAlquiler = reader.GetInt32(2),
								Inmueble = new Inmueble
								{
									IdInmueble = reader.GetInt32(3),
									Direccion = reader.GetString(4),
								},
								Inquilino = new Inquilino
								{
									IdInquilino = reader.GetInt32(5),
									Nombre = reader.GetString(6),
									Apellido = reader.GetString(7)
								}
                            },
                            Fecha = reader.GetString(8),
                            Importe = reader.GetInt32(9)
                        };
                        res.Add(pago);
                    }
                    connection.Close();
                }
            }
            return res;
        }

		public Pago ObtenerPorId(int id)
		{
			Pago p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdPago, NroPago, Pago.IdAlquiler, Alquiler.IdInmueble, Inmueble.Direccion, Alquiler.IdInquilino, Inquilino.Nombre, Inquilino.Apellido, Fecha, Pago.Importe FROM Pago INNER JOIN Alquiler ON Alquiler.IdAlquiler=Pago.IdAlquiler INNER JOIN Inmueble ON Inmueble.IdInmueble = Alquiler.IdInmueble INNER JOIN Inquilino ON Inquilino.IdInquilino = Alquiler.IdInquilino WHERE IdPago=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Pago
						{
							IdPago = reader.GetInt32(0),
							NroPago = reader.GetInt32(1),
							Alquiler = new Alquiler
							{
								IdAlquiler = reader.GetInt32(2),
								Inmueble = new Inmueble
								{
									IdInmueble = reader.GetInt32(3),
									Direccion = reader.GetString(4),
								},
								Inquilino = new Inquilino
								{
									IdInquilino = reader.GetInt32(5),
									Nombre = reader.GetString(6),
									Apellido = reader.GetString(7)
								}
							},
							Fecha = reader.GetString(8),
							Importe = reader.GetInt32(9)
						};
					}
					connection.Close();
				}
			}
			return p;
		}
	}
}
